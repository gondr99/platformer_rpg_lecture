using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBIdleState : EnemyState<DeathBringerStateEnum>
{
    public DBIdleState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

}
