using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corsi_Colliders_Manager : MonoBehaviour
{
    public CorsiBlockManager corsiMan;
    [Space(10)]
    public Collider LeftHand;
    public Collider RightHand;
    [Space(10)]
    public AudioSource Notification;
    public AudioSource Go;
    public bool presentation;
    public bool presentFinishing;
    private bool once;
    private bool once2;

    // Update is called once per frame
    void Update()
    {
        if (Notification.isPlaying && !presentation)
        {
            presentation = true;
            once = true;
        }
        if (presentation && once)
        {
            LeftHand.enabled = false;
            RightHand.enabled = false;
            once = false;
        }
        if (presentation && Go.isPlaying)
        {
            presentation = false;
            presentFinishing = true;
            once2 = true;
        }
        if (presentFinishing)
        {
            if (!Go.isPlaying)
            {
                presentFinishing = false;
            }
        }
        if (!presentation && !presentFinishing && once2)
        {
            LeftHand.enabled = true;
            RightHand.enabled = true;
            once2 = false;
        }
        
    }
}
