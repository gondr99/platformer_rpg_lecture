using UnityEngine;

public class SkelectonStunState : EnemyState<SkelectonStateEnum>
{
    private Coroutine _stunCoroutine;
    public SkelectonStunState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(_stunCoroutine != null) {
            _enemyBase.StopCoroutine(_stunCoroutine);
        }

        _enemyBase.CanStateChangeable = false;

        _stunCoroutine = _enemyBase.StartDelayCallback(_enemyBase.stunDuration, () =>
        {
            _enemyBase.CanStateChangeable = true;
            _stateMachine.ChangeState(SkelectonStateEnum.Battle);
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
