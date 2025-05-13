using UnityEngine;

public class Secret_Computer : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject SecretComputer_Text;

    public void Interact()
    {
        SecretComputer_Text.SetActive(true);
        gameObject.tag = "Used";
        gameObject.layer = 0;
    }
}
