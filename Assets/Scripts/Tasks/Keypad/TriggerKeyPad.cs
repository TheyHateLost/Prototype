using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static InventorySystem;
public class TriggerKeyPad : MonoBehaviour, IInteractable
{
    public GameObject keypadUI;
    [SerializeField] Transform Player_Transform;
    public InventoryItemData referenceItem_KeyCard;
    [SerializeField] GameObject CardRequired_Text;
    float TextDisappear_Timer = 3f;
    float distanceFromPlayer;

    //Attach to Physcial Task Objects, make interact scrip aware of thse objects
    //Brings up the assigned UI
    public void Interact()
    {
        InventoryItem item = InventorySystem.current.Get(referenceItem_KeyCard);
        if (item != null && item.data.id == "InventoryItem_Keycard" && KeypadTask.isResetting == false)
        {
            keypadUI.SetActive(true);
        }
        else
        {
            CardRequired_Text.SetActive(true);
        }
    }

    void Update() 
    {
        distanceFromPlayer = Vector3.Distance(Player_Transform.position, gameObject.transform.position);

        //Closes keypad UI if too far
        if (distanceFromPlayer >= 4f)
        {
            keypadUI.SetActive(false);

        }

        //Interact prompt does not show up while using keypad
        if (keypadUI.activeInHierarchy)
        {
            gameObject.tag = "Used";
            gameObject.layer = 0;
        }
        else
        {
            gameObject.tag = "CanInteract";
            gameObject.layer = 6;
        }

        //Take away text after some time
        if (CardRequired_Text.activeInHierarchy)
        {
            TextDisappear_Timer -= Time.deltaTime;
            if (TextDisappear_Timer <= 0)
            {
                CardRequired_Text.SetActive(false);
                TextDisappear_Timer = 3f;
            }
        }
    }

}
