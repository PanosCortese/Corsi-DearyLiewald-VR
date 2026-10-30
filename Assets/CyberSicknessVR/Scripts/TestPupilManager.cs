using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestPupilManager : MonoBehaviour
{
    public EyeTracking eye;
    public TextMeshProUGUI amountR;
    public TextMeshProUGUI amountL;
    public TextMeshProUGUI amountOpenR;
    public TextMeshProUGUI amountOpenL;
    public TextMeshProUGUI amountIrisR;
    public TextMeshProUGUI amountIrisL;
    public bool VarjoET;
    public EyeTrackingVarjo ET;
    // Start is called before the first frame update
    void Start()
    {
        eye = GetComponent<EyeTracking>();
    }

    // Update is called once per frame
    void Update()
    {

        if (VarjoET)
        {
            amountL.text = ET.leftPupilDiameterInMM.ToString();
            amountR.text = ET.rightPupilDiameterInMM.ToString();
            amountOpenL.text = ET.leftEyeOpenness.ToString();
            amountOpenR.text = ET.rightEyeOpenness.ToString();
            amountIrisL.text = ET.leftIrisDiameterInMM.ToString();
            amountIrisR.text = ET.rightIrisDiameterInMM.ToString();
        }
        else
        {
            amountL.text = eye.PupilSizeL.ToString();
            amountR.text = eye.PupilSizeR.ToString();
            amountOpenL.text = eye.EyeOpenL.ToString();
            amountOpenR.text = eye.EyeOpenR.ToString();
        }
    }
}
