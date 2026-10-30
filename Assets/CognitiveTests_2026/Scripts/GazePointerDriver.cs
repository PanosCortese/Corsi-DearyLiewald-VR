// =====================================================================================
//  GazePointerDriver.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Moves the scene's "ET_Pointer" (small kinematic trigger sphere, tag
//              "ET_Pointer") to where the participant is looking, for the variants that
//              do NOT use the original SRanipal ET_Pointer.cs script. All existing gaze
//              metrics (Deary attention time, Corsi fixation count/duration) are computed
//              by OnTriggerEnter/Exit between this pointer and the targets, so the
//              measurement logic is IDENTICAL across headsets; only the gaze source changes.
//
//              Gaze sources (chosen from VariantConfig):
//                * Varjo          -> Varjo native gaze API (VarjoEyeTracking.GetGaze()).
//                                    Also feeds pupil diameters into EyeTracking's lists
//                                    so Deary_Manager / Corsi_ET_Manager compute pupil size
//                                    exactly as they do for the Pro Eye.
//                * ProEye         -> nothing to move here: the original ET_Pointer.cs
//                                    (SRanipal Focus) drives the pointer and EyeTracking.cs
//                                    collects pupil data. This driver only reports whether
//                                    SRanipal is actually WORKING (for the GazeSource column).
//                * FocusVision    -> VIVE OpenXR Plugin (XR_HTC_eye_tracker) binocular gaze
//                                    pose + pupillometry (GetEyePupilData in mm), with automatic
//                                    fallback to XR_EXT_eye_gaze_interaction if needed.
//                * QuestPro       -> Meta Quest Pro OpenXR eye gaze (XR_EXT_eye_gaze_interaction)
//                                    via both UnityEngine.XR.InputDevices and InputSystem
//                                    <EyeGaze>/pose, plus Android runtime permission request
//                                    for com.oculus.permission.EYE_TRACKING.
//                * OpenXR_PC /    -> OPTIONAL OpenXR eye gaze (XR_EXT_eye_gaze_interaction),
//                  OpenXR_Android    used only if the headset/runtime exposes an eye-tracking
//                                    device. Otherwise there is NO gaze: the pointer is parked
//                                    far away and every gaze metric is written as "NA".
//                                    IMPORTANT: unlike the old OpenXRGazePointer, there is
//                                    deliberately NO head-direction fallback, because head
//                                    pointing would silently produce fake "attention times".
//
//  Layer     : Runtime, eye-tracking input adapter (one instance on ExperimentManager).
//  Depends on: VariantConfig, EyeTracking (pupil lists), Varjo XR, VIVE.OpenXR, ViveSR.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant rebuild of Corsi VR and Deary-Liewald VR.
//    2026-10-29  v1.1  Added FocusVision (XR_HTC_eye_tracker gaze + pupil) and QuestPro support.
// =====================================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using Varjo.XR;
using VIVE.OpenXR;
using VIVE.OpenXR.EyeTracker;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class GazePointerDriver : MonoBehaviour
{
    [Header("Scene references (wired by the scene builder)")]
    [Tooltip("The ET_Pointer object (kinematic Rigidbody + trigger SphereCollider, tag 'ET_Pointer').")]
    public Transform gazePointer;

    [Tooltip("The XR (HMD) camera. Varjo gaze rays are relative to it; OpenXR poses are relative to its parent (Camera Offset).")]
    public Camera xrCamera;

    [Tooltip("EyeTracking component whose Pupil_Size_R/L lists Deary_Manager and Corsi_ET_Manager average. " +
             "For Varjo and FocusVision the pupil samples are pushed into these lists.")]
    public EyeTracking pupilSink;

    [Header("Ray settings")]
    [Tooltip("Maximum gaze ray length in metres.")]
    public float maxRayLength = 20f;

    [Tooltip("Where the pointer is parked (metres along the gaze ray) when the gaze does not hit a target tagged 'ET'. " +
             "Same behaviour as the original VarjoGazePointer (5 m), far behind every task object.")]
    public float floatingDistance = 5f;

    [Tooltip("Where the pointer is parked when there is no gaze at all (far below the floor, so it can never touch a target).")]
    public Vector3 noGazeParkingPosition = new Vector3(0f, -100f, 0f);

    [Header("Read-only diagnostics")]
    [SerializeField] private string gazeSource = "None";
    [SerializeField] private int validSamples;
    [SerializeField] private bool gazeValidThisFrame;

    /// <summary>Scene-wide access for Score_Manager / TrialLogger.</summary>
    public static GazePointerDriver Instance { get; private set; }

    /// <summary>Human-readable gaze source for the CSV "GazeSource" column.</summary>
    public string GazeSource => gazeSource;

    /// <summary>True if at least ~1 s of valid gaze was received during the session.
    /// When false, every gaze-derived metric is written as "NA" by Score_Manager.</summary>
    public bool GazeEverValid => validSamples > 60;

    private HardwareVariant variant;
    private Rigidbody gazePointerRb;

    // OpenXR eye-gaze device via UnityEngine.XR.InputDevices (XR_EXT_eye_gaze_interaction).
    private InputDevice openXrEyeDevice;
    private float nextDeviceSearch;
    private static readonly InputFeatureUsage<Vector3> GazePosition = new InputFeatureUsage<Vector3>("gazePosition");
    private static readonly InputFeatureUsage<Quaternion> GazeRotation = new InputFeatureUsage<Quaternion>("gazeRotation");
    private static readonly InputFeatureUsage<bool> GazeTracked = new InputFeatureUsage<bool>("isTracked");
    private readonly List<InputDevice> deviceBuffer = new List<InputDevice>();

    // OpenXR eye-gaze action via Unity InputSystem (<EyeGaze>/pose), used on Quest Pro / Android OpenXR
    // when EyeGazeInteraction routes the pose through InputSystem's EyeGazeDevice layout.
    private InputAction eyePoseAction;

    // Gaze ray of the current frame (world space). Raycast happens in FixedUpdate because the
    // pointer is a physics trigger and its OnTriggerEnter/Exit are evaluated by the physics step.
    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    private void Awake()
    {
        Instance = this;
        if (gazePointer != null) gazePointerRb = gazePointer.GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        if (eyePoseAction != null)
        {
            eyePoseAction.Disable();
            eyePoseAction.Dispose();
            eyePoseAction = null;
        }
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        variant = VariantConfig.Instance != null ? VariantConfig.Instance.variant : HardwareVariant.OpenXR_PC;
        if (xrCamera == null) xrCamera = Camera.main;
        if (gazePointer != null && gazePointerRb == null) gazePointerRb = gazePointer.GetComponent<Rigidbody>();

        // Create the InputSystem <EyeGaze>/pose binding for all OpenXR variants (QuestPro, FocusVision, OpenXR_*).
        if (variant != HardwareVariant.Varjo && variant != HardwareVariant.ProEye)
        {
            eyePoseAction = new InputAction("EyeGazePose", InputActionType.PassThrough, "<EyeGaze>/pose");
            eyePoseAction.Enable();
        }

        switch (variant)
        {
            case HardwareVariant.Varjo:
                gazeSource = "Varjo";
                // 200 Hz is the native Varjo rate; we sample once per frame for the pointer,
                // pupil samples are also taken once per frame (same as the SRanipal version).
                VarjoEyeTracking.SetGazeOutputFrequency(VarjoEyeTracking.GazeOutputFrequency.MaximumSupported);
                break;

            case HardwareVariant.ProEye:
                gazeSource = "SRanipal";
                break;

            case HardwareVariant.FocusVision:
                gazeSource = "VIVE-XR_HTC_eye_tracker";
                ParkNoGaze();
                break;

            case HardwareVariant.QuestPro:
                gazeSource = "OpenXR-QuestPro";
                RequestQuestEyePermissionIfNeeded();
                ParkNoGaze();
                break;

            default:
                gazeSource = "None";
                ParkNoGaze();
                break;
        }
    }

    private static void RequestQuestEyePermissionIfNeeded()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        const string QuestEyePermission = "com.oculus.permission.EYE_TRACKING";
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(QuestEyePermission))
        {
            UnityEngine.Android.Permission.RequestUserPermission(QuestEyePermission);
        }
#endif
    }

    private void Update()
    {
        gazeValidThisFrame = false;
        switch (variant)
        {
            case HardwareVariant.Varjo:
                UpdateVarjo();
                break;
            case HardwareVariant.ProEye:
                UpdateSRanipalStatus();
                break;
            case HardwareVariant.FocusVision:
                UpdateFocusVision();
                break;
            default:
                UpdateOpenXrEyeGaze();
                break;
        }
        if (gazeValidThisFrame) validSamples++;
    }

    // ------------------------------------------------------------------ Varjo

    private void UpdateVarjo()
    {
        if (!VarjoEyeTracking.IsGazeAllowed() || !VarjoEyeTracking.IsGazeCalibrated()) return;

        VarjoEyeTracking.GazeData gaze = VarjoEyeTracking.GetGaze();
        // "Adjust" = still calibrating, "Invalid" = headset off / eyes not found -> ignore both.
        if (gaze.status != VarjoEyeTracking.GazeStatus.Valid) return;

        // Gaze rays are relative to the HMD pose -> transform to world space via the XR camera.
        rayOrigin = xrCamera.transform.TransformPoint(gaze.gaze.origin);
        rayDirection = xrCamera.transform.TransformDirection(gaze.gaze.forward).normalized;
        gazeValidThisFrame = true;

        // Pupillometry: only while a task is running (EyeTracking.corsi / deary are set by the
        // task managers exactly like for the Pro Eye). Only eyes reported as Tracked or
        // Compensated are used (Invalid = shut, Visible = blink/saccade), and values <= 0 mm are
        // skipped so that blinks do not bias the average downwards.
        if (pupilSink != null && (pupilSink.corsi || pupilSink.deary))
        {
            VarjoEyeTracking.EyeMeasurements m = VarjoEyeTracking.GetEyeMeasurements();
            if (gaze.rightStatus >= VarjoEyeTracking.GazeEyeStatus.Compensated && m.rightPupilDiameterInMM > 0f)
                pupilSink.Pupil_Size_R.Add(m.rightPupilDiameterInMM);
            if (gaze.leftStatus >= VarjoEyeTracking.GazeEyeStatus.Compensated && m.leftPupilDiameterInMM > 0f)
                pupilSink.Pupil_Size_L.Add(m.leftPupilDiameterInMM);
        }
    }

    // ------------------------------------------------------------------ Pro Eye (SRanipal)

    private void UpdateSRanipalStatus()
    {
        // The pointer itself is moved by ET_Pointer.cs; we only count frames in which the
        // SRanipal framework reports WORKING, to decide whether gaze metrics are valid.
        if (ViveSR.anipal.Eye.SRanipal_Eye_Framework.Status == ViveSR.anipal.Eye.SRanipal_Eye_Framework.FrameworkStatus.WORKING)
        {
            gazeValidThisFrame = true;
        }
    }

    // ------------------------------------------------------------------ HTC VIVE Focus Vision (XR_HTC_eye_tracker)

    private void UpdateFocusVision()
    {
        // 1) Binocular gaze via XR_HTC_eye_tracker (poses are in OpenXR app/tracking space).
        if (XR_HTC_eye_tracker.Interop.GetEyeGazeData(out XrSingleEyeGazeDataHTC[] gazes) &&
            gazes != null && gazes.Length >= (int)XrEyePositionHTC.XR_EYE_POSITION_COUNT_HTC)
        {
            var left = gazes[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
            var right = gazes[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];
            bool leftValid = left.isValid;
            bool rightValid = right.isValid;

            if (leftValid || rightValid)
            {
                Vector3 localOrigin;
                Vector3 localDir;
                if (leftValid && rightValid)
                {
                    Vector3 lPos = left.gazePose.position.ToUnityVector();
                    Vector3 rPos = right.gazePose.position.ToUnityVector();
                    Vector3 lDir = left.gazePose.orientation.ToUnityQuaternion() * Vector3.forward;
                    Vector3 rDir = right.gazePose.orientation.ToUnityQuaternion() * Vector3.forward;
                    localOrigin = 0.5f * (lPos + rPos);
                    localDir = (lDir + rDir).normalized;
                }
                else if (leftValid)
                {
                    localOrigin = left.gazePose.position.ToUnityVector();
                    localDir = (left.gazePose.orientation.ToUnityQuaternion() * Vector3.forward).normalized;
                }
                else
                {
                    localOrigin = right.gazePose.position.ToUnityVector();
                    localDir = (right.gazePose.orientation.ToUnityQuaternion() * Vector3.forward).normalized;
                }

                Transform trackingSpace = xrCamera != null ? xrCamera.transform.parent : null;
                rayOrigin = trackingSpace != null ? trackingSpace.TransformPoint(localOrigin) : localOrigin;
                rayDirection = (trackingSpace != null ? trackingSpace.TransformDirection(localDir) : localDir).normalized;
                gazeSource = "VIVE-XR_HTC_eye_tracker";
                gazeValidThisFrame = true;
            }
        }

        // 2) Pupillometry via XR_HTC_eye_tracker (pupilDiameter is reported in millimetres).
        if (pupilSink != null && (pupilSink.corsi || pupilSink.deary))
        {
            if (XR_HTC_eye_tracker.Interop.GetEyePupilData(out XrSingleEyePupilDataHTC[] pupils) &&
                pupils != null && pupils.Length >= (int)XrEyePositionHTC.XR_EYE_POSITION_COUNT_HTC)
            {
                var leftPupil = pupils[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
                var rightPupil = pupils[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];
                if (rightPupil.isDiameterValid && rightPupil.pupilDiameter > 0f)
                    pupilSink.Pupil_Size_R.Add(rightPupil.pupilDiameter);
                if (leftPupil.isDiameterValid && leftPupil.pupilDiameter > 0f)
                    pupilSink.Pupil_Size_L.Add(leftPupil.pupilDiameter);
            }
        }

        // 3) Fallback to standard XR_EXT_eye_gaze_interaction if XR_HTC_eye_tracker was not valid this frame.
        if (!gazeValidThisFrame)
        {
            UpdateOpenXrEyeGaze();
        }
    }

    // ------------------------------------------------------------------ OpenXR Eye Gaze (Quest Pro & optional OpenXR)

    private void UpdateOpenXrEyeGaze()
    {
        Transform trackingSpace = xrCamera != null ? xrCamera.transform.parent : null;

        // Path A: UnityEngine.XR.InputDevices (EyeGazeInteraction XR InputDevice)
        if (!openXrEyeDevice.isValid && Time.unscaledTime >= nextDeviceSearch)
        {
            nextDeviceSearch = Time.unscaledTime + 1f;
            deviceBuffer.Clear();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, deviceBuffer);
            if (deviceBuffer.Count > 0)
            {
                openXrEyeDevice = deviceBuffer[0];
            }
        }

        if (openXrEyeDevice.isValid)
        {
            bool tracked = !openXrEyeDevice.TryGetFeatureValue(GazeTracked, out bool t) || t;
            if (tracked &&
                openXrEyeDevice.TryGetFeatureValue(GazePosition, out Vector3 pos) &&
                openXrEyeDevice.TryGetFeatureValue(GazeRotation, out Quaternion rot))
            {
                rayOrigin = trackingSpace != null ? trackingSpace.TransformPoint(pos) : pos;
                rayDirection = ((trackingSpace != null ? trackingSpace.rotation * rot : rot) * Vector3.forward).normalized;
                gazeSource = variant == HardwareVariant.QuestPro ? "OpenXR-QuestPro" : "OpenXR-EyeGaze";
                gazeValidThisFrame = true;
                return;
            }
        }

        // Path B: UnityEngine.InputSystem (<EyeGaze>/pose PoseState, used by EyeGazeInteraction's EyeGazeDevice)
        if (eyePoseAction != null && eyePoseAction.enabled)
        {
            PoseState pose = eyePoseAction.ReadValue<PoseState>();
            if (pose.isTracked)
            {
                rayOrigin = trackingSpace != null ? trackingSpace.TransformPoint(pose.position) : pose.position;
                rayDirection = ((trackingSpace != null ? trackingSpace.rotation * pose.rotation : pose.rotation) * Vector3.forward).normalized;
                gazeSource = variant == HardwareVariant.QuestPro ? "OpenXR-QuestPro" : "OpenXR-EyeGaze";
                gazeValidThisFrame = true;
            }
        }
    }

    // ------------------------------------------------------------------ pointer placement

    private void FixedUpdate()
    {
        if (gazePointer == null || variant == HardwareVariant.ProEye) return;

        // No valid gaze ever / currently no source: keep the pointer away from every target.
        if (rayDirection == Vector3.zero)
        {
            ParkNoGaze();
            return;
        }

        Vector3 targetPos;
        // Default raycast layers exclude "Ignore Raycast" (the pointer's own layer).
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, maxRayLength) && hit.collider.CompareTag("ET"))
        {
            targetPos = hit.point;
        }
        else
        {
            targetPos = rayOrigin + rayDirection * floatingDistance;
        }

        MovePointer(targetPos);
    }

    private void ParkNoGaze()
    {
        if (gazePointer != null) MovePointer(noGazeParkingPosition);
    }

    private void MovePointer(Vector3 worldPos)
    {
        if (gazePointerRb != null && gazePointerRb.isKinematic)
            gazePointerRb.MovePosition(worldPos);
        gazePointer.position = worldPos;
    }
}
