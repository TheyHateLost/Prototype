using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public static InventorySystem current;
    public List<InventoryItem> inventory { get; private set; }

    void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public class InventoryItem
    {
        public InventoryItemData data { get; private set; }
        public int stackSize { get; private set; }

        public InventoryItem(InventoryItemData source)
        {
            data = source;
            AddToStack();
        }

        public void AddToStack()
        {
            stackSize++;
        }
        public void RemoveFromStack()
        {
            stackSize--;
        }
    }

    public void Add(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    [Serializable]
    public struct ItemRequirement
    {
        public InventoryItemData itemData;
        public int amount;

        public bool HasRequirement()
        {
            InventoryItem item = InventorySystem.current.Get(itemData);

            if (item == null || item.stackSize < amount) { return false; }

            return true;
        }
    }
}
