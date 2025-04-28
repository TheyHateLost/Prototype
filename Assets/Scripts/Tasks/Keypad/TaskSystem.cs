using UnityEngine;
public class TaskSystem : MonoBehaviour, IInteractable
{
    public GameObject keypadUI;

    //Attach to Physcial Task Objects, make interact scrip aware of thse objects
    //Brings up the assigned UI
    public void Interact()
    {
        if (KeypadTask.isResetting == false)
        keypadUI.SetActive(true);
    }
}
