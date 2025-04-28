using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static InventorySystem;
using NUnit.Framework.Interfaces;

public class FuseBox : MonoBehaviour,IInteractable
{
    public InventoryItemData referenceItem;
    int numberOfFuses = 0;
    public List<ItemRequirement> requirements;
    public bool removeRequirements;
    [SerializeField] GameObject[] FusesInFuseBox;

    public void Interact()
    {
        InventoryItem item = InventorySystem.current.Get(referenceItem);
        if (item != null && item.data.id == "InventoryItem_Fuse")
        {
            InventorySystem.current.Remove(referenceItem);

            FusesInFuseBox[numberOfFuses].SetActive(true);
            numberOfFuses++;
            Debug.Log(numberOfFuses);
        }

    }

    /*bool MeetsRequirements()
    {
        foreach (ItemRequirement requirement in requirements)
        {
            if (!requirement.HasRequirement()) { return false; }
        }

        return true;
    }
    void RemoveRequirements()
    {
        foreach (ItemRequirement requirement in requirements)
        {
            for (int i = 0; i < requirement.amount; i++)
            {
                InventorySystem.current.Remove(requirement.itemData);
                Debug.Log("Removed " + requirement.itemData);
            }
        }
    }*/
}
