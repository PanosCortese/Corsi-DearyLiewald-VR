using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class CorsiBlockManager : MonoBehaviour
{
    public bool Forward;
    public bool Backward;
    public Corsi_ET_Manager ET;
    public Corsi_TimeManager TimeMan;
    public List<GameObject> corsiBlocks;
    public List<GameObject> corsiAppear;
    [Space(10)]
    public GameObject FrontMidMid;
    public GameObject FrontMidLeft;
    public GameObject FrontMidRight;
    [Space(10)]
    public GameObject FrontUpMid;
    public GameObject FrontUpLeft;
    public GameObject FrontUpRight;
    [Space(10)]
    public GameObject FrontDownMid;
    public GameObject FrontDownLeft;
    public GameObject FrontDownRight;
    [Space(10)]
    public GameObject MidMidMid;
    public GameObject MidMidLeft;
    public GameObject MidMidRight;
    [Space(10)]
    public GameObject MidUpMid;
    public GameObject MidUpLeft;
    public GameObject MidUpRight;
    [Space(10)]
    public GameObject MidDownMid;
    public GameObject MidDownLeft;
    public GameObject MidDownRight;
    [Space(10)]
    public GameObject BackMidMid;
    public GameObject BackMidLeft;
    public GameObject BackMidRight;
    [Space(10)]
    public GameObject BackUpMid;
    public GameObject BackUpLeft;
    public GameObject BackUpRight;
    [Space(10)]
    public GameObject BackDownMid;
    public GameObject BackDownLeft;
    public GameObject BackDownRight;
    [Space(10)]
    public GameObject One;
    [Space(10)]
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject fourth;
    public GameObject fifth;
    public GameObject sixth;
    public GameObject seventh;
    public GameObject last;
    [Tooltip("This will be always NULL")]
    public GameObject eighth;
    [Space(10)]
    public int obj;
    public int quantity;
    public bool newRound;
    [Space(10)]
    public int loop;
    public int loop2;
    public bool appear;
    public bool presentation;
    public GameObject cover;
    [Space(10)]
    public int trial;
    public int mistakes;
    public GameObject selected;
    [Space(10)]
    public int TotalScore;
    public int score;
    public int span;
    private bool delay;
    private float currentTime;
    private float delayTime = 0.5f;
    private bool once;
    public bool delayToDisable;
    public bool DONE;
    public AudioSource Intro;
    private bool introPlay;
    private bool introPlayed;
    [Space(10)]
    public GameObject Next;

#if UNITY_EDITOR
    [CustomEditor(typeof(CorsiBlockManager))]
    public class CorsiBlockManagerEditor : Editor
    {
        private bool swap;
        public override void OnInspectorGUI()
        {
            CorsiBlockManager corsiMan = (CorsiBlockManager)target;
            DrawDefaultInspector();
            if (!swap)
            {
                if (corsiMan.Forward)
                {
                    corsiMan.Backward = !corsiMan.Forward;
                    swap = true;
                }
                else if (corsiMan.Backward)
                {
                    corsiMan.Forward = !corsiMan.Backward;
                    swap = true;
                }
            }
            else if (swap)
            {
                if (corsiMan.Backward)
                {
                    corsiMan.Forward = !corsiMan.Backward;
                    swap = false;
                }
                else if (corsiMan.Forward)
                {
                    corsiMan.Backward = !corsiMan.Forward;
                    swap = false;
                }
            }

        }
    }
#endif
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        ET.gameObject.SetActive(true);
        TimeMan.gameObject.SetActive(true);
        corsiBlocks = new List<GameObject>
        {
            FrontDownLeft, FrontDownMid, FrontDownRight,
            FrontMidLeft, FrontMidMid, FrontMidRight,
            FrontUpLeft, FrontUpMid, FrontUpRight,
                MidDownLeft, MidDownMid, MidDownRight,
                MidMidLeft, MidMidMid, MidMidRight,
                MidUpLeft, MidUpMid, MidUpRight,
                    BackDownLeft, BackDownMid, BackDownRight,
                    BackMidLeft, BackMidMid, BackMidRight,
                    BackUpLeft, BackUpMid, BackUpRight
        };
        quantity = 2;
        loop = 0;
        loop2 = 0;
        trial = 0;
        mistakes = 0;
        score = 0;
        span = 0;
        TotalScore = 0;
        //newRound = true; // testing if it works fine
        introPlay = true;
        introPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (introPlay)
        {
            Intro.Play();
            introPlay = false;
            cover.SetActive(true);
        }
        else if (!Intro.isPlaying && !introPlayed)
        {
            cover.SetActive(false);
            newRound = true;
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
                delayToDisable = false;
                gameObject.SetActive(false);
            }
        }
        if (delay && once == false)
        {
            currentTime = Time.time;
            once = true;
        }

        if (delay && Time.time > currentTime + delayTime)
        {
            Delay();
            once = false;
        }
        if (newRound)
        {
            last = eighth;
            TimeMan.firstSelected = false;
            if (ET.readyToCalculate)
            {
                ET.Calculation();
            }
            TargetsToAppear();
            TargetForSelection();
        }
    }

    public void TargetsToAppear()
    {
        if (!cover.activeSelf)
        {
            cover.SetActive(true);
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
        }

        if (loop < 9 && !appear)
        {
            obj = Random.Range(0, corsiBlocks.Count());
            One = corsiBlocks[obj];
            One.SetActive(true);
            corsiAppear.Add(One);
            corsiBlocks.Remove(One);
            loop++;
        }

        if (loop == 9)
        {
            loop = 0;
            appear = true;
        }

    }

    public void TargetForSelection()
    {
        if (loop2 < quantity && appear)
        {
            obj = Random.Range(0, corsiAppear.Count());
            One = corsiAppear[obj];
            corsiAppear.Remove(One);
            switch (loop2)
            {
                case 0:
                    first = One;
                    break;
                case 1:
                    second = One;
                    break;
                case 2:
                    third = One;
                    break;
                case 3:
                    fourth = One;
                    break;
                case 4:
                    fifth = One;
                    break;
                case 5:
                    sixth = One;
                    break;
                case 6:
                    seventh = One;
                    break;
            }
            loop2++;
        }
        if (loop2 == quantity && appear)
        {
            loop2 = 0;
            appear = false;
            corsiBlocks = new List<GameObject>
            {            
                FrontDownLeft, FrontDownMid, FrontDownRight,
                FrontMidLeft, FrontMidMid, FrontMidRight,
                FrontUpLeft, FrontUpMid, FrontUpRight,
                    MidDownLeft, MidDownMid, MidDownRight,
                    MidMidLeft, MidMidMid, MidMidRight,
                    MidUpLeft, MidUpMid, MidUpRight,
                        BackDownLeft, BackDownMid, BackDownRight,
                        BackMidLeft, BackMidMid, BackMidRight,
                        BackUpLeft, BackUpMid, BackUpRight
            };
            ET.Assignment();
            corsiAppear.Clear();
            newRound = false;
            cover.SetActive(false);
            first.GetComponent<CorsiPresentation>().presentation = true;
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
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            last = second;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 1)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            last = second;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 2)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            last = second;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 3:
                    if (trial == 3)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            last = third;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 4)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            last = third;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 4:
                    if (trial == 5)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            last = fourth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 6)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            last = fourth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 5:
                    if (trial == 7)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            last = fifth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 8)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            last = fifth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 6:
                    if (trial == 9)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            last = sixth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 10)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            last = sixth;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 7:
                    if (trial == 11)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!seventh.GetComponent<Corsi_Selected>().done)
                        {
                            last = seventh;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 12)
                    {
                        if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!seventh.GetComponent<Corsi_Selected>().done)
                        {
                            last = seventh;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
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
                        if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }

                    }
                    else if (trial == 1)
                    {
                        if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 2)
                    {
                        if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 3:
                    if (trial == 3)
                    {
                        if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 4)
                    {
                        if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 4:
                    if (trial == 5)
                    {
                        if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 6)
                    {
                        if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 5:
                    if (trial == 7)
                    {
                        if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 8)
                    {
                        if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 6:
                    if (trial == 9)
                    {
                        if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 10)
                    {
                        if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
                case 7:
                    if (trial == 11)
                    {
                        if (!seventh.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    else if (trial == 12)
                    {
                        if (!seventh.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.firstSelected = true;
                            TimeMan.SelectioTimeCalc();
                            if (selected == seventh)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!sixth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == sixth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fifth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fifth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!fourth.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == fourth)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!third.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == third)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!second.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == second)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                        else if (!first.GetComponent<Corsi_Selected>().done)
                        {
                            TimeMan.SelectioTimeCalc();
                            if (selected == first)
                            {
                                selected.GetComponent<Corsi_Selected>().Correct();
                            }
                            else
                            {
                                selected.GetComponent<Corsi_Selected>().Wrong();
                            }
                        }
                    }
                    break;
            }
        }

    }
    public void NewRound()
    {
        delay = true;
        corsiBlocks = new List<GameObject>
        {
            FrontDownLeft, FrontDownMid, FrontDownRight,
            FrontMidLeft, FrontMidMid, FrontMidRight,
            FrontUpLeft, FrontUpMid, FrontUpRight,
                MidDownLeft, MidDownMid, MidDownRight,
                MidMidLeft, MidMidMid, MidMidRight,
                MidUpLeft, MidUpMid, MidUpRight,
                    BackDownLeft, BackDownMid, BackDownRight,
                    BackMidLeft, BackMidMid, BackMidRight,
                    BackUpLeft, BackUpMid, BackUpRight
        };
    }

    public void Delay()
    {
        FrontDownLeft.SetActive(false);
        FrontDownMid.SetActive(false);
        FrontDownRight.SetActive(false);

        FrontMidLeft.SetActive(false);
        FrontMidMid.SetActive(false);
        FrontMidRight.SetActive(false);

        FrontUpLeft.SetActive(false);
        FrontUpMid.SetActive(false);
        FrontUpRight.SetActive(false);

        MidDownLeft.SetActive(false);
        MidDownMid.SetActive(false);
        MidDownRight.SetActive(false);

        MidMidLeft.SetActive(false);
        MidMidMid.SetActive(false);
        MidMidRight.SetActive(false);

        MidUpLeft.SetActive(false);
        MidUpMid.SetActive(false);
        MidUpRight.SetActive(false);

        BackDownLeft.SetActive(false);
        BackDownMid.SetActive(false);
        BackDownRight.SetActive(false);

        BackMidLeft.SetActive(false);
        BackMidMid.SetActive(false);
        BackMidRight.SetActive(false);

        BackUpLeft.SetActive(false);
        BackUpMid.SetActive(false);
        BackUpRight.SetActive(false);

        newRound = true;
        delay = false;
    }
    private void OnDisable()
    {
        ET.gameObject.SetActive(false); ;
        TimeMan.gameObject.SetActive(false);
        TotalScore = score + span;
        FrontDownLeft.SetActive(false);
        FrontDownMid.SetActive(false);
        FrontDownRight.SetActive(false);

        FrontMidLeft.SetActive(false);
        FrontMidMid.SetActive(false);
        FrontMidRight.SetActive(false);

        FrontUpLeft.SetActive(false);
        FrontUpMid.SetActive(false);
        FrontUpRight.SetActive(false);

        MidDownLeft.SetActive(false);
        MidDownMid.SetActive(false);
        MidDownRight.SetActive(false);

        MidMidLeft.SetActive(false);
        MidMidMid.SetActive(false);
        MidMidRight.SetActive(false);

        MidUpLeft.SetActive(false);
        MidUpMid.SetActive(false);
        MidUpRight.SetActive(false);

        BackDownLeft.SetActive(false);
        BackDownMid.SetActive(false);
        BackDownRight.SetActive(false);

        BackMidLeft.SetActive(false);
        BackMidMid.SetActive(false);
        BackMidRight.SetActive(false);

        BackUpLeft.SetActive(false);
        BackUpMid.SetActive(false);
        BackUpRight.SetActive(false);

        DONE = true;

        quantity = 2;
        loop = 0;
        loop2 = 0;
        trial = 0;
        if (Next != null)
        {
            Next.SetActive(true);
        }
    }
}
