using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using static InventorySystem;

public class ItemObject : MonoBehaviour, IInteractable
{
    public InventoryItemData referenceItem;

    public void Interact()
    {
        OnHandlePickupItem();
        //Debug.Log("Picked up " + referenceItem.displayName);
    }
    public void OnHandlePickupItem()
    {
        InventorySystem.current.Add(referenceItem);
        Destroy(gameObject);
    }
}
