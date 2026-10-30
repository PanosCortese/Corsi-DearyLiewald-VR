// =====================================================================================
//  TrialLogger.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Per-trial CSV log (one row per Deary-Liewald stimulus / per Corsi sequence),
//              written next to the summary CSV:  trialsID<ID>_<Test>_<Variant>_<stamp>.csv
//              The summary CSV (Score_Manager) keeps the original averaged columns; this file
//              adds the trial-level data needed for RT distributions, outlier trimming,
//              practice-vs-test comparisons and learning effects.
//
//  Columns (identical for both tests; non-applicable cells = "NA"):
//    ParticipantID, Test, Variant, Platform, EyeTracker, GazeSource,
//    Block            Deary: SinglePractice | Single | ChoicePractice | Choice
//                     Corsi: Forward | Backward
//    TrialInBlock     1-based trial counter inside the block
//    Practice         1 = practice trial (not scored), 0 = scored trial
//    Level            Deary: fore-period (s) before the stimulus; Corsi: sequence length
//    Target           Deary: name of the target that lit up; Corsi: block sequence (A>B>C)
//    Response         Deary: name of the target that was touched (NA if none)
//    Outcome          Correct | Wrong | Slow (Deary time-out after presentingTime s)
//    RT_s             Deary: stimulus onset -> touch; Corsi: sequence-end time (trial time)
//    PlanningTime_s   Corsi: end of presentation -> first selection
//    MeanMotorTime_s  Corsi: mean time between successive selections in this trial
//    AttentionTime_s  Deary: stimulus onset -> gaze reaches the target (gaze variants only)
//    SessionTime_s    Time.time at the moment the row was written
//    Timestamp        local wall clock, ISO 8601
//
//  Rows are appended immediately (File.AppendAllText) so data survive a crash or an
//  Alt-F4. Volume is tiny (~70 rows per session) so this has no performance cost.
//
//  Layer     : Runtime, data output (static, no scene object needed).
//  Depends on: CsvUtil, SessionInfo, VariantConfig, GazePointerDriver.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant rebuild of Corsi VR and Deary-Liewald VR.
// =====================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class TrialLogger
{
    private static readonly string[] Header =
    {
        "ParticipantID", "Test", "Variant", "Platform", "EyeTracker", "GazeSource",
        "Block", "TrialInBlock", "Practice", "Level", "Target", "Response", "Outcome",
        "RT_s", "PlanningTime_s", "MeanMotorTime_s", "AttentionTime_s", "SessionTime_s", "Timestamp"
    };

    private static string path;
    private static int participantId;
    // Rows produced before the file exists (should not happen, but never lose data).
    private static readonly List<string> pending = new List<string>();
    // Per-block trial counters (Block name -> count).
    private static readonly Dictionary<string, int> counters = new Dictionary<string, int>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetStatics()
    {
        path = null;
        participantId = 0;
        pending.Clear();
        counters.Clear();
    }

    /// <summary>Creates the per-trial CSV. Called by Score_Manager once the participant ID is known.</summary>
    public static void Init(string filePath, int id)
    {
        path = filePath;
        participantId = id;
        File.WriteAllText(path, string.Join(",", Header) + Environment.NewLine);
        if (pending.Count > 0)
        {
            var sb = new StringBuilder();
            foreach (var row in pending) sb.Append(row.Replace("{PID}", participantId.ToString())).Append(Environment.NewLine);
            File.AppendAllText(path, sb.ToString());
            pending.Clear();
        }
    }

    /// <summary>Returns the next 1-based trial number for a block.</summary>
    public static int NextTrial(string block)
    {
        counters.TryGetValue(block, out int n);
        n++;
        counters[block] = n;
        return n;
    }

    /// <summary>Writes one trial row. Pass float.NaN / null for values that do not apply.</summary>
    public static void Log(string block, int trialInBlock, bool practice, float level, string target, string response,
                           string outcome, float rt, float planning, float meanMotor, float attention)
    {
        try
        {
            var vc = VariantConfig.Instance;
            var gz = GazePointerDriver.Instance;
            string[] cells =
            {
                path != null ? participantId.ToString() : "{PID}",
                vc != null ? vc.TestLabel : CsvUtil.NA,
                vc != null ? vc.VariantLabel : CsvUtil.NA,
                vc != null ? vc.PlatformLabel : CsvUtil.NA,
                vc != null ? vc.EyeTrackerLabel : CsvUtil.NA,
                gz != null ? gz.GazeSource : "None",
                CsvUtil.Format(block),
                trialInBlock.ToString(),
                practice ? "1" : "0",
                CsvUtil.F(level),
                CsvUtil.Format(target ?? CsvUtil.NA),
                CsvUtil.Format(response ?? CsvUtil.NA),
                CsvUtil.Format(outcome),
                CsvUtil.F(rt),
                CsvUtil.F(planning),
                CsvUtil.F(meanMotor),
                CsvUtil.F(attention),
                CsvUtil.F(Time.time),
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture)
            };
            string row = string.Join(",", cells);
            if (path == null) pending.Add(row);
            else File.AppendAllText(path, row + Environment.NewLine);
        }
        catch (Exception e)
        {
            // Never let logging break the experiment flow.
            Debug.LogWarning("[TrialLogger] Could not write trial row: " + e.Message);
        }
    }
}
