using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static InventorySystem;

public class GasCan : MonoBehaviour
{
    [SerializeField] GameObject Player_GasCan;
    public InventoryItemData GasCanObject;
    static bool PlayerHasGasCan = false;    

    // Update is called once per frame
    void Update()
    {
        InventoryItem item = InventorySystem.current.Get(GasCanObject);
        if (item != null && item.data.id == "InventoryItem_GasCan")
        {
            PlayerHasGasCan = true;
            Player_GasCan.SetActive(true);
        }
        else
        {
            Player_GasCan.SetActive(false);
        }
    }
}
