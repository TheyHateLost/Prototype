using UnityEngine;

//Script is for actiavting the puzzle UI - goes on physical object that will be used for bringing up the UI
public class PuzzleTrigger : MonoBehaviour, IInteractable
{
    public GameObject PuzzleUI;
    bool playerNearby = false;
    [SerializeField] Transform playerTransform;
    float DistanceFromPlayer;
    float taskRadius = 4f;

    public void Interact()
    {
        if (PuzzleUI.activeInHierarchy)
        {
            PuzzleUI.SetActive(false);
        }
        else
        {
            PuzzleUI.SetActive(true);
        }
    }
    void Update()
    {
        DistanceFromPlayer = Vector3.Distance(playerTransform.position, this.transform.position);

        if (DistanceFromPlayer >= taskRadius)
        {
            PuzzleUI.SetActive(false);
        }
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

