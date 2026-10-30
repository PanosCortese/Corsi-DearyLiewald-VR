using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Varjo.XR;
using UnityEngine.PlayerLoop;
using Varjo.XR.Input;
using static Varjo.XR.VarjoEyeTracking;
using TMPro;

public class VarjoET_Calibration : MonoBehaviour
{
    public GazeCalibrationMode gazeCalibrationMode = VarjoEyeTracking.GazeCalibrationMode.Fast;
    public GazeCalibrationQuality gazeCalibrationQuality;
    public string calibrationQualityR;
    public string calibrationQualityL;
    public bool calibrationHigh;
    public TextMeshProUGUI eyeTrackingCalibration;
    private void OnEnable()
    {
        calibrationHigh = false;
        // Start gaze calibration
        eyeTrackingCalibration.text = "ET Calibration Started";
        StartCoroutine(ET_Calibration());
    }

    private IEnumerator ET_Calibration()
    {
        while ((calibrationQualityR != "High" && calibrationQualityR != "Medium") || (calibrationQualityL != "High" && calibrationQualityL != "Medium"))
        {
            // Start gaze calibration
            VarjoEyeTracking.RequestGazeCalibration(gazeCalibrationMode);
            // Wait for calibration to finish
            yield return new WaitUntil(() => VarjoEyeTracking.IsGazeCalibrated());
            // Get the calibration quality
            gazeCalibrationQuality = VarjoEyeTracking.GetGazeCalibrationQuality();
            calibrationQualityR = gazeCalibrationQuality.right.ToString();
            calibrationQualityL = gazeCalibrationQuality.left.ToString();
            Debug.Log("Calibration quality Right/Left: " + calibrationQualityR + "/" + calibrationQualityL);
            eyeTrackingCalibration.text = "ET Calibration R/L: " + calibrationQualityR + "/" + calibrationQualityL;
        }
        eyeTrackingCalibration.text = "ET: High Accuracy";
        calibrationHigh = true;
    }
}
