using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FixationET : MonoBehaviour
{
    public float fixationCount;
    public float fixationDuration;
    private float TimeEnter;
    public List<float> fixationTime;
    public float AvgFixationDuration;
    public IsFocusedET focus;
    // Start is called before the first frame update
    void Start()
    {
        fixationCount = 0;
        fixationDuration = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ET_Pointer"))
        {
            fixationCount++;
            TimeEnter = Time.time;
            if (focus != null)
            {
                focus.Focused();
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ET_Pointer"))
        {
            fixationDuration = Time.time - TimeEnter;
            fixationTime.Add(fixationDuration);
            if (focus != null)
            {
                focus.UnFocused();
            }
        }
    }

    public void Calculation()
    {
        if (fixationTime.Count > 1)
        {
            AvgFixationDuration = fixationTime.Average();
            fixationDuration = fixationTime.Sum();
        }
        else if (fixationTime.Count == 1)
        {
            AvgFixationDuration = fixationDuration;
        }
        else
        {
            fixationDuration = 0;
            fixationCount = 0;
            AvgFixationDuration = 0;
        }
    }

    public void Clear()
    {
        fixationTime.Clear();
        fixationCount = 0;
        fixationDuration = 0;
    }
}
