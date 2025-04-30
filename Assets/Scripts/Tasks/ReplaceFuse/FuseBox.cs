using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static InventorySystem;
using NUnit.Framework.Interfaces;

public class FuseBox : MonoBehaviour,IInteractable
{
    public InventoryItemData referenceItem;
    [SerializeField]int FusesInBox;
    [SerializeField] GameObject[] FusesInFuseBox;
    //public static int FuseBoxPowered = 0;
    public static int FusesAdded;

    public void Interact()
    {
        InventoryItem item = InventorySystem.current.Get(referenceItem);
        if (item != null && item.data.id == "InventoryItem_Fuse" && FusesInBox < 2)
        {
            InventorySystem.current.Remove(referenceItem);

            FusesInFuseBox[FusesInBox].SetActive(true);
            FusesInBox += 1;
            FusesAdded += 1;
            Debug.Log(FusesInBox);
        }
    }

    void Update()
    {
        if (FusesAdded >= 4)
        {
            Fuse_Lever.Lever_Active = true;
        }
    }
}
