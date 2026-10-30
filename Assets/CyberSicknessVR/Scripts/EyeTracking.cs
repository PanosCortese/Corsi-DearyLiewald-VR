using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ViveSR.anipal.Eye;
public class EyeTracking : MonoBehaviour
{
    private FocusInfo FocusInfo;
    public float MaxDistance;
    private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    public GameObject head;
    public bool CSQ;
    public bool Ride;
    public bool digit;
    public bool corsi;
    public bool deary;
    public float PupilSizeR;
    public float PupilSizeL;
    public float EyeOpenR;
    public float EyeOpenL;

    public List<float> Pupil_Size_R;
    public List<float> Pupil_Size_L;
    public List<float> Eye_Open_R;
    public List<float> Eye_Open_L;
    // Start is called before the first frame update
    void Start()
    {
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
        {
            SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
        }
        else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }

        foreach (GazeIndex index in GazePriority)
        {
            Ray GazeRay;
            int dart_board_layer_id = LayerMask.NameToLayer("Eye_Tracking");
            bool eye_focus;
            if (eye_callback_registered)
                eye_focus = SRanipal_Eye_v2.Focus(index, out GazeRay, out FocusInfo, 0, MaxDistance, (1 << dart_board_layer_id), eyeData);
            else
                eye_focus = SRanipal_Eye_v2.Focus(index, out GazeRay, out FocusInfo, 0, MaxDistance, (1 << dart_board_layer_id));

            if (Ride)
            {
                PupilSizeR = eyeData.verbose_data.right.pupil_diameter_mm;
                EyeOpenR = eyeData.verbose_data.right.eye_openness;
                PupilSizeL = eyeData.verbose_data.left.pupil_diameter_mm;
                EyeOpenL = eyeData.verbose_data.left.eye_openness;

                if (EyeOpenR > 0.2f)
                {
                    Pupil_Size_R.Add(PupilSizeR);
                }
                if (EyeOpenL > 0.2f)
                {
                    Pupil_Size_L.Add(PupilSizeL);
                }
                Eye_Open_R.Add(EyeOpenR);
                Eye_Open_L.Add(EyeOpenL);
            }
            else if (CSQ)
            {
                PupilSizeR = eyeData.verbose_data.right.pupil_diameter_mm;
                EyeOpenR = eyeData.verbose_data.right.eye_openness;
                PupilSizeL = eyeData.verbose_data.left.pupil_diameter_mm;
                EyeOpenL = eyeData.verbose_data.left.eye_openness;
                
                if (EyeOpenR > 0.2f)
                {
                    Pupil_Size_R.Add(PupilSizeR);
                }
                if (EyeOpenL > 0.2f)
                {
                    Pupil_Size_L.Add(PupilSizeL);
                }
            }
            else if (corsi)
            {
                PupilSizeR = eyeData.verbose_data.right.pupil_diameter_mm;
                EyeOpenR = eyeData.verbose_data.right.eye_openness;
                PupilSizeL = eyeData.verbose_data.left.pupil_diameter_mm;
                EyeOpenL = eyeData.verbose_data.left.eye_openness;

                // [2026-10-29 P.K.] SRanipal reports -1 (or 0) when an eye is closed / not tracked;
                // such samples are skipped so blinks do not pull the average pupil size down.
                if (PupilSizeR > 0f) Pupil_Size_R.Add(PupilSizeR);
                if (PupilSizeL > 0f) Pupil_Size_L.Add(PupilSizeL);
            }
            else if (digit)
            {
                PupilSizeR = eyeData.verbose_data.right.pupil_diameter_mm;
                EyeOpenR = eyeData.verbose_data.right.eye_openness;
                PupilSizeL = eyeData.verbose_data.left.pupil_diameter_mm;
                EyeOpenL = eyeData.verbose_data.left.eye_openness;

                Pupil_Size_R.Add(PupilSizeR);
                Pupil_Size_L.Add(PupilSizeL);
                Eye_Open_R.Add(EyeOpenR);
                Eye_Open_L.Add(EyeOpenL);
            }
            else if (deary)
            {
                PupilSizeR = eyeData.verbose_data.right.pupil_diameter_mm;
                EyeOpenR = eyeData.verbose_data.right.eye_openness;
                PupilSizeL = eyeData.verbose_data.left.pupil_diameter_mm;
                EyeOpenL = eyeData.verbose_data.left.eye_openness;

                // [2026-10-29 P.K.] skip invalid (closed / untracked eye) samples, see corsi branch.
                if (PupilSizeR > 0f) Pupil_Size_R.Add(PupilSizeR);
                if (PupilSizeL > 0f) Pupil_Size_L.Add(PupilSizeL);
            }

        }

        
    }

    public void Clear()
    {
        Pupil_Size_R.Clear();
        Pupil_Size_L.Clear();
        Eye_Open_R.Clear();
        Eye_Open_L.Clear();
    }
    private void Release()
    {
        if (eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
    }
    private static void EyeCallback(ref EyeData_v2 eye_data)
    {
        eyeData = eye_data;
    }
}

