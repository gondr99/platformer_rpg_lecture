using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        _player.SetVelocity(xInput * _player.moveSpeed, _rigidbody.velocity.y);

        if(Mathf.Abs(xInput) < 0.05f || _player.IsWallDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
