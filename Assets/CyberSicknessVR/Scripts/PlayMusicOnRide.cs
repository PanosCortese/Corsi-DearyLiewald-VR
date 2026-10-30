using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnRide : MonoBehaviour
{
    public AudioSource audi;
    public Animator anim;
    AnimatorClipInfo[] animClipInfo;
    public string currentClipName;
    public string begin;
    public string finish;
    public string finish2;
    public bool once;
    // Start is called before the first frame update
    void Start()
    {
        audi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        animClipInfo = anim.GetCurrentAnimatorClipInfo(0);
        currentClipName = animClipInfo[0].clip.name;
        if (currentClipName == begin)
        {
            if (!once && audi.clip != null)
            {
                audi.Play();
                once = true;
            }
        }
        else if (currentClipName == finish || currentClipName == finish2)
        {
            if (audi.isPlaying && once && audi.clip != null)
            {
                audi.Stop();
                once = false;
            }
        }
    }
}
