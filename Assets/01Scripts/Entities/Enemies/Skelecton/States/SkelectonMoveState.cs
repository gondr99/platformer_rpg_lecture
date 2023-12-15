
public class SkelectonMoveState : SkelectonGroundState
{
    public SkelectonMoveState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
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
        _enemyBase.SetVelocity(_enemyBase.moveSpeed * _enemyBase.FacingDirection, _rigidbody.velocity.y);

        if (_enemyBase.IsWallDetected() || !_enemyBase.IsGroundDetected())
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Idle);
        }
    }
}
