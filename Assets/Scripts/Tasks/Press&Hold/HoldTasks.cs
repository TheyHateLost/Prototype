using System;
using JetBrains.Annotations;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class HoldTasks : MonoBehaviour, IInteractable
{
    public float holdDuration = 1f;
    public Image fillCircle;

    float holdTimer;
    bool isHolding = false;

    public static event Action OnHoldComplete;

    // Update is called once per frame

    public void Interact()
    {
         isHolding = true;
         
    }
    void Update()
    {
        if (!Input.GetKey(KeyCode.E))
        {
            isHolding = false;
        }

        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                //Load next level
                //OnHoldComplete.Invoke();
                ResetHold();
            }
        }
    }

    public void onHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true;
        }
        else if (context.canceled)
        {
            ResetHold();
        }
    }

    void ResetHold()
    {
        isHolding = false;
        holdTimer = 0;
        fillCircle.fillAmount = 0;
    }
}
