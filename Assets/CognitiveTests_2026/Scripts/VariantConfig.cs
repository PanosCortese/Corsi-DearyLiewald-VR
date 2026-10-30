// =====================================================================================
//  VariantConfig.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Declares, per scene, WHICH cognitive test and WHICH hardware variant this
//              scene/build is. All other scripts ask this component instead of using
//              scattered booleans, so all six variants of a test can share one code base:
//                * Varjo          - Varjo XR loader, Varjo eye tracking + pupillometry
//                * ProEye         - OpenXR (SteamVR runtime) + HTC SRanipal eye tracking
//                * OpenXR_PC      - OpenXR PC VR, no eye tracking (RT / accuracy only)
//                * OpenXR_Android - OpenXR Android standalone universal, no required ET
//                * FocusVision    - OpenXR Android + VIVE OpenXR Plugin (XR_HTC_eye_tracker)
//                                   for HTC VIVE Focus Vision eye tracking + pupillometry
//                * QuestPro       - OpenXR Android + Meta Quest Support + EyeGazeInteraction
//                                   for Meta Quest Pro eye tracking (gaze attention/fixation)
//              It also writes the descriptive columns (Test, Variant, Platform, EyeTracker)
//              that make pooled CSV files from different headsets distinguishable.
//  Layer     : Runtime, scene configuration (one instance on the ExperimentManager object).
//  Depends on: nothing (other scripts depend on it).
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant rebuild of Corsi VR and Deary-Liewald VR.
//    2026-10-29  v1.1  Added FocusVision (VIVE OpenXR ET + pupil) and QuestPro (Meta ET) variants.
// =====================================================================================

using UnityEngine;

public enum CognitiveTest
{
    Corsi,
    DearyLiewald
}

public enum HardwareVariant
{
    Varjo,
    ProEye,
    OpenXR_PC,
    OpenXR_Android,
    FocusVision,
    QuestPro
}

[DisallowMultipleComponent]
public class VariantConfig : MonoBehaviour
{
    [Tooltip("Which cognitive test this scene runs.")]
    public CognitiveTest test = CognitiveTest.Corsi;

    [Tooltip("Which hardware/runtime variant this scene is built for.")]
    public HardwareVariant variant = HardwareVariant.OpenXR_PC;

    [Tooltip("Version string written into every CSV row (helps when results from different builds are pooled).")]
    public string appVersion = "2026.10";

    /// <summary>Scene-wide access point. There is exactly one VariantConfig per scene.</summary>
    public static VariantConfig Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // ------------------------------------------------------------------ derived properties

    /// <summary>True when the variant has an eye tracker (Varjo, ProEye, FocusVision, QuestPro).</summary>
    public bool HasEyeTracker =>
        variant == HardwareVariant.Varjo ||
        variant == HardwareVariant.ProEye ||
        variant == HardwareVariant.FocusVision ||
        variant == HardwareVariant.QuestPro;

    /// <summary>True when the variant provides pupil diameter in mm (Varjo, ProEye, FocusVision).</summary>
    public bool HasPupillometry =>
        variant == HardwareVariant.Varjo ||
        variant == HardwareVariant.ProEye ||
        variant == HardwareVariant.FocusVision;

    /// <summary>Short test name used in file names and the "Test" CSV column.</summary>
    public string TestLabel => test == CognitiveTest.Corsi ? "CorsiVR" : "DearyLiewaldVR";

    /// <summary>Label for the "Variant" CSV column and file names.</summary>
    public string VariantLabel => variant.ToString();

    /// <summary>Label for the "Platform" CSV column.</summary>
    public string PlatformLabel
    {
        get
        {
            switch (variant)
            {
                case HardwareVariant.Varjo: return "PCVR-Varjo";
                case HardwareVariant.ProEye: return "PCVR-OpenXR-SteamVR";
                case HardwareVariant.OpenXR_PC: return "PCVR-OpenXR";
                case HardwareVariant.FocusVision: return "Android-OpenXR-FocusVision";
                case HardwareVariant.QuestPro: return "Android-OpenXR-QuestPro";
                default: return "Android-OpenXR";
            }
        }
    }

    /// <summary>Label for the "EyeTracker" CSV column.</summary>
    public string EyeTrackerLabel
    {
        get
        {
            switch (variant)
            {
                case HardwareVariant.Varjo: return "Varjo";
                case HardwareVariant.ProEye: return "SRanipal-ProEye";
                case HardwareVariant.FocusVision: return "VIVE-FocusVision";
                case HardwareVariant.QuestPro: return "OpenXR-QuestPro";
                default: return "None";
            }
        }
    }
}
