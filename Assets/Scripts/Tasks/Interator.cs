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

    //Custom Interact Prompt Text
    //Example: "Press E to Pick Up" or "Press E to Partyy!"
    [HideInInspector]public static string interactTextString = "[E]";

    void Update()
    {
        //Raycast from mouse position(center screen)
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(playerCamTransform.position, playerCamTransform.forward, out hit, InteractRange, interactableLayer))
        {
            //if looking at something with IInteractable class on it...
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                if (hit.collider.gameObject.CompareTag("CanGrab"))
                {
                    interactTextString = "[E] - Grab";
                }
                else if (hit.collider.gameObject.CompareTag("CanInteract"))
                {
                    interactTextString = "[E] - Interact";
                }

                interactPrompt.text = interactTextString;
                interactText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    //use the method from the script of the item you are looking at
                    interactObj.Interact();
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
