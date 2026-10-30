using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class ControllerVibrationOnCollision : MonoBehaviour
{
    public string targetTag = "YourTargetTag"; // Tag of the GameObject to react to
    public float vibrationDuration = 0.5f; // Duration of the vibration in seconds
    public float vibrationStrength = 0.5f; // Strength of the vibration, range 0 to 1
    public float vibrationFrequency = 50f; // Frequency of the vibration in Hz, typical range 0 to 320

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Vibrate both controllers
            StartCoroutine(VibrateController(XRNode.LeftHand, vibrationDuration, vibrationStrength, vibrationFrequency));
            StartCoroutine(VibrateController(XRNode.RightHand, vibrationDuration, vibrationStrength, vibrationFrequency));
        }
    }

    private IEnumerator VibrateController(XRNode hand, float duration, float strength, float frequency)
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(hand, devices);
        var device = devices.FirstOrDefault();

        if (device != null && device.isValid)
        {
            // Check for haptic capabilities
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities) && capabilities.supportsBuffer)
            {
                // Create a haptic buffer
                int sampleRateHz = 320; // Common sample rate for haptic buffers
                int sampleCount = Mathf.CeilToInt(duration * sampleRateHz);
                byte[] hapticBuffer = new byte[sampleCount];

                for (int i = 0; i < sampleCount; i++)
                {
                    // Create a simple sine wave pattern for the vibration
                    float time = i / (float)sampleRateHz;
                    float amplitude = Mathf.Sin(2 * Mathf.PI * frequency * time) * strength;
                    hapticBuffer[i] = (byte)(Mathf.Clamp01(amplitude) * 255);
                }

                device.SendHapticBuffer(0, hapticBuffer);
            }
            else if (device.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
            {
                // Fallback to simple impulse if buffer not supported
                device.SendHapticImpulse(0, strength, duration);
            }
        }

        yield return new WaitForSeconds(duration);

        // Optionally stop vibration - depending on your controller you may need to manually stop it
        if (device != null && device.isValid)
        {
            device.StopHaptics();
        }
    }
}
