using System;
using UnityEngine;

public class SwitchPuzzle : MonoBehaviour
{
    public Renderer[] PuzzleLight_Object;
    bool taskComplete = false;
    [SerializeField] GameObject PuzzleTrigger_Object;

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
        Color currentColor = PuzzleLight_Object[index].material.color;

        if (currentColor == Color.grey)
        {
            PuzzleLight_Object[index].material.color = Color.yellow;
        }
        else
            PuzzleLight_Object[index].material.color = Color.grey;
    }

    void Update()
    {
        CheckForCompletion();
    }

    void CheckForCompletion()
    {
        foreach (Renderer renderer in PuzzleLight_Object)
        {
            if ((PuzzleLight_Object[0].material.color == Color.yellow) && (PuzzleLight_Object[1].material.color == Color.yellow) && (PuzzleLight_Object[2].material.color == Color.yellow) && (PuzzleLight_Object[3].material.color == Color.yellow))
            {
                if (taskComplete == false)
                {
                    taskComplete = true;
                    GameEventsManager.tasksRemaining--;
                    PuzzleTrigger_Object.tag = "Used";
                    PuzzleTrigger_Object.layer = 0;
                    gameObject.SetActive(false);
                }
            }
        }
    }
} 