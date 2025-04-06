using TMPro;
using UnityEngine;

// Grabs the Interact method from other scripts using IInteractable
interface IInteractable
{
    public void Interact();
}
// Main interaction script
public class Interator : MonoBehaviour
{
    public Transform InteracterSource;
    public float InteractRange = 2f; // Default value for safety
    public GameObject interactText;
    public TextMeshProUGUI interactPrompt;

    void Update()
    {
        //Raycast from mouse position(center screen)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractRange))
        {
            //if looking at something with IInteractable class on it...
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                //show prompt to interact with item
                if (hit.collider.gameObject.CompareTag("CanGrab"))
                {
                    interactPrompt.text = "Press E to Pick Up";
                    interactText.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //use the method from the script of the item you are looking at
                        interactObj.Interact();
                    }
                }

                //show prompt to interact with item
                if (hit.collider.gameObject.CompareTag("CanInteract"))
                {
                    interactPrompt.text = "Press E to Interact";
                    interactText.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //use the method from the script of the item you are looking at
                        interactObj.Interact();
                    }
                }
            }
        }
        //if not looking at something with IInteractable class then turn off prompt
        else
        {
            interactText.SetActive(false);
        }
    }
}
