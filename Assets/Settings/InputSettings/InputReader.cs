using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    #region input event section
    public event Action JumpEvent;
    public event Action DashEvent;
    public event Action PrimaryAttackEvent;
    #endregion

    #region input value section
    public float XInput { get; private set; }
    public float YInput { get; private set; }
    #endregion

    private Controls _controls;

    private void OnEnable()
    {
        if(_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    public void OnXMovement(InputAction.CallbackContext context)
    {
        XInput = context.ReadValue<float>();
    }

    public void OnYMovement(InputAction.CallbackContext context)
    {
        YInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) 
            JumpEvent?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
            DashEvent?.Invoke();
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
            PrimaryAttackEvent?.Invoke();
    }
}
