using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static InventorySystem;
using System;
using JetBrains.Annotations;
//using UnityEditor.ShortcutManagement; //Was preventing build
using UnityEngine.UI;
public class RefuelTank : MonoBehaviour, IInteractable
{
    bool taskdone = false;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameEventsManager gameEventsManager;
    [SerializeField] Image fillCircle;
    [SerializeField] float holdDuration = 1f;
    float holdTimer;
    bool isHolding = false;
    float Distancefromplayer;

    int numberOfGasCanUsed = 0;

    public InventoryItemData referenceItem_GasCan;

    [SerializeField] Animator ReactorAnimator;
    void Start()
    {
        fillCircle.fillAmount = 0;
        ReactorAnimator.Play("ReactorPistonAnim");
    }
    public void Interact()
    {
        isHolding = true;
    }
    void Update()
    {
        Distancefromplayer = Vector3.Distance(playerTransform.position, this.transform.position);

        if (Distancefromplayer >= 4f || !Input.GetKey(KeyCode.E))
        {
            isHolding = false;
        }

            InventoryItem item = InventorySystem.current.Get(referenceItem_GasCan);
        if (item != null && item.data.id == "InventoryItem_GasCan" && Distancefromplayer <= 4f && isHolding)
        {
            fillCircle.gameObject.SetActive(true);
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                //Hold complete / Gas can used
                fillCircle.fillAmount = 0;
                holdTimer = 0;
                InventorySystem.current.Remove(referenceItem_GasCan);
                numberOfGasCanUsed++;

                if (numberOfGasCanUsed >= 2 && taskdone == false)
                {
                    taskdone = true;
                    gameEventsManager.tasksRemaining--;
                    gameObject.tag = "Used";
                    gameObject.layer = 0;
                    ReactorAnimator.Play("WorkingReactor");
                }
            }
        }

        if (isHolding == false)
        {
            fillCircle.gameObject.SetActive(false);
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
