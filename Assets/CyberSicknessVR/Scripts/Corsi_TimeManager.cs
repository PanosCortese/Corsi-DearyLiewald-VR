using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Corsi_TimeManager : MonoBehaviour
{
    public CorsiBlockManager corsiMan;
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

    // [2026-10-29 P.K.] motor times of the CURRENT trial only (for the per-trial log).
    // Not serialized: purely runtime bookkeeping, the original lists above are untouched.
    [System.NonSerialized] public List<float> trialMotorTimes = new List<float>();

    /// <summary>[2026-10-29 P.K.] Mean inter-selection time of the current trial (NaN if < 2 selections).</summary>
    public float TrialMeanMotorTime()
    {
        return CsvUtil.SafeAverage(trialMotorTimes);
    }

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
            trialMotorTimes.Clear();      // [2026-10-29 P.K.] new trial -> reset per-trial values
            PlanningTime = float.NaN;     // (PlanningTimes list itself is not affected)
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
            trialMotorTimes.Add(MotorTime); // [2026-10-29 P.K.]
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
        // [2026-10-29 P.K.] SafeAverage instead of Average(): an empty list (e.g. session aborted
        // before the first selection) used to throw here and abort the rest of OnDisable.
        if (!calculations)
        {
            FinishTrial();
            calculations = false;
            AvgTrialTime = CsvUtil.SafeAverage(TrialTimes);
            AvgPlanningTime = CsvUtil.SafeAverage(PlanningTimes);
            AvgMotorTime = CsvUtil.SafeAverage(MotorTimes);
        }
        else
        {
            AvgTrialTime = CsvUtil.SafeAverage(TrialTimes);
            AvgPlanningTime = CsvUtil.SafeAverage(PlanningTimes);
            AvgMotorTime = CsvUtil.SafeAverage(MotorTimes);
            calculations = false;
        }

        TrialTimes.Clear();
        PlanningTimes.Clear();
        MotorTimes.Clear();
    }

}
