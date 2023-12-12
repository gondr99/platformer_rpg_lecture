
public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        //나갈 때 원래대로 돌려주고.
        CameraManager.Instance.LerpedFromPlayerFalling = false;
        CameraManager.Instance.LerpYDamping(false); //원상복귀
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        CameraManager manager = CameraManager.Instance;

        //임계값을 넘어서 떨어지고 있고
        bool overCameraThreshold = _rigidbody.velocity.y < manager.fallSpeedYDampingChangeThreshold;
        
        //현재 댐핑중이 아니라면
        if (overCameraThreshold && !manager.IsLerpingYDamping && !manager.LerpedFromPlayerFalling)
        {
            manager.LerpYDamping(true);
        }


        if (_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

    }
}
