using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FixationET_Digit : MonoBehaviour
{
    public EyeTracking ET;
    public float fixationCount;
    public float fixationDuration;
    private float TimeEnter;
    public List<float> fixationTime;
    public float AvgFixationDuration;
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
            if (ET.digit)
            {
                fixationCount++;
                TimeEnter = Time.time;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ET_Pointer"))
        {
            if (ET.digit)
            {
                fixationDuration = Time.time - TimeEnter;
                fixationTime.Add(fixationDuration);
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
    }

    public void Clear()
    {
        fixationTime.Clear();
        fixationCount = 0;
        fixationDuration = 0;
    }
}
