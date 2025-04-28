using UnityEngine;

public class PuzzleTrigger : MonoBehaviour, IInteractable
{
    public GameObject puzzleCanvas;
    private bool playerNearby = false;


    public void Interact()
    {
        if (puzzleCanvas.activeInHierarchy)
        {
            puzzleCanvas.SetActive(false);
        }
        else
        {
            puzzleCanvas.SetActive(true);
        }
    }
    void Update()
    {
        /*if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            puzzleCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}

