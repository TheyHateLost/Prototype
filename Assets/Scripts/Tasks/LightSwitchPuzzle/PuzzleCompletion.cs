using UnityEngine;

//Script is for actiavting the puzzle UI - goes on physical object that will be used for bringing up the UI
public class PuzzleCompletion : MonoBehaviour
{
    public static bool puzzleComplete = false;

    void Update()
    {
        if (puzzleComplete)
        {
            puzzleComplete = false;
            GameEventsManager.tasksRemaining--;
        }
    }
}

