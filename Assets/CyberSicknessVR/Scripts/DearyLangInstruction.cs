using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DearyLangInstruction : MonoBehaviour
{
    public Deary_Manager dearyMan;
    public bool English;
    public bool Greek;
    [Space(10)]
    public AudioClip instructSinglePractice;
    public AudioClip instructSingle;
    public AudioClip instructMultiPractice;
    public AudioClip instructMulti;
    [Space(10)]
    public AudioClip instructSinglePracticeGR;
    public AudioClip instructSingleGR;
    public AudioClip instructMultiPracticeGR;
    public AudioClip instructMultiGR;
    // Start is called before the first frame update
    void Start()
    {
        if (English)
        {
            dearyMan.instructSinglePractice = instructSinglePractice;
            dearyMan.instructSingle = instructSingle;
            dearyMan.instructMultiPractice = instructMultiPractice;
            dearyMan.instructMulti = instructMulti;
        }
        else if (Greek)
        {
            dearyMan.instructSinglePractice = instructSinglePracticeGR;
            dearyMan.instructSingle = instructSingleGR;
            dearyMan.instructMultiPractice = instructMultiPracticeGR;
            dearyMan.instructMulti = instructMultiGR;
        }
    }

    private void OnEnable()
    {
        if (English)
        {
            dearyMan.instructSinglePractice = instructSinglePractice;
            dearyMan.instructSingle = instructSingle;
            dearyMan.instructMultiPractice = instructMultiPractice;
            dearyMan.instructMulti = instructMulti;
        }
        else if (Greek)
        {
            dearyMan.instructSinglePractice = instructSinglePracticeGR;
            dearyMan.instructSingle = instructSingleGR;
            dearyMan.instructMultiPractice = instructMultiPracticeGR;
            dearyMan.instructMulti = instructMultiGR;
        }
    }
}
