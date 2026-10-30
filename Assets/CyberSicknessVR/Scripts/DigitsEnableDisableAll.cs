using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitsEnableDisableAll : MonoBehaviour
{
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;

    private void OnEnable()
    {
        one.SetActive(true);
        two.SetActive(true);
        three.SetActive(true);
        four.SetActive(true);
        five.SetActive(true);
        six.SetActive(true);
        seven.SetActive(true);
        eight.SetActive(true);
        nine.SetActive(true);
    }

    private void OnDisable()
    {
        one.SetActive(false);
        two.SetActive(false);
        three.SetActive(false);
        four.SetActive(false);
        five.SetActive(false);
        six.SetActive(false);
        seven.SetActive(false);
        eight.SetActive(false);
        nine.SetActive(false);
    }
}
