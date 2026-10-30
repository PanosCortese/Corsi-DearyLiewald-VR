// =====================================================================================
//  VibrateInteract.cs   (CognitiveTestsVR version: SteamVR-free)
// -------------------------------------------------------------------------------------
//  Purpose   : Short controller vibration when the participant's index-finger collider
//              touches a Corsi block / Digit button.
//  Layer     : Runtime, interaction feedback only (does NOT affect scoring or timing).
//  Depends on: UnityEngine.XR (works with Varjo, OpenXR PC and OpenXR Android loaders).
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2024-xx-xx  original  SteamVR_Action_Vibration based (Valve.VR namespace).
//    2026-10-29  v2.0      Re-implemented with UnityEngine.XR.InputDevice haptics because the
//                          lean CognitiveTestsVR project does not ship the SteamVR plugin
//                          (it requires OpenVR and would not work on Android / OpenXR anyway).
//                          The class name, file GUID and serialized fields duration /
//                          frequency / amplitude are kept so that existing scene references
//                          stay valid. The old 'haptic' (SteamVR action) field is dropped;
//                          Unity silently ignores the orphaned serialized value.
// =====================================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VibrateInteract : MonoBehaviour
{
    [Tooltip("Vibration length in seconds.")]
    public float duration;
    [Tooltip("Kept for compatibility with the SteamVR version (XR impulse haptics have no frequency).")]
    public float frequency;
    [Tooltip("Vibration strength 0..1.")]
    public float amplitude;

    // Re-used list to avoid allocating every physics frame (OnTriggerStay runs at 50 Hz+).
    private readonly List<InputDevice> devices = new List<InputDevice>();

    // OnTriggerStay used to fire a SteamVR pulse every physics step. XR impulses restart on
    // every call, so we throttle to avoid flooding the runtime with haptic commands.
    private float nextAllowedPulse;

    private void Pulse(XRNode node)
    {
        if (Time.unscaledTime < nextAllowedPulse) return;
        nextAllowedPulse = Time.unscaledTime + Mathf.Max(0.02f, duration);

        devices.Clear();
        InputDevices.GetDevicesAtXRNode(node, devices);
        foreach (var d in devices)
        {
            if (d.isValid && d.TryGetHapticCapabilities(out var caps) && caps.supportsImpulse)
            {
                d.SendHapticImpulse(0u, Mathf.Clamp01(amplitude), Mathf.Max(0.01f, duration));
            }
        }
    }

    private void Handle(Collider other)
    {
        if (!other.transform.gameObject.CompareTag("IndexFinger")) return;
        // Same name-based hand detection as the original script.
        if (other.transform.gameObject.name == "IndexFingerColliderR") Pulse(XRNode.RightHand);
        else if (other.transform.gameObject.name == "IndexFingerColliderL") Pulse(XRNode.LeftHand);
    }

    private void OnTriggerEnter(Collider other) { Handle(other); }
    private void OnTriggerStay(Collider other) { Handle(other); }
}
