using Mono.Cecil;
using NUnit.Framework.Internal;
using UnityEngine;

public class Drawer : MonoBehaviour, IInteractable
{
    bool drawerIsOpen = false;
    bool OpenDrawer = false;
    bool CanInteract = true;
    Vector3 startPosition;
    Vector3 targetPosition;
    float Speed = 0.01f;

    public void Interact()
    {
        if (CanInteract == true)
        {
            //If the drawer is open, close it
            if (drawerIsOpen == false)
            {
                //OpenDrawer
                OpenDrawer = true;
                CanInteract = false;
            }
            //If the drawer is closed, open it
            if (drawerIsOpen == true)
            {
                //CloseDrawer
                OpenDrawer = false;
                CanInteract = false;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x - 0.6f, startPosition.y, startPosition.z);
    }

    void Update()
    {
        if (OpenDrawer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                CanInteract = true;
                drawerIsOpen = true;
            }
        }
        else if (OpenDrawer == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Speed);

            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                CanInteract = true;
                drawerIsOpen = false;
            }
        }
    }
}
