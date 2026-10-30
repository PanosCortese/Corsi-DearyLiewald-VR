using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digits_Presentation : MonoBehaviour
{
    public bool English;
    public bool Greek;
    public AudioSource audi;
    public GameObject next;
    public GameObject digits;
    public DigitRecallManager digiMan;
    public EyeTracking ET;
    public AudioSource GO;
    public AudioSource Notify;
    public float presentTime;
    public float delay = 0.5f;
    public bool presentation;
    public bool presenting;
    public bool playing;
    public bool delaying;
    public bool assignment;
    [Space(10)]
    public AudioClip EN_one;
    public AudioClip EN_two;
    public AudioClip EN_three;
    public AudioClip EN_four;
    public AudioClip EN_five;
    public AudioClip EN_six;
    public AudioClip EN_seven;
    public AudioClip EN_eight;
    public AudioClip EN_nine;
    [Space(10)]
    public AudioClip GR_one;
    public AudioClip GR_two;
    public AudioClip GR_three;
    public AudioClip GR_four;
    public AudioClip GR_five;
    public AudioClip GR_six;
    public AudioClip GR_seven;
    public AudioClip GR_eight;
    public AudioClip GR_nine;
    [Space(10)]
    public AudioClip first;
    public AudioClip second;
    public AudioClip third;
    public AudioClip fourth;
    public AudioClip fifth;
    public AudioClip sixth;
    public AudioClip seventh;
    [Space(10)]
    public bool first_Done;
    public bool second_Done;
    public bool third_Done;
    public bool fourth_Done;
    public bool fifth_Done;
    public bool sixth_Done;
    public bool seventh_Done;
    public bool allDone;
    public bool play;
    // Start is called before the first frame update
    void Start()
    {
        audi = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (presentation)
        {
            if (!assignment)
            {
                Assignment();
            }
            else if (assignment)
            {
                Notify.Play();
                playing = true;
                presentation = false;
                assignment = false;
            }
        }
        if (playing)
        {
            if (Notify.isPlaying)
            {
                return;
            }
            else
            {
                ET.digit = true;
                presenting = true;
                playing = false;
            }
        }
        if (presenting)
        {
            PresentingNow();
            presenting = false;
        }
        if ((play && !audi.isPlaying) || play && allDone)
        {
            presentTime = Time.time;
            delaying = true;
            play = false;
        }
        if (delaying)
        {
            if (Time.time <= presentTime + delay)
            {
                return;
            }
            else
            {
                if (allDone)
                {
                    digits.SetActive(true);
                    GO.Play();
                    delaying = false;
                    allDone = false;
                    first_Done = false;
                    second_Done = false;
                    third_Done = false;
                    fourth_Done = false;
                    fifth_Done = false;
                    sixth_Done = false;
                    seventh_Done = false;
                    ET.digit = false;
                }
                else
                {
                    presenting = true;
                    delaying = false;
                }
            }
        }
    }
    public void Clear()
    {
        first = null;
        second = null;
        third = null;
        fourth = null;
        fifth = null;
        sixth = null;
        seventh = null;

    }
    private void PresentingNow()
    {
        if (!first_Done && first != null)
        {
            audi.clip = first;
            audi.Play();
            play = true;
            first_Done = true;
        }
        else if (!second_Done && second != null)
        {
            audi.clip = second;
            audi.Play();
            play = true;
            second_Done=true;
        }
        else if (!third_Done && third != null)
        {
            audi.clip = third;
            audi.Play();
            play = true;
            third_Done=true;
        }
        else if (!fourth_Done && fourth != null)
        {
            audi.clip = fourth;
            audi.Play();
            play = true;
            fourth_Done=true;
        }
        else if (!fifth_Done && fifth != null)
        {
            audi.clip = fifth;
            audi.Play();
            play = true;
            fifth_Done=true;
        }
        else if (!sixth_Done && sixth != null)
        {
            audi.clip = sixth;
            audi.Play();
            play = true;
            sixth_Done=true;
        }
        else if (!seventh_Done && seventh != null)
        {
            audi.clip = seventh;
            audi.Play();
            play = true;
            seventh_Done=true;
        }
        else
        {
            play = true;
            allDone = true;
        }
    }
    private void Assignment()
    {
        if (English)
        {
            if (digiMan.first != null)
            {
                next = digiMan.first;
                if (next == digiMan.One)
                {
                    first = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    first = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    first = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    first = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    first = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    first = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    first = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    first = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    first = EN_nine;
                }
            }
            if (digiMan.second != null)
            {
                next = digiMan.second;
                if (next == digiMan.One)
                {
                    second = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    second = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    second = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    second = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    second = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    second = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    second = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    second = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    second = EN_nine;
                }
            }
            if (digiMan.third != null)
            {
                next = digiMan.third;
                if (next == digiMan.One)
                {
                    third = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    third = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    third = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    third = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    third = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    third = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    third = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    third = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    third = EN_nine;
                }
            }
            if (digiMan.fourth != null)
            {
                next = digiMan.fourth;
                if (next == digiMan.One)
                {
                    fourth = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    fourth = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    fourth = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    fourth = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    fourth = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    fourth = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    fourth = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    fourth = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    fourth = EN_nine;
                }
            }
            if (digiMan.fifth != null)
            {
                next = digiMan.fifth;
                if (next == digiMan.One)
                {
                    fifth = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    fifth = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    fifth = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    fifth = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    fifth = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    fifth = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    fifth = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    fifth = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    fifth = EN_nine;
                }
            }
            if (digiMan.sixth != null)
            {
                next = digiMan.sixth;
                if (next == digiMan.One)
                {
                    sixth = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    sixth = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    sixth = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    sixth = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    sixth = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    sixth = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    sixth = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    sixth = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    sixth = EN_nine;
                }
            }
            if (digiMan.seventh != null)
            {
                next = digiMan.seventh;
                if (next == digiMan.One)
                {
                    seventh = EN_one;
                }
                else if (next == digiMan.Two)
                {
                    seventh = EN_two;
                }
                else if (next == digiMan.Three)
                {
                    seventh = EN_three;
                }
                else if (next == digiMan.Four)
                {
                    seventh = EN_four;
                }
                else if (next == digiMan.Five)
                {
                    seventh = EN_five;
                }
                else if (next == digiMan.Six)
                {
                    seventh = EN_six;
                }
                else if (next == digiMan.Seven)
                {
                    seventh = EN_seven;
                }
                else if (next == digiMan.Eight)
                {
                    seventh = EN_eight;
                }
                else if (next == digiMan.Nine)
                {
                    seventh = EN_nine;
                }
            }

        }
        else if (Greek)
        {
            if (digiMan.first != null)
            {
                next = digiMan.first;
                if (next == digiMan.One)
                {
                    first = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    first = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    first = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    first = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    first = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    first = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    first = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    first = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    first = GR_nine;
                }
            }
            if (digiMan.second != null)
            {
                next = digiMan.second;
                if (next == digiMan.One)
                {
                    second = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    second = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    second = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    second = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    second = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    second = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    second = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    second = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    second = GR_nine;
                }
            }
            if (digiMan.third != null)
            {
                next = digiMan.third;
                if (next == digiMan.One)
                {
                    third = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    third = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    third = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    third = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    third = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    third = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    third = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    third = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    third = GR_nine;
                }
            }
            if (digiMan.fourth != null)
            {
                next = digiMan.fourth;
                if (next == digiMan.One)
                {
                    fourth = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    fourth = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    fourth = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    fourth = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    fourth = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    fourth = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    fourth = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    fourth = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    fourth = GR_nine;
                }
            }
            if (digiMan.fifth != null)
            {
                next = digiMan.fifth;
                if (next == digiMan.One)
                {
                    fifth = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    fifth = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    fifth = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    fifth = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    fifth = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    fifth = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    fifth = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    fifth = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    fifth = GR_nine;
                }
            }
            if (digiMan.sixth != null)
            {
                next = digiMan.sixth;
                if (next == digiMan.One)
                {
                    sixth = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    sixth = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    sixth = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    sixth = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    sixth = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    sixth = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    sixth = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    sixth = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    sixth = GR_nine;
                }
            }
            if (digiMan.seventh != null)
            {
                next = digiMan.seventh;
                if (next == digiMan.One)
                {
                    seventh = GR_one;
                }
                else if (next == digiMan.Two)
                {
                    seventh = GR_two;
                }
                else if (next == digiMan.Three)
                {
                    seventh = GR_three;
                }
                else if (next == digiMan.Four)
                {
                    seventh = GR_four;
                }
                else if (next == digiMan.Five)
                {
                    seventh = GR_five;
                }
                else if (next == digiMan.Six)
                {
                    seventh = GR_six;
                }
                else if (next == digiMan.Seven)
                {
                    seventh = GR_seven;
                }
                else if (next == digiMan.Eight)
                {
                    seventh = GR_eight;
                }
                else if (next == digiMan.Nine)
                {
                    seventh = GR_nine;
                }
            }
        }

        assignment = true;
    }

  
}
