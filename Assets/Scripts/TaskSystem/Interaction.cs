using TMPro;
using UnityEngine;

// Grabs the Interact method from other scripts using IInteractable
interface IInteractable2
{
    public void Interact();
    //public void InteractText();
}
// Main interaction script
public class Interaction : MonoBehaviour
{
    public float InteractRange = 2f; // Default value for safety
    public GameObject interactText;
    public TextMeshProUGUI interactPrompt;

    [SerializeField] Transform playerCamTransform;
    [SerializeField] LayerMask interactableLayer;

    //Custom Interact Prompt Text
    //Example: "Press E to Pick Up" or "Press E to Partyy!"
    [HideInInspector] public static string interactTextString = "[E]";

    void Update()
    {
        //Raycast from mouse position(center screen)
        RaycastHit hit;
        if (Physics.Raycast(playerCamTransform.position, playerCamTransform.forward, out hit, InteractRange, interactableLayer))
        {
            interactPrompt.text = "Press E to Interact";
            interactText.SetActive(true);
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
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
