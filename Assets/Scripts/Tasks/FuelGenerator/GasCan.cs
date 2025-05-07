using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static InventorySystem;

public class GasCan : MonoBehaviour, IInteractable
{
    bool HoldingGasCan = false;

    [SerializeField] GameObject Player_GasCan;
    [SerializeField] GameObject gasCanPrefab;
    GameObject DroppedGasCan;
    [SerializeField] PlayerController playerScript;
    public InventoryItemData referenceItem;

    public void Interact()
    {
        Destroy(DroppedGasCan);
    }

    // Update is called once per frame
    void Update()
    {
        InventoryItem item = InventorySystem.current.Get(referenceItem);
        if (item != null && item.data.id == "InventoryItem_GasCan")
        {
            Interator.CanInteract = false;
            Player_GasCan.SetActive(true);

            if (PlayerController.crouching != true)
            {
                playerScript.currentMoveSpeed = 6f;
            }

            // If the player presses the Q key, drop the gas can
            if (Input.GetKeyUp(KeyCode.Q))
            {
                DropGasCan();
            }
        }
        else
        {
            Interator.CanInteract = true;
            Player_GasCan.SetActive(false);
        }
    }

    void DropGasCan()
    {
        Player_GasCan.SetActive(false);
        InventorySystem.current.Remove(referenceItem);
        Instantiate(DroppedGasCan, playerScript.transform.position, Quaternion.identity);
    }
}
