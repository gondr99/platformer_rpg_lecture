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
        _enemyBase.lastTimeAttacked = Time.time; //record skelecton's last attack time
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_triggerCalled) // attack animation playing finished
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Battle); //change to battle state
        }
    }
}
