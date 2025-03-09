using UnityEngine;
public class TaskSystem : MonoBehaviour, IInteractable
{
    public GameObject keypad;

    public void Interact()
    {
        keypad.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            keypad.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
