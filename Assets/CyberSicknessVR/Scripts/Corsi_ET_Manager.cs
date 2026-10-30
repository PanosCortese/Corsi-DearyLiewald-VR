using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Corsi_ET_Manager : MonoBehaviour
{
    public EyeTracking ET;
    public CorsiBlockManager corsiMan;
    public bool readyToCalculate;
    [Space(10)]
    public FixationET_Corsi first;
    public FixationET_Corsi second;
    public FixationET_Corsi third;
    public FixationET_Corsi fourth;
    public FixationET_Corsi fifth;
    public FixationET_Corsi sixth;
    public FixationET_Corsi seventh;
    [Space(10)]
    public int ET_quantity;
    public List<float> FixationCountList;
    public List<float> FixationDurationList;
    public float AvgFixationCount;
    public float AvgFixationDuration;
    public float PupilSizeR;
    public float PupilSizeL;
    public bool ProEye = false; 
    public void Assignment()
    {
        ET_quantity = corsiMan.quantity;
        switch (corsiMan.quantity)
        {
            case 2: 
                first = corsiMan.first.GetComponent<FixationET_Corsi>();
                second = corsiMan.second.GetComponent<FixationET_Corsi>();
                readyToCalculate = true;
                break;
            case 3:
                first = corsiMan.first.GetComponent<FixationET_Corsi>();
                second= corsiMan.second.GetComponent<FixationET_Corsi>();
                third = corsiMan.third.GetComponent<FixationET_Corsi>();
                readyToCalculate = true;
                break;
            case 4:
                first = corsiMan.first.GetComponent<FixationET_Corsi>();
                second = corsiMan.second.GetComponent<FixationET_Corsi>();
                third = corsiMan.third.GetComponent<FixationET_Corsi>();
                fourth = corsiMan.fourth.GetComponent<FixationET_Corsi>();
                readyToCalculate = true;
                break;
            case 5:
                first = corsiMan.first.GetComponent<FixationET_Corsi>();
                second = corsiMan.second.GetComponent<FixationET_Corsi>();
                third = corsiMan.third.GetComponent<FixationET_Corsi>();
                fourth = corsiMan.fourth.GetComponent<FixationET_Corsi>();
                fifth = corsiMan.fifth.GetComponent<FixationET_Corsi>();
                readyToCalculate = true;
                break;
            case 6:
                first = corsiMan.first.GetComponent<FixationET_Corsi>();
                second = corsiMan.second.GetComponent<FixationET_Corsi>();
                third = corsiMan.third.GetComponent<FixationET_Corsi>();
                fourth = corsiMan.fourth.GetComponent<FixationET_Corsi>();
                fifth = corsiMan.fifth.GetComponent<FixationET_Corsi>();
                sixth = corsiMan.sixth.GetComponent<FixationET_Corsi>();
                readyToCalculate = true;
                break;
            case 7:
                first = corsiMan.first.GetComponent<FixationET_Corsi>();
                second = corsiMan.second.GetComponent<FixationET_Corsi>();
                third = corsiMan.third.GetComponent<FixationET_Corsi>();
                fourth = corsiMan.fourth.GetComponent<FixationET_Corsi>();
                fifth = corsiMan.fifth.GetComponent<FixationET_Corsi>();
                sixth = corsiMan.sixth.GetComponent<FixationET_Corsi>();
                seventh = corsiMan.seventh.GetComponent<FixationET_Corsi>();
                readyToCalculate = true;
                break;
        }
    }

    public void Calculation()
    {
        switch (ET_quantity)
        {
            case 2:
                first.Calculation();
                second.Calculation();
                FixationCountList.Add(first.fixationCount);
                FixationCountList.Add(second.fixationCount);
                FixationDurationList.Add(first.fixationDuration);
                FixationDurationList.Add(second.fixationDuration);
                first.Clear();
                second.Clear();
                break;
            case 3:
                first.Calculation();
                second.Calculation();
                third.Calculation();
                FixationCountList.Add(first.fixationCount);
                FixationCountList.Add(second.fixationCount);
                FixationCountList.Add(third.fixationCount);
                FixationDurationList.Add(first.fixationDuration);
                FixationDurationList.Add(second.fixationDuration);
                FixationDurationList.Add(third.fixationDuration);
                first.Clear();
                second.Clear();
                third.Clear();
                break;
            case 4:
                first.Calculation();
                second.Calculation();
                third.Calculation();
                fourth.Calculation();
                FixationCountList.Add(first.fixationCount);
                FixationCountList.Add(second.fixationCount);
                FixationCountList.Add(third.fixationCount);
                FixationCountList.Add(fourth.fixationCount);
                FixationDurationList.Add(first.fixationDuration);
                FixationDurationList.Add(second.fixationDuration);
                FixationDurationList.Add(third.fixationDuration);
                FixationDurationList.Add(fourth.fixationDuration);
                first.Clear();
                second.Clear();
                third.Clear();
                fourth.Clear();
                break;
            case 5:
                first.Calculation();
                second.Calculation();
                third.Calculation();
                fourth.Calculation();
                fifth.Calculation();
                FixationCountList.Add(first.fixationCount);
                FixationCountList.Add(second.fixationCount);
                FixationCountList.Add(third.fixationCount);
                FixationCountList.Add(fourth.fixationCount);
                FixationCountList.Add(fifth.fixationCount);
                FixationDurationList.Add(first.fixationDuration);
                FixationDurationList.Add(second.fixationDuration);
                FixationDurationList.Add(third.fixationDuration);
                FixationDurationList.Add(fourth.fixationDuration);
                FixationDurationList.Add(fifth.fixationDuration);
                first.Clear();
                second.Clear();
                third.Clear();
                fourth.Clear();
                fifth.Clear();
                break;
            case 6:
                first.Calculation();
                second.Calculation();
                third.Calculation();
                fourth.Calculation();
                fifth.Calculation();
                sixth.Calculation();
                FixationCountList.Add(first.fixationCount);
                FixationCountList.Add(second.fixationCount);
                FixationCountList.Add(third.fixationCount);
                FixationCountList.Add(fourth.fixationCount);
                FixationCountList.Add(fifth.fixationCount);
                FixationCountList.Add(sixth.fixationCount);
                FixationDurationList.Add(first.fixationDuration);
                FixationDurationList.Add(second.fixationDuration);
                FixationDurationList.Add(third.fixationDuration);
                FixationDurationList.Add(fourth.fixationDuration);
                FixationDurationList.Add(fifth.fixationDuration);
                FixationDurationList.Add(sixth.fixationDuration);
                first.Clear();
                second.Clear();
                third.Clear();
                fourth.Clear();
                fifth.Clear();
                sixth.Clear();
                break;
            case 7:
                first.Calculation();
                second.Calculation();
                third.Calculation();
                fourth.Calculation();
                fifth.Calculation();
                sixth.Calculation();
                seventh.Calculation();
                FixationCountList.Add(first.fixationCount);
                FixationCountList.Add(second.fixationCount);
                FixationCountList.Add(third.fixationCount);
                FixationCountList.Add(fourth.fixationCount);
                FixationCountList.Add(fifth.fixationCount);
                FixationCountList.Add(sixth.fixationCount);
                FixationCountList.Add(seventh.fixationCount);
                FixationDurationList.Add(first.fixationDuration);
                FixationDurationList.Add(second.fixationDuration);
                FixationDurationList.Add(third.fixationDuration);
                FixationDurationList.Add(fourth.fixationDuration);
                FixationDurationList.Add(fifth.fixationDuration);
                FixationDurationList.Add(sixth.fixationDuration);
                FixationDurationList.Add(seventh.fixationDuration);
                first.Clear();
                second.Clear();
                third.Clear();
                fourth.Clear();
                fifth.Clear();
                sixth.Clear();
                seventh.Clear();
                break;
        }
        ET_quantity = 0;
        readyToCalculate = false;
    }
    private void OnDisable()
    {
        // [2026-10-29 P.K.] BUG FIX: Average() on an empty list threw (e.g. aborted session) and
        // Sum()/Sum() gave 0/0 = NaN when no fixation was recorded (no eye tracking). Both now
        // return NaN on purpose, which Score_Manager writes as "NA".
        AvgFixationCount = CsvUtil.SafeAverage(FixationCountList);
        AvgFixationDuration = CsvUtil.SafeDivide(FixationDurationList.Sum(), FixationCountList.Sum());
        FixationCountList.Clear();
        FixationDurationList.Clear();
        // 'ProEye' = "this variant has pupillometry" (Pro Eye or Varjo); name kept for scene compatibility.
        if (ProEye)
        {
            PupilSizeR = CsvUtil.SafeAverage(ET.Pupil_Size_R);
            PupilSizeL = CsvUtil.SafeAverage(ET.Pupil_Size_L);
            ET.Clear();
        }

    }
}
