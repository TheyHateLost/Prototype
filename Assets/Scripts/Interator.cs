using UnityEngine;

// Define the Interactable interface correctly
public interface Interactable
{
    void Interact();
}

// Main interaction script
public class IntertheAct : MonoBehaviour
{
    public Transform InteracterSource;
    public float InteractRange = 2f; // Default value for safety

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteracterSource.position, InteracterSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                // Try to get a component that implements Interactable
                Interactable interactObj = hitInfo.collider.gameObject.GetComponent<Interactable>();
                if (interactObj != null)
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
