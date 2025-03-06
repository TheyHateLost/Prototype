using UnityEngine;
public class TaskSystem : MonoBehaviour, IInteractable
{
    public GameObject keypad;
    public void Interact()
    {
        keypad.SetActive(true);
    }
}
