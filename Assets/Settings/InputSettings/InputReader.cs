using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions, IUIActions
{
    #region input event section
    public event Action JumpEvent;
    public event Action DashEvent;
    public event Action PrimaryAttackEvent;
    public event Action CounterAttackEvent;
    public event Action<bool> ThrowSwordEvent;
    public event Action UltiSkillEvent;
    public event Action CrystalSkillEvent;
    public event Action<bool> InteractionEvent;

    public event Action OpenMenuEvent;
    #endregion

    #region input value section
    public float XInput { get; private set; }
    public float YInput { get; private set; }
    public Vector2 AimPosition { get; private set; }
    #endregion

    private Controls _controls;

    private void OnEnable()
    {
        if(_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.UI.SetCallbacks(this);

        }
        _controls.Player.Enable();
        _controls.UI.Enable();
    }

    public void SetPlayerInputEnable(bool value)
    {
        if(value)
            _controls.Player.Enable();
        else
            _controls.Player.Disable();
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

    public void OnCounterAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
            CounterAttackEvent?.Invoke();
    }

    public void OnAimPoision(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

    public void OnThrowSword(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ThrowSwordEvent?.Invoke(true);
        }else if (context.canceled)
        {
            ThrowSwordEvent?.Invoke(false);
        }
    }

    public void OnUltiSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UltiSkillEvent?.Invoke();
        }
    }

    public void OnCrystalSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CrystalSkillEvent?.Invoke();
        }
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
            OpenMenuEvent?.Invoke();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(context.performed)
            InteractionEvent?.Invoke(true);
        else if (context.canceled)
            InteractionEvent?.Invoke(false);
    }
}
