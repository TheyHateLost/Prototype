using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static InventorySystem;

public class FuseBox : MonoBehaviour,IInteractable
{
    public InventoryItemData referenceItem;
    int numberOfFuses = 0;

    public List<ItemRequirement> requirements;

    public void Interact()
    {
        InventorySystem.current.Remove(referenceItem);
        Debug.Log("Removed " + referenceItem.displayName);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (referenceItem.id == "InventoryItem_Fuse")
        {
            InventorySystem.current.Get(referenceItem);
            //Debug.LogError("Reference item is not set in the inspector.");
        }
        //Debug.Log("FuseBox started");
    }

    // Update is called once per frame
    void Update()
    {
        if (MeetsRequirements())
        {
            InventorySystem.current.Remove(referenceItem);
        }
    }

    bool MeetsRequirements()
    {
        foreach (ItemRequirement requirement in requirements)
        {
            if (!requirement.HasRequirement()) { return false; }
        }

        return true;
    }
}
