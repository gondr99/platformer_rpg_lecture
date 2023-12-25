
public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _player.CanStateChangeable = false; //상태변경 불가능
        _player.StartDelayCallback(1f, () => _player.StopImmediately(false));
    }
}
