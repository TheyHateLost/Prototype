using UnityEngine;

public class PuzzleWinChecker : MonoBehaviour
{
    public Renderer[] lights;
    public GameObject winText;

    void Update()
    {
        if (AllLightsOn())
        {
            winText.SetActive(true);
            GameEventsManager.tasksRemaining--;
        }
    }

    bool AllLightsOn()
    {
        foreach (Renderer r in lights)
        {
            if (r.material.color != Color.yellow)
                return false;
        }
        return true;
    }
}
