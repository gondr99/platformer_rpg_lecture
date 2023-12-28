using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBStunState : EnemyState<DeathBringerStateEnum>
{
    private Coroutine _stunCoroutine;
    private EnemyDeathBringer _deathBringer;
    public DBStunState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _deathBringer = enemyBase as EnemyDeathBringer;
    }

    public override void Enter()
    {
        base.Enter();
        if (_stunCoroutine != null)
        {
            _enemyBase.StopCoroutine(_stunCoroutine);
        }

        _deathBringer.stunCount += 1;
        if(_deathBringer.stunCount >= 3)
        {
            //enemy will change to cast state when every three counter entered
            // wait 0.5 sec for play stun animation end
            _stunCoroutine = _enemyBase.StartDelayCallback(0.5f, () =>
            {
                _stateMachine.ChangeState(DeathBringerStateEnum.Teleport);
            });
            
            return;
        }

        _enemyBase.CanStateChangeable = false;

        _stunCoroutine = _enemyBase.StartDelayCallback(_enemyBase.stunDuration, () =>
        {
            _enemyBase.CanStateChangeable = true;
            _stateMachine.ChangeState(DeathBringerStateEnum.Move);
        });
    }
}
