using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float currentOxygen;
    public float startingOxygen = 100f;
    public static int OxygenDecreaseRate = 8; // Oxygen decrease rate per second

    [SerializeField] Text[] countdownText;
    void Start()
    {
        currentOxygen = startingOxygen;
        for (int i = 0; i < countdownText.Length; i++)
        {
            if (countdownText[i] != null)
            {
                countdownText[i].text = currentOxygen.ToString("0") + " %";
            }
        }
    }
    void Update()
    {
        currentOxygen -= Time.deltaTime / OxygenDecreaseRate;
        // Ensure currentOxygen does not go below 0 and does not exceed startingOxygen
        currentOxygen = Mathf.Clamp(currentOxygen, 0, startingOxygen);

        // Update the countdown text
        for (int i = 0; i < countdownText.Length; i++)
        {
            if (countdownText[i] != null)
            countdownText[i].text = currentOxygen.ToString("0") + " %";
        }

        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);  
        }
    }
}