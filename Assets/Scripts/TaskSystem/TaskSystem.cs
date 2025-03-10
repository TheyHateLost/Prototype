using UnityEngine;
public class TaskSystem : MonoBehaviour, IInteractable
{
    public GameObject keypadUI;

    public void Interact()
    {
        keypadUI.SetActive(true);
    }
}
