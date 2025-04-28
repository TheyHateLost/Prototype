using UnityEditor;
using UnityEngine;

public class FuseBox : MonoBehaviour,IInteractable
{
    public InventoryItemData referenceItem;
    int numberOfFuses = 0;

    public void Interact()
    {
        InventorySystem.current.Remove(referenceItem);
        Debug.Log("Removed " + referenceItem.displayName);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (referenceItem.id == "")
        {
            Debug.LogError("Reference item is not set in the inspector.");
        }
        //Debug.Log("FuseBox started");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
