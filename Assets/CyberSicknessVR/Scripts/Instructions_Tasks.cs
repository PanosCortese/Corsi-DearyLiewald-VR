using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Instructions_Tasks : MonoBehaviour
{
    public GameObject CSQ;
    public GameObject Deary;
    public GameObject DigitF;
    public GameObject Digit;
    public GameObject CorsiF;
    public GameObject Corsi;
    [Space(10)]
    public GameObject LeftNotes;
    public GameObject RightNotes;
    [Space(10)]
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;
    public TextMeshProUGUI text7;
    public TextMeshProUGUI text8;
    public TextMeshProUGUI text9;
    public TextMeshProUGUI text10;
    [Space(10)]
    public string quest1;
    public string quest2;
    public string quest3;
    public string quest4;
    public string quest5;
    [Space(10)]
    public string digitF1;
    public string digitF2;
    public string digitF3;
    public string digitF4;
    public string digitF5;
    [Space(10)]
    public string digit1;
    public string digit2;
    public string digit3;
    public string digit4;
    public string digit5;
    [Space(10)]
    public string corsiF1;
    public string corsiF2;
    public string corsiF3;
    public string corsiF4;
    public string corsiF5;
    [Space(10)]
    public string corsi1;
    public string corsi2;
    public string corsi3;
    public string corsi4;
    public string corsi5;
    [Space(10)]
    public string deary1;
    public string deary2;
    public string deary3;
    public string deary4;
    public string deary5;

    private void OnEnable()
    {
        if (CSQ != null)
        {
            if (CSQ.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = quest1;
                text2.text = quest2;
                text3.text = quest3;
                text4.text = quest4;
                text5.text = quest5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (DigitF != null)
        {
            if (DigitF.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = digitF1;
                text2.text = digitF2;
                text3.text = digitF3;
                text4.text = digitF4;
                text5.text = digitF5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (Digit != null)
        {
            if (Digit.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = digit1;
                text2.text = digit2;
                text3.text = digit3;
                text4.text = digit4;
                text5.text = digit5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }

        if (CorsiF != null)
        {
            if (CorsiF.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = corsiF1;
                text2.text = corsiF2;
                text3.text = corsiF3;
                text4.text = corsiF4;
                text5.text = corsiF5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (Corsi != null)
        {
            if (Corsi.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = corsi1;
                text2.text = corsi2;
                text3.text = corsi3;
                text4.text = corsi4;
                text5.text = corsi5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
            }
        }
        if (Deary != null)
        {
            if (Deary.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = deary1;
                text2.text = deary2;
                text3.text = deary3;
                text4.text = deary4;
                text5.text = deary5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (!CSQ.activeSelf && !Deary.activeSelf) //Change to else for other paradigms
        {
            LeftNotes.SetActive(false);
            RightNotes.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(CSQ != null)
        {
            if (CSQ.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = quest1;
                text2.text = quest2;
                text3.text = quest3;
                text4.text = quest4;
                text5.text = quest5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (DigitF != null)
        {
            if (DigitF.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = digitF1;
                text2.text = digitF2;
                text3.text = digitF3;
                text4.text = digitF4;
                text5.text = digitF5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (Digit != null)
        {
            if (Digit.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = digit1;
                text2.text = digit2;
                text3.text = digit3;
                text4.text = digit4;
                text5.text = digit5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }

        if (CorsiF != null)
        {
            if (CorsiF.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = corsiF1;
                text2.text = corsiF2;
                text3.text = corsiF3;
                text4.text = corsiF4;
                text5.text = corsiF5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (Corsi != null)
        {
            if (Corsi.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = corsi1;
                text2.text = corsi2;
                text3.text = corsi3;
                text4.text = corsi4;
                text5.text = corsi5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
            }
        }
        if (Deary != null)
        {
            if (Deary.activeSelf)
            {
                LeftNotes.SetActive(true);
                RightNotes.SetActive(true);
                text1.text = deary1;
                text2.text = deary2;
                text3.text = deary3;
                text4.text = deary4;
                text5.text = deary5;
                text6.text = text1.text;
                text7.text = text2.text;
                text8.text = text3.text;
                text9.text = text4.text;
                text10.text = text5.text;
            }
        }
        if (!CSQ.activeSelf && !Deary.activeSelf) //Change to else for other paradigms
        {
            LeftNotes.SetActive(false);
            RightNotes.SetActive(false);
        }
    }
}
