using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float currentOxygen;
    public float startingOxygen = 100f;
    public static int OxygenDecreaseRate = 10; // Oxygen decrease rate per second

    [SerializeField] Text countdownText;
    void Start()
    {
        currentOxygen = startingOxygen;
        countdownText.text = currentOxygen.ToString("0") + " %";
    }
    void Update()
    {
        currentOxygen -= Time.deltaTime / OxygenDecreaseRate;
        countdownText.text = currentOxygen.ToString("0") + " %";

        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);  
        }
    }
}