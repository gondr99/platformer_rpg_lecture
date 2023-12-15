using UnityEngine;

public class SkelectonAttackState : EnemyState<SkelectonStateEnum>
{
    public SkelectonAttackState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemyBase.StopImmediately(false);
    }

    public override void Exit()
    {
        _enemyBase.lastTimeAttacked = Time.time; //마지막으로 공격한 시간을 기록함.
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_triggerCalled) //애니메이션이 끝났다면
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Battle); //추적상태로 다시 전환.
        }
    }
}
