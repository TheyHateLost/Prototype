using TMPro;
using UnityEngine;

// Grabs the Interact method from other scripts using IInteractable
interface IInteractable
{
    public void Interact();
    //public void InteractText();
}
// Main interaction script
public class Interator : MonoBehaviour
{
    [SerializeField] Transform playerCamTransform;
    [SerializeField] LayerMask interactableLayer;
    public float InteractRange = 2f; // Default value for safety
    public GameObject interactText;
    public TextMeshProUGUI interactPrompt;
    public static bool CanInteract = true; // Set to true when looking at an interactable object

    //Custom Interact Prompt Text
    //Example: "Press E to Pick Up" or "Press E to Partyy!"
    [HideInInspector]public static string interactTextString = "[E]";

    void Update()
    {
        //Raycast from mouse position(center screen)
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(playerCamTransform.position, playerCamTransform.forward, out hit, InteractRange, interactableLayer) && CanInteract == true)
        {
            //if looking at something with IInteractable class on it...
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                if (hit.collider.gameObject.CompareTag("CanGrab"))
                {
                    interactTextString = "[E] - Grab";

                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        //use the method from the script of the item you are looking at
                        interactObj.Interact();
                    }
                }
                else if (hit.collider.gameObject.CompareTag("CanInteract"))
                {
                    interactTextString = "[E] - Interact";

                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        //use the method from the script of the item you are looking at
                        interactObj.Interact();
                    }
                }
                else if (hit.collider.gameObject.CompareTag("CanHold"))
                {
                    interactTextString = "[E] - Hold";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //use the method from the script of the item you are looking at
                        interactObj.Interact();
                    }
                }

                interactPrompt.text = interactTextString;
                interactText.SetActive(true);

            }
        }
        //if not looking at something with IInteractable class then turn off prompt
        else
        {
            interactText.SetActive(false);
        }
    }
}
