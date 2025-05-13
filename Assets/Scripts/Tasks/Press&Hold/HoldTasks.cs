using System;
using JetBrains.Annotations;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class HoldTasks : MonoBehaviour, IInteractable
{
    bool taskdone = false; 
    [SerializeField] Transform playerTransform;
    [SerializeField] Image fillCircle;
    [SerializeField] float holdDuration = 1f;
    [SerializeField] float taskRadius = 4f;
    float holdTimer;
    bool isHolding = false;
    float Distancefromplayer;

    public static event Action OnHoldComplete;

    public void Interact()
    {
        isHolding = true; 
    }

    void Start()
    {
        fillCircle.fillAmount = 0;
    }
    void Update()
    {
        Distancefromplayer = Vector3.Distance(playerTransform.position, this.transform.position);

        if (Distancefromplayer >= taskRadius)
        {
            isHolding = false;
            fillCircle.gameObject.SetActive(false);
        }
        else
        {
            fillCircle.gameObject.SetActive(true);
        }
           
        if (!Input.GetKey(KeyCode.E))
        {
            isHolding = false;
        }

        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration && taskdone==false)
                
            {  taskdone = true;
                //Do Task
                GameEventsManager.tasksRemaining--; //ResetHold();
                fillCircle.gameObject.SetActive(false);
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
