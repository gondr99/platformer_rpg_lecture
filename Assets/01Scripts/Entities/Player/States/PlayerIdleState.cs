using UnityEngine;
using UnityEngine.Windows;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float xInput = _player.PlayerInput.XInput;

        if (Mathf.Abs(_player.FacingDirection + xInput) > 1.2f && _player.IsWallDetected())
            return;

        if (Mathf.Abs(xInput) > 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Move);
        }
    }
}
