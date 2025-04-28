using System;
using UnityEngine;

public class SwitchPuzzle : MonoBehaviour
{
    public Renderer[] lights;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FlipSwitch(int switchNumber)
    {
        if (switchNumber == 0)
        {
            ToggleLight(0);
            ToggleLight(1);
        }
        else if (switchNumber == 1)
        {
            ToggleLight(1);
            ToggleLight(2);
        }
        else if (switchNumber == 2)
        {
            ToggleLight(2);
            ToggleLight(3);
        }
        else if (switchNumber == 3)
        {
            ToggleLight(0);
            ToggleLight(3);
        }
    }

    void ToggleLight(int index)
    {
        Color currentColor = lights[index].material.color;

        if (currentColor == Color.grey)
        {
            lights[index].material.color = Color.yellow;
        }
        else
            lights[index].material.color = Color.grey;
    }

    void Update()
    {
        CheckForCompletion();
    }

    void CheckForCompletion()
    {
        int index = 4;
        foreach (Renderer renderer in lights)

        {
            if (lights[index].material.color == Color.yellow)
            {
                Debug.Log("Complete");
            }
        }
    }
}