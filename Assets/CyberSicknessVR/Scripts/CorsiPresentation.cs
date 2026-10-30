using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorsiPresentation : MonoBehaviour
{
    public EyeTracking ET;
    public Material current;
    public Material present;
    public AudioSource audi;
    public AudioClip bell;
    public GameObject next;
    public CorsiBlockManager corsiMan;
    public AudioSource GO;
    public AudioSource Notify;
    private float presentTime;
    public float delay = 0.5f;
    public bool presentation;
    public bool presenting;
    private bool playing;
    private bool delaying;
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
            if (gameObject == corsiMan.first)
            {
                next = corsiMan.second;
                audi.clip = bell;
                Notify.Play();
                playing = true;
                presentation = false;
            }
            else if (gameObject == corsiMan.second)
            {
                next = corsiMan.third;
                audi.clip = bell;
                presenting = true;
            }
            else if (gameObject == corsiMan.third)
            {
                next = corsiMan.fourth;
                audi.clip = bell;
                presenting = true;
            }
            else if (gameObject == corsiMan.fourth)
            {
                next = corsiMan.fifth;
                audi.clip = bell;
                presenting = true;
            }
            else if (gameObject == corsiMan.fifth)
            {
                next = corsiMan.sixth;
                audi.clip = bell;
                presenting = true;
            }
            else if (gameObject == corsiMan.sixth)
            {
                next = corsiMan.seventh;
                audi.clip = bell;
                presenting = true;
            }
            else if (gameObject == corsiMan.seventh)
            {
                next = corsiMan.eighth;
                audi.clip = bell;
                presenting = true;
            }
        }
        if (playing)
        {
            ET.corsi = true;
            if (Notify.isPlaying)
            {
                return;
            }
            else
            {
                presenting = true;
                playing = false;
                presentation = true;
            }
        }
        if (presenting)
        {
            if (presentation)
            {
                audi.Play();
                GetComponent<MeshRenderer>().material = present;
                presentation = false;
            }
            if (audi.isPlaying)
            {
                GetComponent<MeshRenderer>().material = present;
            }
            else if (!audi.isPlaying)
            {
                GetComponent<MeshRenderer>().material = current;
                presenting = false;
                presentTime = Time.time;
                delaying = true;
            }
        }
        if (delaying)
        {
            if (Time.time <= presentTime + delay)
            {
                return;
            }
            else
            {
                if (next == null)
                {
                    ET.corsi = false;
                    GO.Play();
                    delaying = false;
                }
                else
                {
                    next.GetComponent<CorsiPresentation>().presentation = true;
                    delaying = false;
                }
            }
        }
    }
}
