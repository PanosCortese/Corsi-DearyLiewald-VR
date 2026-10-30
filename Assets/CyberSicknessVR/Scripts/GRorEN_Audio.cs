using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRorEN_Audio : MonoBehaviour
{
    public bool English;
    public bool Greek;
    public AudioClip GR;
    public AudioClip EN;
    public AudioSource audi;
    // Start is called before the first frame update
    void Start()
    {
        if (English)
        {
            Greek = !English;
            audi.clip = EN;
        }
        else if (Greek)
        {
            English = !Greek;
            audi.clip = GR;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (English)
        {
            Greek = !English;
            audi.clip = EN;
        }
        else if (Greek)
        {
            English = !Greek;
            audi.clip = GR;
        }
    }
}
