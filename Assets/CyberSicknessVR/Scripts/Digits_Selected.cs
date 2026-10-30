using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digits_Selected : MonoBehaviour
{
    public Material current;
    public Material select;
    public Material correct;
    public Material wrong;
    [Space(10)]
    public DigitRecallManager digiMan;
    public Digits_TimeManager digiManTime;
    [Space(10)]
    public float selectTime;
    public float delay;

    public Collider col;
    [Space(10)]
    public GameObject Other1;
    public GameObject Other2;
    public GameObject Other3;
    public GameObject Other4;
    public GameObject Other5;
    public GameObject Other6;        
    public GameObject Other7;
    public GameObject Other8;
    [Space(10)]
    public AudioSource audi;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    private bool selectedInitiated; // New Code
    private void OnEnable()
    {
        if (current == null)
        {
            current = GetComponent<MeshRenderer>().material;
        }
        else if (current != null)
        {
            GetComponent<MeshRenderer>().material = current;
        }
        col = GetComponent<Collider>();
        col.enabled = true;
        audi = GetComponent<AudioSource>();
        selectedInitiated = false; // New Code
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(Time.frameCount + " enter" + other.gameObject.name + " enter on " + gameObject.name);
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            //print(Time.frameCount + " enter if" + other.gameObject.name);
            if (!selectedInitiated)
            {
                selectTime = Time.time;
                selectedInitiated = true; // New Code
                GetComponent<MeshRenderer>().material = select;
            }
            //print("[" + Time.time + "][" + Time.frameCount + "] " + gameObject.name + " selected=" + selectedInitiated + " Selected Time = " + selectTime + " Delay = " + delay + " now = " + (selectTime + delay));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //print(Time.frameCount + " stay" + other.gameObject.name);
        if (other.gameObject.CompareTag("IndexFinger"))
        {
            //print(Time.frameCount + " stay if" + other.gameObject.name);
            //print("[" + Time.time + "][" + Time.frameCount + "] " + gameObject.name + " selected=" + selectedInitiated + " Selected Time = " + selectTime + " Delay = " + delay + " now = " + (selectTime+delay));
            if ((Time.time > (selectTime + delay)) && selectedInitiated) // New Code = && !selectedInitiated
            {
                //print(Time.frameCount + " stay if time" + other.gameObject.name);
                digiMan.selected = gameObject;
                selectedInitiated = false; // New Code 
                digiMan.Selected(); 
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

    public void Correct()
    {
        ChangeOthers();
        col.enabled = false;
        GetComponent<MeshRenderer>().material = correct;
        audi.clip = correctClip;
        audi.Play();
        if (digiMan.Forward)
        {
            if (digiMan.last != null)
            {
                if (gameObject == digiMan.last && digiMan.trial != 12)
                {
                    digiMan.mistakes = 0;
                    digiMan.score += 1;
                    digiMan.span = digiMan.quantity;
                    if (digiMan.trial != 0 && digiMan.trial % 2 == 0)
                    {
                        digiMan.quantity += 1;
                    }
                    digiMan.trial += 1;
                    digiManTime.FinishTrial();
                    digiMan.delay = true;
                }
                else if (gameObject == digiMan.last && digiMan.trial == 12)
                {
                    digiManTime.FinishTrial();
                    digiMan.mistakes = 0;
                    digiMan.score += 1;
                    digiMan.span = digiMan.quantity;
                    digiMan.delayToDisable = true;
                }
            }

        }
        else if (digiMan.Backward)
        {
            if (digiMan.last != null)
            {
                if (gameObject == digiMan.last && digiMan.trial != 12) // digi.first replaced by digi.last
                {
                    digiMan.mistakes = 0;
                    digiMan.score += 1;
                    digiMan.span = digiMan.quantity;
                    if (digiMan.trial != 0 && digiMan.trial % 2 == 0)
                    {
                        digiMan.quantity += 1;
                    }
                    digiMan.trial += 1;
                    digiManTime.FinishTrial();
                    digiMan.delay = true;
                }
                else if (gameObject == digiMan.last && digiMan.trial == 12)// digi.first replaced by digi.last
                {
                    digiManTime.FinishTrial();
                    digiMan.mistakes = 0;
                    digiMan.score += 1;
                    digiMan.span = digiMan.quantity;
                    digiMan.delayToDisable = true;
                }
            }

        }

    }

    public void Wrong()
    {
        ChangeOthers();
        digiManTime.FinishTrial();
        col.enabled = false;
        GetComponent<MeshRenderer>().material = wrong;
        audi.clip = wrongClip;
        audi.Play();
        digiMan.mistakes += 1;
        if (digiMan.mistakes < 2)
        {
            if (digiMan.trial == 12)
            {
                digiMan.delayToDisable = true;
            }
            else if (digiMan.trial < 12)
            {
                if (digiMan.trial != 0 && digiMan.trial % 2 == 0)
                {
                    digiMan.quantity += 1;
                    digiMan.mistakes = 0;
                }
                digiMan.trial += 1;
                digiMan.delay = true;
            }

        }
        else
        {
            digiMan.delayToDisable = true ;
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

    private void ChangeOthers()
    {
        Other1.GetComponent<MeshRenderer>().material = current;
        Other2.GetComponent<MeshRenderer>().material = current;
        Other3.GetComponent<MeshRenderer>().material = current;
        Other4.GetComponent<MeshRenderer>().material = current;
        Other5.GetComponent<MeshRenderer>().material = current;
        Other6.GetComponent<MeshRenderer>().material = current;
        Other7.GetComponent<MeshRenderer>().material = current;
        Other8.GetComponent<MeshRenderer>().material = current;

        Other1.GetComponent<Collider>().enabled = true;
        Other2.GetComponent<Collider>().enabled = true;
        Other3.GetComponent<Collider>().enabled = true;
        Other4.GetComponent<Collider>().enabled = true;
        Other5.GetComponent<Collider>().enabled = true;
        Other6.GetComponent<Collider>().enabled = true;
        Other7.GetComponent<Collider>().enabled = true;
        Other8.GetComponent<Collider>().enabled = true;
    }

}
