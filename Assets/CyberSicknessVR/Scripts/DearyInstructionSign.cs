using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DearyInstructionSign : MonoBehaviour
{
    public AudioSource MainSource;
    public bool playing;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if (MainSource.isPlaying)
        {
            playing = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (MainSource.isPlaying && !playing)
        {
            playing = true;
        }
        if (playing && !MainSource.isPlaying)
        {
            playing = false;
            gameObject.SetActive(false);
        }
    }
}
