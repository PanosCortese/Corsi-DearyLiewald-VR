using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Digits_TimeManager : MonoBehaviour
{
    public DigitRecallManager digiMan;
    [Space(10)]
    public float StartTrialTime;
    public float EndTrialTime;
    public float TrialTime;
    [Space(10)]
    public float SelectionTime;
    public float PreviousSelectionTime;
    [Space(10)]
    public float PlanningTime;
    public float MotorTime;
    [Space(10)]
    public float AvgTrialTime;
    public float AvgPlanningTime;
    public float AvgMotorTime;
    [Space(10)]
    public List<float> TrialTimes;
    public List<float> PlanningTimes;
    public List<float> MotorTimes;
    [Space(10)]
    public AudioSource starting;
    public bool start;
    public bool calculations;
    public bool firstSelected;

    // Update is called once per frame
    void Update()
    {
        if (starting.isPlaying && !start)
        {
            start = true;
        }
        else if (!starting.isPlaying && start)
        {
            calculations = false;
            StartTrialTime = Time.time;
            //Debug.Log("Trial started at : " + StartTrialTime);
            start = false;
        }
    }
    public void SelectioTimeCalc()
    {
        if (firstSelected)
        {
            SelectionTime = Time.time;
            PreviousSelectionTime = SelectionTime;
            PlanningTime = SelectionTime - StartTrialTime;
            PlanningTimes.Add(PlanningTime);
            firstSelected = false;
        }
        else
        {
            SelectionTime = Time.time;
            MotorTime = SelectionTime - PreviousSelectionTime;
            PreviousSelectionTime = SelectionTime;
            MotorTimes.Add(MotorTime);
        }

    }
    public void FinishTrial()
    {
        EndTrialTime = Time.time;
        TrialTime = EndTrialTime - StartTrialTime;
        TrialTimes.Add(TrialTime);
        calculations = true;
    }

    private void OnDisable()
    {
        if (!calculations)
        {
            FinishTrial();
            calculations = false;
            AvgTrialTime = TrialTimes.Average();
            AvgPlanningTime = PlanningTimes.Average();
            AvgMotorTime = MotorTimes.Average();
        }
        else
        {
            AvgTrialTime = TrialTimes.Average();
            AvgPlanningTime = PlanningTimes.Average();
            AvgMotorTime = MotorTimes.Average();
            calculations = false;
        }

        TrialTimes.Clear();
        PlanningTimes.Clear();
        MotorTimes.Clear();
    }
}
