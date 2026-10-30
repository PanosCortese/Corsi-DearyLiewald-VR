using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Varjo.XR;
using TMPro;

public class EyeTrackingVarjo : MonoBehaviour
{
    // Member variables to store eye tracking data
    public float leftPupilDiameterInMM;
    public float rightPupilDiameterInMM;
    public float leftIrisDiameterInMM;
    public float rightIrisDiameterInMM;
    public float leftEyeOpenness;
    public float rightEyeOpenness;
    public TextMeshProUGUI eyeTrackingCalibration;

    // Other member variables and initialization code...

    void Update()
    {
        // Fetching eye tracking data
        if (VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            // Fetching the latest gaze data
            VarjoEyeTracking.GazeData gazeData = VarjoEyeTracking.GetGaze();
            if (gazeData.status != VarjoEyeTracking.GazeStatus.Invalid)
            {
                // Fetching the latest eye measurements data
                VarjoEyeTracking.EyeMeasurements eyeMeasurements = VarjoEyeTracking.GetEyeMeasurements();

                // Updating the member variables with the latest eye tracking data
                leftPupilDiameterInMM = eyeMeasurements.leftPupilDiameterInMM;
                rightPupilDiameterInMM = eyeMeasurements.rightPupilDiameterInMM;
                leftIrisDiameterInMM = eyeMeasurements.leftIrisDiameterInMM;
                rightIrisDiameterInMM = eyeMeasurements.rightIrisDiameterInMM;
                leftEyeOpenness = eyeMeasurements.leftEyeOpenness;
                rightEyeOpenness = eyeMeasurements.rightEyeOpenness;

                // Optionally, log the fetched data for debugging
                //Debug.Log($"Left Pupil Diameter: {leftPupilDiameterInMM}mm, Right Pupil Diameter: {rightPupilDiameterInMM}mm");
                //Debug.Log($"Left Iris Diameter: {leftIrisDiameterInMM}mm, Right Iris Diameter: {rightIrisDiameterInMM}mm");
                //Debug.Log($"Left Eye Openness: {leftEyeOpenness}, Right Eye Openness: {rightEyeOpenness}");
            }
            else
            {
                //Debug.LogWarning("Gaze data is invalid.");
            }
        }
        else
        {
            //Debug.LogWarning("Gaze is not allowed or not calibrated.");
        }

        // Other update code...
    }

    // Other methods and logic...
}

