using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deary_ET_Attention : MonoBehaviour
{
    public Deary_Manager dearyMan;
    public Deary_PresentSelect dearySel;
    public bool Measure;

    private void OnEnable()
    {
        dearySel = GetComponent<Deary_PresentSelect>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ET_Pointer") && dearySel.presenting)
        {
            if (Measure)
            {
                // [2026-10-29 P.K.] per-trial gaze latency for the trial log (all blocks).
                if (float.IsNaN(dearySel.trialAttentionTime))
                {
                    dearySel.trialAttentionTime = Time.time - dearySel.startTime;
                }
                if (!dearyMan.practice && dearyMan.multi)
                {
                    dearyMan.AttentionTime = Time.time - dearySel.startTime;
                    dearyMan.AttentionTimes.Add(dearyMan.AttentionTime);
                    Measure = false;
                }
            }
        }
    }
}
