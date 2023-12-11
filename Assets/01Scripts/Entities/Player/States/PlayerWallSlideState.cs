using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.JumpEvent -= HandleJumpEvent;
    }

    private void HandleJumpEvent()
    {
        _stateMachine.ChangeState(PlayerStateEnum.WallJump);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        if (yInput < 0)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y * 0.7f);
        }

        //다른방향으로 키를 눌렀다면.
        if (Mathf.Abs(xInput + _player.FacingDirection) < 0.5f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        //땅에 닿았다면 취소.
        if (_player.IsGroundDetected() || !_player.IsWallDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
