// =====================================================================================
//  ParticipantIdKeypad.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Experimenter-entered Participant ID at the start of every session,
//              replacing the old silent PlayerPrefs auto-increment (which numbered each PC /
//              headset separately, so IDs collided between devices and could not be matched
//              to the lab's participant list).
//
//              Two input paths, both always active:
//                1) In-VR numeric keypad (built at runtime, no prefab needed) operated with
//                   the index-finger colliders exactly like the task targets
//                   -> works on Android standalone headsets where there is no keyboard.
//                2) Desktop keyboard (digits, Backspace, Enter / KeypadEnter) for PC VR,
//                   with an IMGUI hint drawn on the PC monitor (not visible in the HMD).
//              Confirming an EMPTY ID keeps the original behaviour (automatic numbering).
//
//  Layer     : Runtime, session start-up step (registers in SessionGate).
//  Depends on: SessionInfo / SessionGate, TextMeshPro, the "IndexFinger" tag.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant rebuild of Corsi VR and Deary-Liewald VR.
// =====================================================================================

using System.Collections;
using TMPro;
using UnityEngine;

public class ParticipantIdKeypad : MonoBehaviour
{
    [Tooltip("XR camera (head). If empty, Camera.main is used.")]
    public Camera xrCamera;

    [Tooltip("Distance of the keypad in front of the head (m).")]
    public float distance = 0.45f;

    [Tooltip("How far below eye level the keypad centre is placed (m).")]
    public float belowEyes = 0.25f;

    [Tooltip("Maximum number of digits.")]
    public int maxDigits = 6;

    private string typed = "";
    private bool confirmed;
    private GameObject root;
    private TextMeshPro display;
    private float cooldownUntil;

    private void Awake()
    {
        SessionGate.Register(this);
    }

    private IEnumerator Start()
    {
        if (xrCamera == null) xrCamera = Camera.main;
        // Wait a moment so that HMD tracking has delivered a real head pose before placing the keypad.
        yield return new WaitForSecondsRealtime(1.0f);
        BuildKeypad();
    }

    private void Update()
    {
        if (confirmed) return;

        // ---- desktop keyboard path (PC VR) ----
        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c)) Press(c.ToString());
            else if (c == '\b') Press("C");
            else if (c == '\n' || c == '\r') Press("OK");
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) Press("OK");
    }

    /// <summary>Called by keypad buttons and the keyboard.</summary>
    public void Press(string key)
    {
        if (confirmed) return;
        if (key == "OK")
        {
            Confirm();
            return;
        }
        if (key == "C")
        {
            typed = typed.Length > 0 ? typed.Substring(0, typed.Length - 1) : "";
        }
        else if (typed.Length < maxDigits)
        {
            typed += key;
        }
        RefreshDisplay();
    }

    /// <summary>Finger presses go through here so one touch cannot register several keys.</summary>
    public void PressFromFinger(string key)
    {
        if (Time.unscaledTime < cooldownUntil) return;
        cooldownUntil = Time.unscaledTime + 0.4f;
        Press(key);
    }

    private void Confirm()
    {
        confirmed = true;
        SessionInfo.ParticipantId = int.TryParse(typed, out int id) ? id : 0;
        SessionInfo.ParticipantConfirmed = true;
        if (root != null) Destroy(root);
        SessionGate.Release(this);
        Debug.Log("[ParticipantIdKeypad] Participant ID confirmed: " + (SessionInfo.ParticipantId > 0 ? SessionInfo.ParticipantId.ToString() : "(automatic)"));
    }

    private void RefreshDisplay()
    {
        if (display != null) display.text = typed.Length > 0 ? typed : "_";
    }

    // ------------------------------------------------------------------ runtime keypad

    private void BuildKeypad()
    {
        if (confirmed) return;

        root = new GameObject("ParticipantIdKeypad_Runtime");
        Transform head = xrCamera != null ? xrCamera.transform : null;
        Vector3 fwd = head != null ? Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized : Vector3.forward;
        if (fwd.sqrMagnitude < 0.01f) fwd = Vector3.forward;
        Vector3 headPos = head != null ? head.position : new Vector3(0, 1.6f, 0);
        root.transform.position = headPos + fwd * distance + Vector3.down * belowEyes;
        root.transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
        // Slight tilt towards the participant, like a real keypad on a desk.
        root.transform.Rotate(25f, 0f, 0f, Space.Self);

        // Title + typed-number display (TextMeshPro 3D uses the default font from TMP Settings).
        MakeText(root.transform, "Participant ID  (experimenter)", new Vector3(0, 0.17f, 0), 0.25f, Color.white);
        display = MakeText(root.transform, "_", new Vector3(0, 0.12f, 0), 0.45f, Color.yellow);

        string[] keys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "C", "0", "OK" };
        const float pitch = 0.075f;
        for (int i = 0; i < keys.Length; i++)
        {
            int row = i / 3, col = i % 3;
            Vector3 local = new Vector3((col - 1) * pitch, 0.06f - row * pitch, 0f);
            MakeButton(root.transform, keys[i], local);
        }
        RefreshDisplay();
    }

    private TextMeshPro MakeText(Transform parent, string text, Vector3 local, float size, Color color)
    {
        var go = new GameObject("Text_" + text);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = local;
        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        tmp.rectTransform.sizeDelta = new Vector2(0.4f, 0.05f);
        return tmp;
    }

    private void MakeButton(Transform parent, string key, Vector3 local)
    {
        // CreatePrimitive gives a cube with the built-in default material (always available in players).
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "Key_" + key;
        cube.transform.SetParent(parent, false);
        cube.transform.localPosition = local;
        cube.transform.localScale = new Vector3(0.06f, 0.06f, 0.02f);
        var box = cube.GetComponent<BoxCollider>();
        box.isTrigger = true; // the finger colliders carry a kinematic Rigidbody, so triggers fire
        var rend = cube.GetComponent<Renderer>();
        rend.material.color = key == "OK" ? new Color(0.2f, 0.7f, 0.3f) : key == "C" ? new Color(0.8f, 0.4f, 0.2f) : new Color(0.25f, 0.25f, 0.3f);

        var label = MakeText(cube.transform, key, new Vector3(0, 0, -0.6f), 0.3f, Color.white);
        // Undo the cube's non-uniform scale so the label has world scale 1 (same as the title text).
        label.transform.localScale = new Vector3(1f / 0.06f, 1f / 0.06f, 1f / 0.02f);
        label.rectTransform.sizeDelta = new Vector2(0.06f, 0.05f);

        var btn = cube.AddComponent<KeypadFingerButton>();
        btn.owner = this;
        btn.key = key;
    }

    // Desktop hint (drawn on the PC monitor only).
    private void OnGUI()
    {
        if (confirmed) return;
        GUI.Box(new Rect(20, 20, 520, 70), "");
        GUI.Label(new Rect(30, 28, 500, 25), "Participant ID (type digits, Backspace = delete, Enter = confirm):");
        GUI.Label(new Rect(30, 55, 500, 25), typed.Length > 0 ? typed : "(empty = automatic numbering)");
    }
}

/// <summary>Single key of the runtime keypad: forwards index-finger touches to the keypad.</summary>
public class KeypadFingerButton : MonoBehaviour
{
    public ParticipantIdKeypad owner;
    public string key;

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.CompareTag("IndexFinger")) owner.PressFromFinger(key);
    }
}
