using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum GazeDataSource3
{
    InputSubsystem,
    Head
}

public class OpenXRGazePointer : MonoBehaviour
{
    [Header("Gaze data")]
    public GazeDataSource3 gazeDataSource = GazeDataSource3.InputSubsystem;

    [Header("Gaze point indicator")]
    public GameObject gazeTarget;

    [Header("Visualization Transforms")]
    public Transform fixationPointTransform;
    public Transform leftEyeTransform;
    public Transform rightEyeTransform;

    [Header("XR camera")]
    public Camera xrCamera;

    [Header("Gaze ray radius")]
    public float gazeRadius = 0.01f;
    public float rayLength = 10f;

    [Header("Gaze point distance if not hit anything")]
    public float floatingGazeTargetDistance = 5f;

    [Header("Raycast or SphereCast")]
    public bool CastRay = true;
    public bool CastSphere = false;

    private InputDevice device;
    private Eyes eyes;
    private Vector3 leftEyePosition;
    private Vector3 rightEyePosition;
    private Quaternion leftEyeRotation;
    private Quaternion rightEyeRotation;
    private Vector3 fixationPoint;
    private Vector3 direction;
    private Vector3 rayOrigin;
    private RaycastHit hit;

    void Start()
    {
        InitializeDevice();
    }

    void InitializeDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, devices);
        if (devices.Count > 0)
        {
            device = devices[0];
            gazeDataSource = GazeDataSource3.InputSubsystem;
            Debug.Log("Eye tracking device found.");
        }
        else
        {
            gazeDataSource = GazeDataSource3.Head;
            Debug.LogError("No eye tracking device found. Fallback to head-based raycasting.");
        }
    }

    void Update()
    {
        if (gazeDataSource == GazeDataSource3.InputSubsystem && device.isValid)
        {
            UpdateInputSubsystemEyeData();
        }
        else if (gazeDataSource == GazeDataSource3.Head)
        {
            UpdateHeadBasedData();
        }
    }

    void UpdateInputSubsystemEyeData()
    {
        if (device.TryGetFeatureValue(CommonUsages.eyesData, out eyes))
        {
            if (eyes.TryGetLeftEyePosition(out leftEyePosition))
            {
                leftEyeTransform.localPosition = leftEyePosition;
            }

            if (eyes.TryGetLeftEyeRotation(out leftEyeRotation))
            {
                leftEyeTransform.localRotation = leftEyeRotation;
            }

            if (eyes.TryGetRightEyePosition(out rightEyePosition))
            {
                rightEyeTransform.localPosition = rightEyePosition;
            }

            if (eyes.TryGetRightEyeRotation(out rightEyeRotation))
            {
                rightEyeTransform.localRotation = rightEyeRotation;
            }

            if (eyes.TryGetFixationPoint(out fixationPoint))
            {
                fixationPointTransform.localPosition = fixationPoint;
            }

            // Set raycast origin point to VR camera position
            rayOrigin = xrCamera.transform.position;

            // Direction from VR camera towards fixation point
            direction = (fixationPointTransform.position - xrCamera.transform.position).normalized;
        }
        else
        {
            Debug.LogError("Failed to get eyes data from device.");
        }
    }

    void UpdateHeadBasedData()
    {
        // Set raycast origin point to VR camera position
        rayOrigin = xrCamera.transform.position;

        // Direction from VR camera's forward direction
        direction = xrCamera.transform.forward;

        // Set the fixation point to be in front of the camera
        fixationPoint = rayOrigin + direction * floatingGazeTargetDistance;
        fixationPointTransform.localPosition = fixationPoint;
    }

    void FixedUpdate()
    {
        if (CastRay)
        {
            PerformRaycast();
        }
        else if (CastSphere)
        {
            PerformSphereCast();
        }
    }

    void PerformRaycast()
    {
        if (Physics.Raycast(rayOrigin, direction, out hit, rayLength))
        {
            if (hit.collider.gameObject.CompareTag("ET"))
            {
                gazeTarget.transform.position = hit.point;
            }
            else
            {
                gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
            }
        }
        else
        {
            gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
        }
    }

    void PerformSphereCast()
    {
        if (Physics.SphereCast(rayOrigin, gazeRadius, direction, out hit, rayLength))
        {
            if (hit.collider.gameObject.CompareTag("ET"))
            {
                gazeTarget.transform.position = hit.point;
            }
            else
            {
                gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
            }
        }
        else
        {
            gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 gizmoDirection = transform.TransformDirection(Vector3.forward) * rayLength;
        // Draw the ray as a line
        Gizmos.DrawRay(transform.position, gizmoDirection);
        // Draw the sphere at the beginning of the cast
        Gizmos.DrawWireSphere(transform.position + gizmoDirection.normalized * gazeRadius, gazeRadius);
        // Draw the sphere at the maximum extent of the cast
        Gizmos.DrawWireSphere(transform.position + gizmoDirection, gazeRadius);
    }
}
