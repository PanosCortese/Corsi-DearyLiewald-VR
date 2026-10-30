using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FixationET_Corsi : MonoBehaviour
{
    public float fixationCount;
    public float fixationDuration;
    public float AvgFixationDuration;
    public List<float> fixationTime;
    [Space(10)]
    public CorsiPresentation present;
    public bool presented;
    private float TimeEnter;
    public bool focused;
    public Corsi_TimeManager corsiTimeMan;
    // Start is called before the first frame update
    void Start()
    {
        fixationCount = 0;
        fixationDuration = 0;
        present = GetComponent<CorsiPresentation>();
    }

    private void OnEnable()
    {
        fixationCount = 0;
        fixationDuration = 0;
        present = GetComponent<CorsiPresentation>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ET_Pointer"))
        {
            if (present.presenting)
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
            if (present.presenting)
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
