using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorsiRemoveFromList : MonoBehaviour
{
    public CorsiBlockManager corsiMan;
    [Space(10)]
    public GameObject neighbor1;
    public GameObject neighbor2;
    public GameObject neighbor3;
    public GameObject neighbor4;
    public GameObject neighbor5;
    public GameObject neighbor6;

    // Start is called before the first frame update
    void Start()
    {
        corsiMan = GetComponentInParent<CorsiBlockManager>();
    }

    private void OnEnable()
    {
        if (neighbor1 != null)
        {
            if (corsiMan.corsiBlocks.Contains(neighbor1))
            {
                corsiMan.corsiBlocks.Remove(neighbor1);
            }
        }
        if (neighbor2 != null)
        {
            if (corsiMan.corsiBlocks.Contains(neighbor2))
            {
                corsiMan.corsiBlocks.Remove(neighbor2);
            }
        }
        if (neighbor3 != null)
        {
            if (corsiMan.corsiBlocks.Contains(neighbor3))
            {
                corsiMan.corsiBlocks.Remove(neighbor3);
            }
        }
        if (neighbor4 != null)
        {
            if (corsiMan.corsiBlocks.Contains(neighbor4))
            {
                corsiMan.corsiBlocks.Remove(neighbor4);
            }
        }
        if (neighbor5 != null)
        {
            if (corsiMan.corsiBlocks.Contains(neighbor5))
            {
                corsiMan.corsiBlocks.Remove(neighbor5);
            }
        }
        if (neighbor6 != null)
        {
            if (corsiMan.corsiBlocks.Contains(neighbor6))
            {
                corsiMan.corsiBlocks.Remove(neighbor6);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
