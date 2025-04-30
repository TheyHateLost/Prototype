using UnityEngine;

public class Fuse_Lever : MonoBehaviour, IInteractable
{
    public static bool Lever_Active = false;
    bool LeverUsed = false;

    // Update is called once per frame
    public void Interact()
    {
        if (Lever_Active == true && LeverUsed == false)
        {
            GameEventsManager.tasksRemaining--;
            LeverUsed = true;
        }
    }
}
