using UnityEngine;
using UnityEngine.InputSystem;

public class HoldButton : MonoBehaviour
{
     /*public PressHold pressHold;  
     public bool isHolding;

    public void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    public void OnEnable()
    {
        PressHold.Player.HoldAction.performed += ctx => StartHold();
        PressHold.Player.HoldAction.canceled += ctx => StopHold();
        PressHold.Enable();
    }

    public void OnDisable()
    {
        PressHold.Player.HoldAction.performed -= ctx => StartHold();
        PressHold.Player.HoldAction.canceled -= ctx => StopHold();
        PressHold.Disable();
    }

    public void StartHold()
    {
        isHolding = true;
        Debug.Log("E key is being held down!");
    }

    public void StopHold()
    {
        isHolding = false;
        Debug.Log("E key released!");
    }

    public void Update()
    {
        if (isHolding)
        {
            // Continuous action while holding 'E'
            Debug.Log("Holding the E key");
        }
    }*/
}
