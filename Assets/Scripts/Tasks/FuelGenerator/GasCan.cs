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
    [SerializeField] PlayerController playerScript;
    public InventoryItemData referenceItem;

    public void Interact()
    {
        Player_GasCan.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // If the gas can prefab is active in the hierarchy, the player is holding the gas can 
        if (Player_GasCan.activeInHierarchy)
        {
            HoldingGasCan = true;

            Interator.CanInteract = false;

            if (PlayerController.crouching != true)
            {
                playerScript.currentMoveSpeed = playerScript.walkSpeed;
            }

            // If the player presses the E key, drop the gas can
            if (Input.GetKeyUp(KeyCode.E))
            {
                DropGasCan();
                InventorySystem.current.Remove(referenceItem);
            }
        }
        else
        {
            HoldingGasCan = false;
            Interator.CanInteract = true;
        }

        // If the player is holding the gas can, set the player's move speed always to walk speed and disable interaction
        /*if (HoldingGasCan == true)
        {
            Interator.CanInteract = false;

            if (PlayerController.crouching != true)
            {
                playerScript.currentMoveSpeed = playerScript.walkSpeed;
            }

            // If the player presses the E key, drop the gas can
            if (Input.GetKeyUp(KeyCode.E))
            {
                DropGasCan();
                InventorySystem.current.Remove(referenceItem);
            }
        }
        else
        {
            Interator.CanInteract = true;
        }*/
    }

    void DropGasCan()
    {
        Player_GasCan.SetActive(false);
        InventorySystem.current.Remove(referenceItem);
        Instantiate(gasCanPrefab, playerScript.transform.position, Quaternion.identity);
    }
}
