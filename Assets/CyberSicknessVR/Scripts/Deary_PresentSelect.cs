using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deary_PresentSelect : MonoBehaviour
{
    public Deary_Manager dearyMan;
    public Material current;
    public Material select;
    public Material correct;
    public Material wrong;
    [Space(10)]
    public AudioSource audi;
    public AudioClip clipCorrect;
    public AudioClip clipWrong;
    [Space(10)]
    public float presentingTime;
    public bool presenting;
    public bool presented;
    public float startTime;
    [Space(10)]
    public Deary_ET_Attention ET_Att;
    public GameObject Other1;
    public GameObject Other2;
    public GameObject Other3;

    // [2026-10-29 P.K.] Per-trial gaze latency (stimulus onset -> gaze on this target), filled by
    // Deary_ET_Attention for EVERY block (the manager's AttentionTimes list is still only fed in
    // the scored choice block, exactly as before). NaN = gaze never reached the target / no ET.
    [System.NonSerialized] public float trialAttentionTime = float.NaN;

    /// <summary>[2026-10-29 P.K.] Block label for the per-trial log, derived from the manager state.</summary>
    private string BlockName()
    {
        if (!dearyMan.multi) return dearyMan.practice ? "SinglePractice" : "Single";
        return dearyMan.practice ? "ChoicePractice" : "Choice";
    }

    /// <summary>[2026-10-29 P.K.] Writes one row to the per-trial CSV (see TrialLogger.cs).
    /// Must be called BEFORE the manager counters are updated / Presentation() is called,
    /// because Presentation() may switch to the next block.</summary>
    private void LogTrial(string target, string response, string outcome, float rt, float attention)
    {
        string block = BlockName();
        TrialLogger.Log(block, TrialLogger.NextTrial(block), dearyMan.practice, dearyMan.interval,
                        target, response, outcome, rt, float.NaN, float.NaN, attention);
    }

    // Start is called before the first frame update
    void Start()
    {
        audi = GetComponent<AudioSource>();
        ET_Att = GetComponent<Deary_ET_Attention>();
        gameObject.GetComponent<MeshRenderer>().material = current;
    }
    private void OnEnable()
    {
        audi = GetComponent<AudioSource>();
        ET_Att = GetComponent<Deary_ET_Attention>();
        gameObject.GetComponent<MeshRenderer>().material = current;
    }

    // Update is called once per frame
    void Update()
    {
        if (presenting)
        {
            if (Time.time > startTime + presentingTime)
            {
                LogTrial(gameObject.name, null, "Slow", float.NaN, trialAttentionTime); // [2026-10-29 P.K.]
                gameObject.GetComponent<MeshRenderer>().material = wrong;
                audi.clip = clipWrong;
                audi.Play();
                presenting = false;
                if (dearyMan.practice)
                {
                    dearyMan.practiceTrial++;
                }
                else if (!dearyMan.practice && !dearyMan.multi)
                {
                    dearyMan.slowResponsesSingle++;
                    dearyMan.trial++;
                }
                else if (!dearyMan.practice && dearyMan.multi)
                {
                    dearyMan.slowResponses++;
                    dearyMan.trial++;
                }
                presented = true;
                dearyMan.Presentation();
            }
        }
        if (presented)
        {
            if (Time.time > startTime + presentingTime + 0.5f)
            {
                gameObject.GetComponent<MeshRenderer>().material = current;
                presented = false;
            }
        }
    }

    public void Presenting()
    {
        startTime = Time.time;
        gameObject.GetComponent<MeshRenderer>().material = select;
        presenting = true;
        ET_Att.Measure = true;
        trialAttentionTime = float.NaN; // [2026-10-29 P.K.] reset per-trial gaze latency
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            if (presenting && dearyMan.target == gameObject)
            {
                LogTrial(gameObject.name, gameObject.name, "Correct", Time.time - startTime, trialAttentionTime); // [2026-10-29 P.K.]
                audi.clip = clipCorrect;
                audi.Play();
                gameObject.GetComponent<MeshRenderer>().material = correct;
                presenting = false;
                if (dearyMan.practice)
                {
                    dearyMan.practiceTrial++;
                }
                else if (!dearyMan.practice && !dearyMan.multi)
                {
                    dearyMan.MotorTimeSingle = Time.time - startTime;
                    dearyMan.MotorTimesSingle.Add(dearyMan.MotorTimeSingle);
                    dearyMan.scoreSingle++;
                    dearyMan.trial++;
                }
                else if (!dearyMan.practice && dearyMan.multi)
                {
                    dearyMan.MotorTime = Time.time - startTime;
                    dearyMan.MotorTimes.Add(dearyMan.MotorTime);
                    dearyMan.score++;
                    dearyMan.trial++;
                }
                dearyMan.Presentation();
            }
            else if (dearyMan.target != gameObject && dearyMan.multi)
            {
                if (Other1.GetComponent<Deary_PresentSelect>().presenting)
                {
                    LogWrong(Other1); // [2026-10-29 P.K.]
                    audi.clip = clipWrong;
                    audi.Play();
                    gameObject.GetComponent<MeshRenderer>().material = wrong;
                    presenting = false;
                    Other1.GetComponent<MeshRenderer>().material = current;
                    Other1.GetComponent<Deary_PresentSelect>().presenting = false;
                    if (dearyMan.practice)
                    {
                        dearyMan.practiceTrial++;
                    }
                    else if (!dearyMan.practice)
                    {
                        dearyMan.mistakes++;
                        dearyMan.trial++;
                    }
                    dearyMan.Presentation();
                }
                else if (Other2.GetComponent<Deary_PresentSelect>().presenting)
                {
                    LogWrong(Other2); // [2026-10-29 P.K.]
                    audi.clip = clipWrong;
                    audi.Play();
                    gameObject.GetComponent<MeshRenderer>().material = wrong;
                    presenting = false;
                    Other2.GetComponent<MeshRenderer>().material = current;
                    Other2.GetComponent<Deary_PresentSelect>().presenting = false;
                    if (dearyMan.practice)
                    {
                        dearyMan.practiceTrial++;
                    }
                    else if (!dearyMan.practice)
                    {
                        dearyMan.mistakes++;
                        dearyMan.trial++;
                    }
                    dearyMan.Presentation();
                }
                else if (Other3.GetComponent<Deary_PresentSelect>().presenting)
                {
                    LogWrong(Other3); // [2026-10-29 P.K.]
                    audi.clip = clipWrong;
                    audi.Play();
                    gameObject.GetComponent<MeshRenderer>().material = wrong;
                    presenting = false;
                    Other3.GetComponent<MeshRenderer>().material = current;
                    Other3.GetComponent<Deary_PresentSelect>().presenting = false;
                    if (dearyMan.practice)
                    {
                        dearyMan.practiceTrial++;
                    }
                    else if (!dearyMan.practice)
                    {
                        dearyMan.mistakes++;
                        dearyMan.trial++;
                    }
                    dearyMan.Presentation();
                }
            }

        }

    }

    /// <summary>[2026-10-29 P.K.] Logs a wrong touch: 'litTarget' was presenting, this object was touched.</summary>
    private void LogWrong(GameObject litTarget)
    {
        var lit = litTarget.GetComponent<Deary_PresentSelect>();
        LogTrial(litTarget.name, gameObject.name, "Wrong", Time.time - lit.startTime, lit.trialAttentionTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            gameObject.GetComponent<MeshRenderer>().material = current;
            audi.Stop();
        }
    }
}
