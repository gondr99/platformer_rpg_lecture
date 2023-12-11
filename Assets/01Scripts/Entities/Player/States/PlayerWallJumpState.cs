
public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(5 * -_player.FacingDirection, _player.jumpForce);

        _player.StartDelayCallback(0.4f, () =>
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        });
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
