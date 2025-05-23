using UnityEngine;

public class Fuse_Lever : MonoBehaviour, IInteractable
{
    public bool Lever_Active = false;
    [SerializeField] GameEventsManager gameEventsManager;
    bool LeverUsed = false;

    // Update is called once per frame
    public void Interact()
    {
        if (Lever_Active == true && LeverUsed == false)
        {
            gameEventsManager.tasksRemaining--;
            LeverUsed = true;
            gameObject.tag = "Used";
            gameObject.layer = 0;
        }
    }
}
