using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corsi_Selected : MonoBehaviour
{
    public Material current;
    public Material select;
    public Material correct;
    public Material wrong;
    [Space(10)]
    public CorsiBlockManager corsiMan;
    public Corsi_TimeManager corsiManTime;
    [Space(10)]
    public float selectTime;
    public float delay;
    
    public bool done;
    public Collider col;
    [Space(10)]
    public AudioSource audi;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    private bool selectedInitiated; // New Code
    //private bool selectedInitiated; // New Code
    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material = current;
        col = GetComponent<Collider>();
        col.enabled = true;
        audi = GetComponent<AudioSource>();
        done = false;
        selectedInitiated = false; // New Code
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            //print("enter if" + other.gameObject.name);
            if (!selectedInitiated)
            {
                selectTime = Time.time;
                selectedInitiated = true; // New Code
                GetComponent<MeshRenderer>().material = select;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //print("stay " + other.gameObject.name);
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            //print("stay if" + other.gameObject.name);
            //GetComponent<MeshRenderer>().material = select;
            if ((Time.time > (selectTime + delay)) && selectedInitiated) // New Code = && !selectedInitiated
            {
                //print("stay if time" + other.gameObject.name);
                corsiMan.selected = gameObject;
                selectedInitiated = false;
                corsiMan.Selected();
                 // New Code 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            GetComponent<MeshRenderer>().material = current;
            selectedInitiated = false; // New Code
        }
    }

    /// <summary>[2026-10-29 P.K.] Sequence of the current Corsi trial as "Block>Block>..." for the trial log.</summary>
    private string SequenceString(int length)
    {
        GameObject[] seq = { corsiMan.first, corsiMan.second, corsiMan.third, corsiMan.fourth, corsiMan.fifth, corsiMan.sixth, corsiMan.seventh };
        var names = new System.Collections.Generic.List<string>();
        for (int i = 0; i < length && i < seq.Length; i++) names.Add(seq[i] != null ? seq[i].name : "?");
        return string.Join(">", names);
    }

    /// <summary>[2026-10-29 P.K.] Writes one Corsi trial row to the per-trial CSV (TrialLogger).
    /// trialIndex / length are captured BEFORE the manager increments them.</summary>
    private void LogCorsiTrial(string outcome, int trialIndex, int length, string sequence)
    {
        string block = corsiMan.Forward ? "Forward" : "Backward";
        TrialLogger.Log(block, trialIndex + 1, false, length, sequence, null, outcome,
                        corsiManTime.TrialTime, corsiManTime.PlanningTime, corsiManTime.TrialMeanMotorTime(), float.NaN);
    }

    public void Correct()
    {
        //print("Correct");
        // [2026-10-29 P.K.] capture trial state for the per-trial log before it is modified below.
        int logTrial = corsiMan.trial;
        int logLength = corsiMan.quantity;
        string logSequence = SequenceString(logLength);
        bool sequenceCompleted = corsiMan.Forward ? (corsiMan.last != null && gameObject == corsiMan.last)
                                                  : (corsiMan.Backward && gameObject == corsiMan.first);
        col.enabled = false;
        GetComponent<MeshRenderer>().material = correct;
        audi.clip = correctClip;
        audi.Play();
        done = true;
        if (corsiMan.Forward)
        {
            if (corsiMan.last != null)
            {
                if (gameObject == corsiMan.last && corsiMan.trial != 12)
                {
                    corsiMan.mistakes = 0;
                    corsiMan.score += 1;
                    corsiMan.span = corsiMan.quantity;
                    if (corsiMan.trial != 0 && corsiMan.trial % 2 == 0)
                    {
                        corsiMan.quantity += 1;
                    }
                    corsiMan.trial += 1;
                    corsiManTime.FinishTrial();
                    corsiMan.NewRound();
                }
                else if (gameObject == corsiMan.last && corsiMan.trial == 12)
                {
                    corsiManTime.FinishTrial();
                    corsiMan.mistakes = 0;
                    corsiMan.score += 1;
                    corsiMan.span = corsiMan.quantity;
                    corsiMan.delayToDisable = true;
                }
            }

        }
        else if (corsiMan.Backward)
        {
            if (gameObject == corsiMan.first && corsiMan.trial != 12)
            {
                corsiMan.mistakes = 0;
                corsiMan.score += 1;
                corsiMan.span = corsiMan.quantity;
                if (corsiMan.trial != 0 && corsiMan.trial % 2 == 0)
                {
                    corsiMan.quantity += 1;
                }
                corsiMan.trial += 1;
                corsiManTime.FinishTrial();
                corsiMan.NewRound();
            }
            else if (gameObject == corsiMan.first && corsiMan.trial == 12)
            {
                corsiManTime.FinishTrial();
                corsiMan.mistakes = 0;
                corsiMan.score += 1;
                corsiMan.span = corsiMan.quantity;
                corsiMan.delayToDisable = true;
            }
        }

        // [2026-10-29 P.K.] whole sequence reproduced correctly -> one "Correct" trial row.
        if (sequenceCompleted)
        {
            LogCorsiTrial("Correct", logTrial, logLength, logSequence);
        }
    }

    public void Wrong()
    {
        // [2026-10-29 P.K.] capture trial state for the per-trial log before it is modified below.
        int logTrial = corsiMan.trial;
        int logLength = corsiMan.quantity;
        string logSequence = SequenceString(logLength);
        corsiManTime.FinishTrial();
        LogCorsiTrial("Wrong", logTrial, logLength, logSequence);
        col.enabled = false;
        GetComponent<MeshRenderer>().material = wrong;
        audi.clip = wrongClip;
        audi.Play();
        done = true;
        corsiMan.mistakes += 1;
        if (corsiMan.mistakes < 2)
        {
            if (corsiMan.trial == 12)
            {
                corsiMan.delayToDisable = true;
            }
            else if (corsiMan.trial < 12)
            {
                if (corsiMan.trial != 0 && corsiMan.trial % 2 == 0)
                {
                    corsiMan.quantity += 1;
                    corsiMan.mistakes = 0;
                }
                corsiMan.trial += 1;
                corsiMan.NewRound();
            }
            
        }
        else
        {
            corsiMan.delayToDisable = true;
        }
    }

    private void OnDisable()
    {
        if (current != null)
        {
            GetComponent<MeshRenderer>().material = current;
        }
        col.enabled = true;
        selectedInitiated = false; // New Code
    }

}
