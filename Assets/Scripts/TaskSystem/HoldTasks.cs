using System;
using JetBrains.Annotations;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldTasks : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        public float 
        private bool isHolding = false;
    
     }

    // Update is called once per frame
    void Update()
    {
    if (isHolding)

    {
        holdTimer += Time.deltaTime
        object Fillcircle = null;
        int holdDuration = 0;
        int holdTimer = 0;
        Fillcircle.fillAmount = holdTimer / holdDuration;
        if (holdTimer >= holdDuration) 
        //Hold to complete task
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
    
}
