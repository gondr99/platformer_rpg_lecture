
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float xInput = _player.PlayerInput.XInput;
        if (Mathf.Abs(xInput) > 0.05f)
        {
            _player.SetVelocity(_player.moveSpeed * 0.7f * xInput, _rigidbody.velocity.y);
        }

        if (_player.IsWallDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.WallSlide);
        }
    }
}
