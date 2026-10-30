// =====================================================================================
//  Score_Manager.cs   (CognitiveTestsVR version)
// -------------------------------------------------------------------------------------
//  Purpose   : Writes the session SUMMARY CSV (one row per session) for Corsi VR and
//              Deary-Liewald VR, and creates the per-trial CSV (TrialLogger).
//  Layer     : Runtime, data output (on the ExperimentManager object).
//  Depends on: CorsiBlockManager, Corsi_ET_Manager, Corsi_TimeManager, Deary_Manager,
//              DigitRecallManager (kept for compatibility), CsvUtil, SessionInfo,
//              VariantConfig, GazePointerDriver, TrialLogger.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2024-05     original  ACG_Final version (PlayerPrefs auto-increment ID, CSV next to exe).
//    2026-10-29  v2.0      CognitiveTestsVR rebuild (all serialized fields kept, so the scenes
//                          copied from ACG_Final keep their references):
//      * Participant ID: waits for the experimenter-entered ID (ParticipantIdKeypad) and for
//        every other start-up step (SessionGate, e.g. Varjo calibration) BEFORE creating the
//        CSV. An empty ID falls back to the original PlayerPrefs auto-increment.
//      * File names: scoresID<ID>_<Test>_<Variant>_<yyyyMMdd_HHmmss>.csv (+ trialsID... for the
//        per-trial log). The timestamp makes overwriting impossible.
//      * Location: unchanged on PC (next to the .exe; Assets/ in the Editor). On Android the
//        old location was inside the read-only APK, so files now go to
//        Application.persistentDataPath (= /sdcard/Android/data/<package>/files/).
//      * Numbers are written with a dot decimal separator on every PC locale (CsvUtil).
//      * Values that could not be measured are written as "NA" (no more 0 / NaN / crash):
//        gaze metrics when the variant has no (working) eye tracker, pupil size when no
//        valid sample was collected.
//      * The original columns keep their names and order. New columns are only appended:
//        Corsi trial/planning/motor times (were computed but never saved), Deary accuracy
//        counts, and session descriptors (Test, Variant, Platform, EyeTracker, GazeSource,
//        ETCalibration, Completed, SessionStart, AppVersion).
// =====================================================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Score_Manager : MonoBehaviour
{
    [System.Serializable]
    public class VariableEntry
    {
        public string variableName;
        public enum VariableType { Int, Float, String }
        public VariableType variableType;
    }

    [Tooltip("CSV columns in order. The scene builder fills this list; values not listed here are not written.")]
    public List<VariableEntry> variables = new List<VariableEntry>();

    private Dictionary<string, string> values = new Dictionary<string, string>();
    private List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
    public CorsiBlockManager corsiF;
    public Corsi_ET_Manager corsiF_Eye;
    public CorsiBlockManager corsiB;
    public Corsi_ET_Manager corsiB_Eye;
    public Deary_Manager deary;
    public DigitRecallManager digitF;
    public DigitRecallManager digitB;
    // Path for the CSV file
    public string csvFilePath;
    public string fileName;
    public int ParticipantID;
    public int TaskNumber;
    public bool CSVInitialization = false;

    [Tooltip("'Pupillometry available' (Pro Eye or Varjo). Name kept for scene compatibility.")]
    public bool ProEye = false;

    // Guards against writing the summary twice (task end + Escape key).
    private bool saved;

    private void Start()
    {
        StartCoroutine(InitialiseWhenReady());
    }

    /// <summary>Waits for the participant ID (and e.g. Varjo calibration), then creates both CSV files.
    /// Task_Selection_Order starts the task only when CSVInitialization becomes true.</summary>
    private IEnumerator InitialiseWhenReady()
    {
        // One frame so that every Awake()/Start() had the chance to register in SessionGate.
        yield return null;
        yield return new WaitUntil(() => SessionGate.AllReleased);

        ParticipantID = SessionInfo.ParticipantId > 0 ? SessionInfo.ParticipantId : NextAutomaticId();

        // Base directory: Editor -> Assets/, PC player -> folder of the .exe (unchanged),
        // Android -> persistentDataPath (the APK itself is read-only).
        string basePath;
        if (Application.isEditor) basePath = Application.dataPath;
        else if (Application.platform == RuntimePlatform.Android) basePath = Application.persistentDataPath;
        else basePath = Path.GetDirectoryName(Application.dataPath);

        var vc = VariantConfig.Instance;
        string suffix = (vc != null ? "_" + vc.TestLabel + "_" + vc.VariantLabel : "") + "_" + SessionInfo.SessionStamp;
        fileName = "scoresID" + ParticipantID + suffix + ".csv";
        csvFilePath = Path.Combine(basePath, fileName);

        InitializeCSV();
        TrialLogger.Init(Path.Combine(basePath, "trialsID" + ParticipantID + suffix + ".csv"), ParticipantID);
        Debug.Log("[Score_Manager] Participant " + ParticipantID + " -> " + csvFilePath);
    }

    /// <summary>The ORIGINAL numbering: PlayerPrefs counter, incremented once per session.
    /// Only used when the experimenter confirms an empty participant ID.</summary>
    private int NextAutomaticId()
    {
        const string idKey = "uniqueID";
        int id = PlayerPrefs.HasKey(idKey) ? PlayerPrefs.GetInt(idKey) + 1 : 1;
        PlayerPrefs.SetInt(idKey, id);
        PlayerPrefs.Save();
        return id;
    }

    // ------------------------------------------------------------------ summary row

    public IEnumerator SaveScore()
    {
        // Escape pressed before the CSV exists, or summary already written -> nothing to do.
        if (!CSVInitialization || saved) yield break;
        saved = true;

        var vc = VariantConfig.Instance;
        var gz = GazePointerDriver.Instance;
        // Gaze-derived metrics are only meaningful if a real eye tracker delivered valid data.
        bool gazeValid = gz != null && gz.GazeEverValid;
        bool completed = false;

        UpdateVariable("ParticipantID", ParticipantID);

        switch (TaskNumber)
        {
            case 0:  // Deary
                completed = deary != null && deary.DONE;
                UpdateVariable("SingleReactionTime", deary.MotorTimeSingle);
                UpdateVariable("ChoiceReactionTime", deary.MotorTime);
                UpdateVariable("AttentionTime", gazeValid ? deary.AttentionTime : float.NaN);
                UpdateVariable("MotorTime", gazeValid ? deary.PureMotorTime : float.NaN);
                UpdateVariable("DearyPupilSize", ProEye ? MeanOfEyes(deary.PupilSizeL, deary.PupilSizeR) : float.NaN);
                // New (appended) accuracy columns - the counters already existed in Deary_Manager.
                UpdateVariable("SingleCorrect", deary.scoreSingle);
                UpdateVariable("SingleSlow", deary.slowResponsesSingle);
                UpdateVariable("ChoiceCorrect", deary.score);
                UpdateVariable("ChoiceErrors", deary.mistakes);
                UpdateVariable("ChoiceSlow", deary.slowResponses);
                break;

            case 1:  // Corsi
                completed = corsiB != null && corsiB.DONE;
                UpdateVariable("CorsiForwardTotalScore", corsiF.TotalScore);
                UpdateVariable("CorsiForwardSpan", corsiF.span);
                UpdateVariable("CorsiForwardFixationTimes", gazeValid ? corsiF_Eye.AvgFixationCount : float.NaN);
                UpdateVariable("CorsiForwardFixationDuration", gazeValid ? corsiF_Eye.AvgFixationDuration : float.NaN);
                UpdateVariable("CorsiForwardPupilSize", ProEye ? MeanOfEyes(corsiF_Eye.PupilSizeL, corsiF_Eye.PupilSizeR) : float.NaN);
                UpdateVariable("CorsiBackwardTotalScore", corsiB.TotalScore);
                UpdateVariable("CorsiBackwardSpan", corsiB.span);
                UpdateVariable("CorsiBackwardFixationTimes", gazeValid ? corsiB_Eye.AvgFixationCount : float.NaN);
                UpdateVariable("CorsiBackwardFixationDuration", gazeValid ? corsiB_Eye.AvgFixationDuration : float.NaN);
                UpdateVariable("CorsiBackwardPupilSize", ProEye ? MeanOfEyes(corsiB_Eye.PupilSizeL, corsiB_Eye.PupilSizeR) : float.NaN);
                // New (appended) timing columns - Corsi_TimeManager computed them but they were never saved.
                UpdateVariable("CorsiForwardAvgTrialTime", corsiF.TimeMan.AvgTrialTime);
                UpdateVariable("CorsiForwardAvgPlanningTime", corsiF.TimeMan.AvgPlanningTime);
                UpdateVariable("CorsiForwardAvgMotorTime", corsiF.TimeMan.AvgMotorTime);
                UpdateVariable("CorsiBackwardAvgTrialTime", corsiB.TimeMan.AvgTrialTime);
                UpdateVariable("CorsiBackwardAvgPlanningTime", corsiB.TimeMan.AvgPlanningTime);
                UpdateVariable("CorsiBackwardAvgMotorTime", corsiB.TimeMan.AvgMotorTime);
                break;

            case 2:  // Digit (not used by CognitiveTestsVR scenes; kept so the class stays compatible)
                completed = digitB != null && digitB.DONE;
                UpdateVariable("DigitForwardTotalScore", digitF.totalScore);
                UpdateVariable("DigitForwardSpan", digitF.span);
                UpdateVariable("DigitBackwardTotalScore", digitB.totalScore);
                UpdateVariable("DigitBackwardSpan", digitB.span);
                UpdateVariable("DigitForwardPupilSize", ProEye ? MeanOfEyes(digitF.AvgPupilL, digitF.AvgPupilR) : float.NaN);
                UpdateVariable("DigitBackwardPupilSize", ProEye ? MeanOfEyes(digitB.AvgPupilL, digitB.AvgPupilR) : float.NaN);
                break;
        }

        // Session descriptors (appended columns, identical for both tests).
        UpdateVariable("Test", vc != null ? vc.TestLabel : CsvUtil.NA);
        UpdateVariable("Variant", vc != null ? vc.VariantLabel : CsvUtil.NA);
        UpdateVariable("Platform", vc != null ? vc.PlatformLabel : CsvUtil.NA);
        UpdateVariable("EyeTracker", vc != null ? vc.EyeTrackerLabel : CsvUtil.NA);
        UpdateVariable("GazeSource", gz != null ? (gazeValid ? gz.GazeSource : "None") : "None");
        UpdateVariable("ETCalibration", SessionInfo.CalibrationQuality);
        UpdateVariable("Completed", completed ? 1 : 0);
        UpdateVariable("SessionStart", SessionInfo.SessionStart.ToString("yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
        UpdateVariable("AppVersion", vc != null ? vc.appVersion : CsvUtil.NA);

        // Add the current row of data to the CSV (synchronously, before the first yield, so the
        // row is on disk even if Application.Quit() follows immediately after an Escape press).
        FinalizeAndAddCurrentRow();

        // Wait for the end of the frame to ensure all data is written to the CSV
        yield return new WaitForEndOfFrame();

        // Optional: Log the path to the CSV file
        Debug.Log("CSV file path: " + csvFilePath);
        yield return null;
    }

    /// <summary>Mean of left/right pupil; if only one eye is valid that eye is used; NaN if none.</summary>
    private static float MeanOfEyes(float left, float right)
    {
        bool l = !float.IsNaN(left) && left > 0f;
        bool r = !float.IsNaN(right) && right > 0f;
        if (l && r) return (left + right) / 2f;
        if (l) return left;
        if (r) return right;
        return float.NaN;
    }

    private void InitializeCSV()
    {
        StringBuilder csvContent = new StringBuilder();
        string delimiter = ",";

        foreach (var variable in variables)
        {
            csvContent.Append(variable.variableName + delimiter);
        }

        if (csvContent.Length > 0) csvContent.Length--; // Remove the last delimiter
        csvContent.AppendLine(); // Add new line for the first row of data

        File.WriteAllText(csvFilePath, csvContent.ToString());
        CSVInitialization = true;
    }

    // Function to add a new row of data
    public void AddNewRow(Dictionary<string, string> newRow)
    {
        StringBuilder newRowContent = new StringBuilder();
        string delimiter = ",";

        foreach (var variable in variables)
        {
            if (newRow.TryGetValue(variable.variableName, out string value))
            {
                newRowContent.Append(value + delimiter);
            }
            else
            {
                newRowContent.Append(CsvUtil.NA + delimiter); // not measured in this task/variant
            }
        }

        if (newRowContent.Length > 0) newRowContent.Length--; // Remove the last delimiter
        newRowContent.AppendLine(); // Add new line for the next row of data

        File.AppendAllText(csvFilePath, newRowContent.ToString());
    }

    /// <summary>Stores a value for the current row. Formatting is culture-invariant (dot decimal)
    /// and NaN/Infinity become "NA" (see CsvUtil).</summary>
    public void UpdateVariable(string name, object value)
    {
        values[name] = CsvUtil.Format(value);
    }

    // Call this to finalize and add the current row of data to the CSV
    public void FinalizeAndAddCurrentRow()
    {
        AddNewRow(values);
        values.Clear(); // Clear current values for the next row
    }
}
