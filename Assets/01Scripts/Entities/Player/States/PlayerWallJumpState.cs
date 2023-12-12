
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private Coroutine _delayCoroutine = null;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(5 * -_player.FacingDirection, _player.jumpForce);

        _delayCoroutine = _player.StartDelayCallback(0.4f, () =>
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        });
    }

    public override void Exit()
    {
        _player.StopCoroutine(_delayCoroutine);
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
