using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBDeadState : EnemyState<DeathBringerStateEnum>
{
    public DBDeadState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }
}
