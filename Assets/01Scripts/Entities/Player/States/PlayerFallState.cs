
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
        //���� �� ������� �����ְ�.
        CameraManager.Instance.LerpedFromPlayerFalling = false;
        CameraManager.Instance.LerpYDamping(false); //���󺹱�
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        CameraManager manager = CameraManager.Instance;

        //�Ӱ谪�� �Ѿ �������� �ְ�
        bool overCameraThreshold = _rigidbody.velocity.y < manager.fallSpeedYDampingChangeThreshold;
        
        //���� �������� �ƴ϶��
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
