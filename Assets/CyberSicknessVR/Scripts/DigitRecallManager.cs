using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DigitRecallManager : MonoBehaviour
{
    public bool Forward;
    public bool Backward;
    [Space(10)]
    public GameObject Speaker;
    public GameObject Digits;
    public Digits_TimeManager TimeMan;
    public EyeTracking ET;
    public FixationET_Digit fixET;
    [Space(10)]
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    public GameObject Seven;
    public GameObject Eight;
    public GameObject Nine;
    [Space(10)]
    public bool NewRound;
    [Space(10)]
    public bool delay;
    public bool once;
    public float currentTime;
    public float delayTime;
    [Space(10)]
    public int loop;
    public int num;
    public int previous_num;
    public int obj;
    public List <int> rand;
    public int randList;
    public List<int> numbers1;
    public List<int> numbers2;
    public List<int> numbers3;
    [Space(10)]
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject fourth;
    public GameObject fifth;
    public GameObject sixth;
    public GameObject seventh;
    [Tooltip("This will be always ZERO")]
    public GameObject eighth;
    public GameObject last;
    public GameObject selected;
    [Space(10)]
    public int mistakes;
    public int quantity;
    public int trial;
    [Space(10)]
    public int score;
    public int span;
    public int totalScore;
    [Space(10)]
    public float AvgPupilR;
    public float AvgPupilL;
    public float AvgOpenEyeR;
    public float AvgOpenEyeL;
    public float fixationCount;
    public float fixationDuration;
    public float AvgFixationDuration;
    private bool firstDone;
    private bool secondDone;
    private bool thirdDone;
    private bool fourthDone;
    private bool fifthDone;
    private bool sixthDone;
    private bool seventhDone;
    public bool lastCorrect;
    public bool delayToDisable;
    public bool DONE;
    public AudioSource Intro;
    private bool introPlay;
    private bool introPlayed;
    [Space(10)]
    public GameObject Next;

#if UNITY_EDITOR
    [CustomEditor(typeof(DigitRecallManager))]
    public class DigitRecallManagerEditor : Editor
    {
        private bool swap;
        public override void OnInspectorGUI()
        {
            DigitRecallManager digiMan = (DigitRecallManager)target;
            DrawDefaultInspector();
            if (!swap)
            {
                if (digiMan.Forward)
                {
                    digiMan.Backward = !digiMan.Forward;
                    swap = true;
                }
                else if (digiMan.Backward)
                {
                    digiMan.Forward = !digiMan.Backward;
                    swap = true;
                }
            }
            else if (swap)
            {
                if (digiMan.Backward)
                {
                    digiMan.Forward = !digiMan.Backward;
                    swap = false;
                }
                else if (digiMan.Forward)
                {
                    digiMan.Backward = !digiMan.Forward;
                    swap = false;
                }
            }

        }
    }
#endif


    private void OnEnable()
    {
        if (first != null)
        {
            first = null;
        }
        if (second != null)
        {
            second = null;
        }
        if (third != null)
        {
            third = null;
        }
        if (fourth != null)
        {
            fourth = null;
        }
        if (fifth != null)
        {
            fifth = null;
        }
        if (sixth != null)
        {
            sixth = null;
        }
        if (seventh != null)
        {
            seventh = null;
        }
        firstDone = false;
        secondDone = false;
        thirdDone = false;
        fourthDone = false;
        fifthDone = false;
        sixthDone = false;
        seventhDone = false;
        numbers1 = new List<int>() {1, 2, 3};
        numbers2 = new List<int>() {4, 5, 6};
        numbers3 = new List<int>() {7, 8, 9};
        rand = new List<int> {1,2,3};
        quantity = 2;
        trial = 0;
        mistakes = 0;
        score = 0;
        span = 0;
        totalScore = 0;
        //NewRound = true; // testing if it works fine
        //Intro.Play(); /// testing
        introPlay = true;
        introPlayed = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (first != null)
        {
            first = null;
        }
        if (second != null)
        {
            second = null;
        }
        if (third != null)
        {
            third = null;
        }
        if (fourth != null)
        {
            fourth = null;
        }
        if (fifth != null)
        {
            fifth = null;
        }
        if (sixth != null)
        {
            sixth = null;
        }
        if (seventh != null)
        {
            seventh = null;
        }
        firstDone = false;
        secondDone = false;
        thirdDone = false;
        fourthDone = false;
        fifthDone = false;
        sixthDone = false;
        seventhDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (introPlay)
        {
            Intro.Play();
            introPlay = false;
        }
        else if (!Intro.isPlaying && !introPlayed)
        {
            NewRound = true;
            introPlayed = true;
        }

        if (delayToDisable)
        {
            if (once == false)
            {
                currentTime = Time.time;
                once = true;
            }
            if (Time.time > currentTime + delayTime)
            {
                once = false; 
                Digits.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        if (delay && once == false)
        {
            currentTime = Time.time;
            once = true;
            if (first != null)
            {
                first = null;
            }
            if (second != null)
            {
                second = null;
            }
            if (third != null)
            {
                third = null;
            }
            if (fourth != null)
            {
                fourth = null;
            }
            if (fifth != null)
            {
                fifth = null;
            }
            if (sixth != null)
            {
                sixth = null;
            }
            if (seventh != null)
            {
                seventh = null;
            }
            firstDone = false;
            secondDone = false;
            thirdDone = false;
            fourthDone = false;
            fifthDone = false;
            sixthDone = false;
            seventhDone = false;
        }

        if (delay && Time.time > currentTime + delayTime)
        {
            NewRound = true;
            Digits.SetActive(false);
            delay = false;
            once = false;
        }

        if (NewRound)
        {
            TargetForSelection();
        }
    }

    private void OnDisable()
    {
        DONE = true;
        delayToDisable = false;
        totalScore = score + span;
        fixET.Calculation();
        fixationCount = fixET.fixationCount;
        fixationDuration = fixET.fixationDuration;
        AvgFixationDuration = fixET.AvgFixationDuration;
        AvgPupilR = ET.Pupil_Size_R.Average();
        AvgPupilL = ET.Pupil_Size_L.Average();
        AvgOpenEyeR = ET.Eye_Open_R.Average();
        AvgOpenEyeL = ET.Eye_Open_L.Average();
        ET.Clear();
        fixET.Clear();
        quantity = 2;
        trial = 0;
        mistakes = 0;
        if (Next != null)
        {
            Next.SetActive(true);
        }
    }

    public void Selected()
    {

        if (Forward)
        {
            switch (quantity)
            {
                case 2:
                    if (trial == 0)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            last = second;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                            secondDone = true;
                        }

                    }
                    else if (trial == 1)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            last = second;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 2)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                            
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            last = second;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 3:
                    if (trial == 3)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                            
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            last = third;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 4)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            last = third;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 4:
                    if (trial == 5)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            last = fourth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 6)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            last = fourth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 5:
                    if (trial == 7)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            last = fifth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 8)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            last = fifth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 6:
                    if (trial == 9)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!sixthDone)
                        {
                            sixthDone = true;
                            last = sixth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 10)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!sixthDone)
                        {
                            sixthDone = true;
                            last = sixth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 7:
                    if (trial == 11)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!sixthDone)
                        {
                            sixthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!seventhDone)
                        {
                            seventhDone = true;
                            last = seventh;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 12)
                    {
                        if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!sixthDone)
                        {
                            sixthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!seventhDone)
                        {
                            seventhDone = true;
                            last = seventh;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
            }
        }
        else if (Backward)
        {
            switch (quantity)
            {
                case 2:
                    if (trial == 0)
                    {
                        if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 1)
                    {
                        if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 2)
                    {
                        if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 3:
                    if (trial == 3)
                    {
                        if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 4)
                    {
                        if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 4:
                    if (trial == 5)
                    {
                        if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 6)
                    {
                        if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 5:
                    if (trial == 7)
                    {
                        if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 8)
                    {
                        if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 6:
                    if (trial == 9)
                    {
                        if (!sixthDone)
                        {
                            sixthDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 10)
                    {
                        if (!sixthDone)
                        {
                            sixthDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 7:
                    if (trial == 11)
                    {
                        if (!seventhDone)
                        {
                            seventhDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!sixthDone)
                        {
                            sixthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 12)
                    {
                        if (!seventhDone)
                        {
                            seventhDone = true;
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!sixthDone)
                        {
                            sixthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fifthDone)
                        {
                            fifthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!fourthDone)
                        {
                            fourthDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!thirdDone)
                        {
                            thirdDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!secondDone)
                        {
                            secondDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                        else if (!firstDone)
                        {
                            firstDone = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                last = selected;// New Fix 
                                selected.GetComponent<Digits_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Digits_Selected>().Wrong();
                            }
                        }
                    }
                    break;
            }
        }
    }
    public void TargetForSelection()
    {
        if (loop < quantity)
        {
            randList = Random.Range(0,rand.Count);
            randList = rand[randList];
            rand.Remove(randList);
            if (randList == 1)
            {
                obj = Random.Range(0, numbers1.Count());
                num = numbers1[obj];
                numbers1.Remove(num);
            }
            else if (randList == 2)
            {
                obj = Random.Range(0, numbers2.Count());
                num = numbers2[obj];
                numbers2.Remove(num);
            }
            else if (randList == 3)
            {
                obj = Random.Range(0, numbers3.Count());
                num = numbers3[obj];
                numbers3.Remove(num);
            }
            if (rand.Count == 0)
            {
                rand = new List<int> {1,2,3};
            }
            if (numbers1.Count() < 2 || numbers2.Count() < 2 || numbers3.Count() < 2)
            {
                numbers1 = new List<int> { 1 , 2 , 3 };
                numbers2 = new List<int> { 4 , 5 , 6 };
                numbers3 = new List<int> { 7 , 8 , 9 }; 
            }
            switch (loop)
            {
                case 0:
                    switch (num)
                    {
                        case 1:
                            first = One;
                            break;
                        case 2:
                            first = Two;
                            break;
                        case 3:
                            first = Three;
                            break;
                        case 4:
                            first = Four;
                            break;
                        case 5:
                            first = Five;
                            break;
                        case 6:
                            first = Six;
                            break;
                        case 7:
                            first = Seven;
                            break;
                        case 8:
                            first = Eight;
                            break;
                        case 9:
                            first = Nine;
                            break;
                    }
                    break;
                case 1:
                    switch (num)
                    {
                        case 1:
                            second = One;
                            break;
                        case 2:
                            second = Two;
                            break;
                        case 3:
                            second = Three;
                            break;
                        case 4:
                            second = Four;
                            break;
                        case 5:
                            second = Five;
                            break;
                        case 6:
                            second = Six;
                            break;
                        case 7:
                            second = Seven;
                            break;
                        case 8:
                            second = Eight;
                            break;
                        case 9:
                            second = Nine;
                            break;
                    }
                    break;
                case 2:
                    switch (num)
                    {
                        case 1:
                            third = One;
                            break;
                        case 2:
                            third = Two;
                            break;
                        case 3:
                            third = Three;
                            break;
                        case 4:
                            third = Four;
                            break;
                        case 5:
                            third = Five;
                            break;
                        case 6:
                            third = Six;
                            break;
                        case 7:
                            third = Seven;
                            break;
                        case 8:
                            third = Eight;
                            break;
                        case 9:
                            third = Nine;
                            break;
                    }
                    break;
                case 3:
                    switch (num)
                    {
                        case 1:
                            fourth = One;
                            break;
                        case 2:
                            fourth = Two;
                            break;
                        case 3:
                            fourth = Three;
                            break;
                        case 4:
                            fourth = Four;
                            break;
                        case 5:
                            fourth = Five;
                            break;
                        case 6:
                            fourth = Six;
                            break;
                        case 7:
                            fourth = Seven;
                            break;
                        case 8:
                            fourth = Eight;
                            break;
                        case 9:
                            fourth = Nine;
                            break;
                    }
                    break;
                case 4:
                    switch (num)
                    {
                        case 1:
                            fifth = One;
                            break;
                        case 2:
                            fifth = Two;
                            break;
                        case 3:
                            fifth = Three;
                            break;
                        case 4:
                            fifth = Four;
                            break;
                        case 5:
                            fifth = Five;
                            break;
                        case 6:
                            fifth = Six;
                            break;
                        case 7:
                            fifth = Seven;
                            break;
                        case 8:
                            fifth = Eight;
                            break;
                        case 9:
                            fifth = Nine;
                            break;
                    }
                    break;
                case 5:
                    switch (num)
                    {
                        case 1:
                            sixth = One;
                            break;
                        case 2:
                            sixth = Two;
                            break;
                        case 3:
                            sixth = Three;
                            break;
                        case 4:
                            sixth = Four;
                            break;
                        case 5:
                            sixth = Five;
                            break;
                        case 6:
                            sixth = Six;
                            break;
                        case 7:
                            sixth = Seven;
                            break;
                        case 8:
                            sixth = Eight;
                            break;
                        case 9:
                            sixth = Nine;
                            break;
                    }
                    break;
                case 6:
                    switch (num)
                    {
                        case 1:
                            seventh = One;
                            break;
                        case 2:
                            seventh = Two;
                            break;
                        case 3:
                            seventh = Three;
                            break;
                        case 4:
                            seventh = Four;
                            break;
                        case 5:
                            seventh = Five;
                            break;
                        case 6:
                            seventh = Six;
                            break;
                        case 7:
                            seventh = Seven;
                            break;
                        case 8:
                            seventh = Eight;
                            break;
                        case 9:
                            seventh = Nine;
                            break;
                    }
                    break;
            }
            loop++;
        }
        if (loop == quantity)
        {
            loop = 0;
            rand = new List<int> { 1, 2, 3 };
            numbers1 = new List<int> { 1, 2, 3 };
            numbers2 = new List<int> { 4, 5, 6 };
            numbers3 = new List<int> { 7, 8, 9 };
            NewRound = false;
            Speaker.GetComponent<Digits_Presentation>().Clear();
            Speaker.GetComponent<Digits_Presentation>().presentation = true;

            // testing code to fix issues with skipping trials
            //
            last = eighth;
        }
    }
}
