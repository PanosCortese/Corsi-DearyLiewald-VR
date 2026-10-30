using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioFornTimes : MonoBehaviour
{
    public AudioSource audi;
    public int played = 0;
    private bool once;
    public int threshold;
    // Start is called before the first frame update
    void Start()
    {
        audi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audi.isPlaying && !once)
        {
            
            once = true;
        }
        else if (!audi.isPlaying && once)
        {
            played += 1;
            once = false;
        }

        if (played >= threshold)
        {
            audi.gameObject.SetActive(false);
        }

    }
}
