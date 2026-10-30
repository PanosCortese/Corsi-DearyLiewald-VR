using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GreekOREnglish : MonoBehaviour
{
    public bool English;
    public bool Greek;
    public string GR;
    public string EN;
    public TextMeshProUGUI display;
    // Start is called before the first frame update
    void Start()
    {
        if (English)
        {
            Greek = !English;
            display.text = EN;
        }
        else if (Greek)
        {
            English = !Greek;
            display.text = GR;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (English)
        {
            Greek = !English;
            display.text = EN;
        }
        else if (Greek)
        {
            English = !Greek;
            display.text = GR;
        }
        
    }
}
