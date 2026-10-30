using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deary_Manager : MonoBehaviour
{
    public EyeTracking ET;
    public bool multi;
    public bool practice;
    public bool toPresent;
    public bool presenting;
    public bool instructions;
    [Space(10)]
    public GameObject Single;
    public GameObject Multiple;
    [Space(10)]
    public AudioSource audi;
    public AudioClip instructSinglePractice;
    public AudioClip instructSingle;
    public AudioClip instructMultiPractice;
    public AudioClip instructMulti;
    [Space(10)]
    public GameObject Instruction1;
    public GameObject Instruction2;
    public GameObject Instruction3;
    public GameObject Instruction4;
    [Space(10)]
    public GameObject Multi1;
    public GameObject Multi2;
    public GameObject Multi3;
    public GameObject Multi4;
    [Space(20)]
    public float previousInterval;
    public float interval;
    public List<float> intervals;
    public int nextInterval;
    [Space(20)]
    public GameObject previousTarget;
    public GameObject target;
    public List<GameObject> targets;
    public int nextTarget;
    [Space(20)]
    public float toPresentTime;
    public int trial;
    public int practiceTrial;
    [Space(10)]
    public int score;
    public int mistakes;
    public int slowResponses;
    [Space(10)]
    public int scoreSingle;
    public int slowResponsesSingle;
    [Space(10)]
    public float AttentionTime;
    public float MotorTime;
    public float PureMotorTime;
    public List<float> AttentionTimes;
    public List<float> MotorTimes;
    [Space(10)]
    public float MotorTimeSingle;
    public List<float> MotorTimesSingle;
    [Space(10)]
    public float PupilSizeR;
    public float PupilSizeL;
    public bool DONE;
    public bool ProEye;
    // Start is called before the first frame update
    void Start()
    {
        intervals = new List<float>() { 1f, 1.5f, 2f, 2.5f, 3f };
        targets = new List<GameObject>() { Multi1, Multi2, Multi3, Multi4 };
        
        Single.SetActive(true);
        Multiple.SetActive(false);

        multi = false;
        practice = true;
        instructions = true;

        previousInterval = 0;
        previousTarget = null;

        practiceTrial = 0;
        trial = 0;
        score = 0;
        mistakes = 0;
        slowResponses = 0;
    }
    private void OnEnable()
    {
        //DONE = false;
        intervals = new List<float>() { 1f, 1.5f, 2f, 2.5f, 3f };
        targets = new List<GameObject>() { Multi1, Multi2, Multi3, Multi4 };
        
        Single.SetActive(true);
        Multiple.SetActive(false);
        
        multi = false;
        practice = true;
        instructions = true;

        previousInterval = 0;
        previousTarget = null;

        practiceTrial = 0;
        trial = 0;
        score = 0;
        mistakes = 0;
        slowResponses = 0;

        scoreSingle = 0;// NEW CODE
        slowResponsesSingle = 0;// NEW CODE

        AttentionTimes.Clear();
        MotorTimes.Clear();
        MotorTimesSingle.Clear();

        ET.deary = true;

        Instruction1.SetActive(true);
        audi.clip = instructSinglePractice;
        audi.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (instructions && audi.isPlaying)
        {
            return;
        }
        else if (instructions && !audi.isPlaying)
        {
            instructions = false;
            Presentation();
        }
        if (toPresent)
        {
            if (Time.time > toPresentTime + interval)
            {
                target.GetComponent<Deary_PresentSelect>().Presenting();
                toPresent = false;
            }   
        }
    }

    public void Presentation()
    {
        CheckTrials();
        if (instructions)
        {
            if (!practice && !multi)
            {
                Instruction2.SetActive(true);
                audi.clip = instructSingle;
                audi.Play();
            }
            else if (practice && multi)
            {
                Instruction3.SetActive(true);
                audi.clip = instructMultiPractice;
                audi.Play();
            }
            else if (!practice && multi)
            {
                Instruction4.SetActive(true);
                audi.clip = instructMulti;
                audi.Play();
            }
        }
        else if (!instructions)
        {
            if (!multi)
            {
                nextInterval = Random.Range(0, intervals.Count);
                interval = intervals[nextInterval];
                intervals.Remove(interval);

                if (previousInterval != 0)
                {
                    intervals.Add(previousInterval);
                    previousInterval = interval;
                }
                else
                {
                    previousInterval = interval;
                }

                target = Single;
            }
            else if (multi)
            {
                nextInterval = Random.Range(0, intervals.Count);
                interval = intervals[nextInterval];
                intervals.Remove(interval);

                nextTarget = Random.Range(0, targets.Count);
                target = targets[nextTarget];
                targets.Remove(target);

                if (previousInterval != 0)
                {
                    intervals.Add(previousInterval);
                    previousInterval = interval;
                }
                else
                {
                    previousInterval = interval;
                }

                if (previousTarget != null)
                {
                    targets.Add(previousTarget);
                    previousTarget = target;
                }
                else
                {
                    previousTarget = target;
                }
            }
            toPresentTime = Time.time;
            toPresent = true;
        }

    }

    private void OnDisable()
    {
        DONE = true;
        
        // [2026-10-29 P.K.] 'ProEye' now means "this variant has pupillometry" (Pro Eye OR Varjo);
        // the field name is kept so existing scene data stay valid. SafeAverage() returns NaN
        // (written as "NA") instead of throwing when no pupil sample was collected.
        if (ProEye) 
        {
            ET.deary = false;
            PupilSizeR = CsvUtil.SafeAverage(ET.Pupil_Size_R);
            PupilSizeL = CsvUtil.SafeAverage(ET.Pupil_Size_L);
            ET.Clear();
        }
        intervals = new List<float>() { 1f, 1.5f, 2f, 2.5f, 3f };
        targets = new List<GameObject>() { Multi1, Multi2, Multi3, Multi4 };
        
        Single.SetActive(true);
        Multiple.SetActive(false);

        multi = false;
        practice = true;
        instructions = true;

        previousInterval = 0;
        previousTarget = null;

        // Scoring
        
        // [2026-10-29 P.K.] BUG FIX: the original Average() calls threw on an empty list
        // (no eye tracking -> AttentionTimes empty; all responses slow -> MotorTimes empty).
        // The exception aborted OnDisable, so MotorTime / PureMotorTime were never computed
        // and ChoiceReactionTime was saved as 0. SafeAverage() returns NaN -> "NA" in the CSV.
        MotorTimeSingle = CsvUtil.SafeAverage(MotorTimesSingle);

        AttentionTime = CsvUtil.SafeAverage(AttentionTimes);
        MotorTime = CsvUtil.SafeAverage(MotorTimes);
        PureMotorTime = MotorTime - AttentionTime; // NaN if either part is missing
        
    }

    public void CheckTrials()
    {
        if (practice)
        {
            if (practiceTrial == 8) 
            {
                practice = false;
                practiceTrial = 0;
                instructions = true;
            }
        }
        else if (!practice)
        {
            if (!multi)
            {
                if (trial == 20)
                {
                    practice = true;
                    multi = true;
                    trial = 0;
                    instructions = true;
                    Single.SetActive(false);
                    Multiple.SetActive(true);
                }
            }
            else if (multi)
            {
                if (trial == 40)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
