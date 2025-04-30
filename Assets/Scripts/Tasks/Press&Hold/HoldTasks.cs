using System;
using JetBrains.Annotations;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class HoldTasks : MonoBehaviour, IInteractable
{
    public Transform playertransform;
    public float holdDuration = 1f;
    public Image fillCircle;
    public float taskRadius = 4f;
    float holdTimer;
    bool isHolding = false;
    float Distancefromplayer;

    public static event Action OnHoldComplete;

    // Update is called once per frame

    public void Interact()
    {
         isHolding = true;
         
    }

   
    void Start()
    {
        // Initialize the fill circle to be empty
        fillCircle.fillAmount = 0;
    }
    void Update()
    {
        Distancefromplayer = Vector3.Distance(playertransform.position, this.transform.position);



        if (Distancefromplayer >= taskRadius)
        {
            isHolding = false;
        }

           

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
