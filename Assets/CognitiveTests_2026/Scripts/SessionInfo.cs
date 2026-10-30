// =====================================================================================
//  SessionInfo.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Static, scene-independent session state shared by the start-up steps
//              (participant-ID entry, eye-tracker calibration) and the data writers
//              (Score_Manager summary CSV, TrialLogger per-trial CSV).
//              Also contains SessionGate: a tiny "wait until everybody is ready" counter
//              so the experiment only starts after ALL start-up steps have finished.
//  Layer     : Runtime, static state (no MonoBehaviour).
//  Depends on: nothing.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant rebuild of Corsi VR and Deary-Liewald VR.
// =====================================================================================
//
//  START-UP ORDER (all variants)
//  -----------------------------
//   1. ParticipantIdKeypad  registers a gate in Awake(), releases it when the experimenter
//                           confirms the ID (in-VR keypad or desktop keyboard).
//   2. EyeCalibrationGate   (Varjo only) registers a gate in Awake(), waits for step 1,
//                           runs the Varjo calibration, releases the gate.
//   3. Score_Manager        waits until SessionGate.AllReleased, then creates the CSV files
//                           and sets CSVInitialization = true.
//   4. Task_Selection_Order (unchanged logic) waits for CSVInitialization and starts the task.
// =====================================================================================

using System;
using System.Collections.Generic;

public static class SessionInfo
{
    /// <summary>Participant ID typed by the experimenter. 0 = not entered (falls back to the
    /// original PlayerPrefs auto-increment numbering inside Score_Manager).</summary>
    public static int ParticipantId;

    /// <summary>True once the experimenter has confirmed the ID screen (even if left empty).</summary>
    public static bool ParticipantConfirmed;

    /// <summary>Result of the eye-tracker calibration, written to the CSV ("ETCalibration").
    /// "NA" for variants without eye tracking.</summary>
    public static string CalibrationQuality = CsvUtil.NA;

    /// <summary>Local wall-clock time at which the scene started (for file names / CSV).</summary>
    public static DateTime SessionStart = DateTime.Now;

    /// <summary>yyyyMMdd_HHmmss stamp used in file names so that files are never overwritten.</summary>
    public static string SessionStamp => SessionStart.ToString("yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>Resets the static state. Called automatically BEFORE the first scene loads
    /// (i.e. before any Awake() can register a gate) because static fields survive
    /// "Enter Play Mode" without domain reload in the Editor.</summary>
    [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Reset()
    {
        ParticipantId = 0;
        ParticipantConfirmed = false;
        CalibrationQuality = CsvUtil.NA;
        SessionStart = DateTime.Now;
        SessionGate.Clear();
    }
}

/// <summary>
/// Counts start-up steps that must finish before the experiment begins.
/// Register(owner) in Awake(), Release(owner) when done. AllReleased is checked by Score_Manager.
/// </summary>
public static class SessionGate
{
    private static readonly HashSet<object> pending = new HashSet<object>();

    public static void Register(object owner) { if (owner != null) pending.Add(owner); }
    public static void Release(object owner) { if (owner != null) pending.Remove(owner); }
    public static bool AllReleased => pending.Count == 0;
    public static void Clear() { pending.Clear(); }
}
