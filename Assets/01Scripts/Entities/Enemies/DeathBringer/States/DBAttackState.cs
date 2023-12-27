using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBAttackState : EnemyState<DeathBringerStateEnum>
{
    
    public DBAttackState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemyBase.StopImmediately(false);

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(_triggerCalled)
        {
            _stateMachine.ChangeState(DeathBringerStateEnum.Move);
        }
    }
}
