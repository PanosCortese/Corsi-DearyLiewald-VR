// =====================================================================================
//  EyeCalibrationGate.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Varjo variant only. After the participant ID has been confirmed, runs the
//              Varjo gaze calibration and only lets the experiment start once calibration
//              quality is Medium or High for BOTH eyes (max. 3 attempts, then the session
//              continues and the achieved quality is written to the CSV "ETCalibration"
//              column so poor-calibration sessions can be excluded later).
//              Same acceptance rule as the lab's existing VarjoET_Calibration.cs.
//  Experimenter keys (desktop keyboard):
//              S = skip calibration (e.g. already calibrated in Varjo Base),
//              R = request a new calibration attempt.
//  Layer     : Runtime, session start-up step (registers in SessionGate).
//  Depends on: SessionInfo / SessionGate, Varjo XR package.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant rebuild of Corsi VR and Deary-Liewald VR.
// =====================================================================================

using System.Collections;
using UnityEngine;
using Varjo.XR;

public class EyeCalibrationGate : MonoBehaviour
{
    [Tooltip("Varjo calibration type. Fast = 5-dot, the lab default.")]
    public VarjoEyeTracking.GazeCalibrationMode calibrationMode = VarjoEyeTracking.GazeCalibrationMode.Fast;

    [Tooltip("Maximum number of calibration attempts before continuing anyway.")]
    public int maxAttempts = 3;

    [Tooltip("Seconds to wait for Varjo Base to allow eye tracking before giving up.")]
    public float gazeAllowedTimeout = 15f;

    [Tooltip("Seconds to wait for a single calibration to finish before giving up on that attempt.")]
    public float calibrationTimeout = 120f;

    private bool skipRequested;
    private bool retryRequested;
    private string status = "";

    private void Awake()
    {
        // Must be registered before Score_Manager checks SessionGate (Awake runs before any Start).
        SessionGate.Register(this);
    }

    private IEnumerator Start()
    {
        // 1) Wait until the experimenter has entered the participant ID.
        yield return new WaitUntil(() => SessionInfo.ParticipantConfirmed);

        // 2) Varjo Base must have "Allow eye tracking" enabled in its privacy settings.
        status = "Waiting for Varjo eye tracking to become available...";
        float t0 = Time.unscaledTime;
        while (!VarjoEyeTracking.IsGazeAllowed() && Time.unscaledTime - t0 < gazeAllowedTimeout && !skipRequested)
            yield return null;
        if (!VarjoEyeTracking.IsGazeAllowed())
        {
            SessionInfo.CalibrationQuality = skipRequested ? "Skipped" : "GazeNotAllowed";
            Debug.LogWarning("[EyeCalibrationGate] Varjo gaze not allowed (enable it in Varjo Base > System > Privacy). Continuing without calibration.");
            Finish();
            yield break;
        }

        // 3) Calibrate until both eyes are Medium/High or attempts run out.
        string quality = "NotCalibrated";
        for (int attempt = 1; attempt <= maxAttempts && !skipRequested; attempt++)
        {
            status = $"Calibration attempt {attempt}/{maxAttempts} - participant follows the dots.";
            retryRequested = false;
            VarjoEyeTracking.RequestGazeCalibration(calibrationMode);

            // Give the runtime a moment to switch into "calibrating" state, then wait for the end.
            yield return new WaitForSecondsRealtime(1f);
            float c0 = Time.unscaledTime;
            while ((VarjoEyeTracking.IsGazeCalibrating() || !VarjoEyeTracking.IsGazeCalibrated())
                   && Time.unscaledTime - c0 < calibrationTimeout && !skipRequested && !retryRequested)
                yield return null;
            if (skipRequested) break;
            if (retryRequested) { attempt--; continue; }

            VarjoEyeTracking.GazeCalibrationQuality q = VarjoEyeTracking.GetGazeCalibrationQuality();
            quality = $"L-{q.left}/R-{q.right}";
            Debug.Log("[EyeCalibrationGate] Calibration quality " + quality);
            if (q.left >= VarjoEyeTracking.GazeEyeCalibrationQuality.Medium &&
                q.right >= VarjoEyeTracking.GazeEyeCalibrationQuality.Medium)
            {
                break;
            }
        }

        SessionInfo.CalibrationQuality = skipRequested ? "Skipped" : quality;
        Finish();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) skipRequested = true;
        if (Input.GetKeyDown(KeyCode.R)) retryRequested = true;
    }

    private void Finish()
    {
        status = "";
        SessionGate.Release(this);
    }

    // Small desktop-mirror hint for the experimenter (IMGUI is drawn on the PC monitor only).
    private void OnGUI()
    {
        if (string.IsNullOrEmpty(status) || !SessionInfo.ParticipantConfirmed) return;
        GUI.Label(new Rect(20, 20, 900, 60), "[Varjo ET] " + status + "   (S = skip, R = recalibrate)");
    }
}
