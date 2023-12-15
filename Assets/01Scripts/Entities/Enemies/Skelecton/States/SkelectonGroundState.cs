using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkelectonGroundState : EnemyState<SkelectonStateEnum>
{
    protected Player _player;

    protected SkelectonGroundState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player = PlayerManager.Instance.Player;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        RaycastHit2D hit = _enemyBase.IsPlayerDetected();
        //플레이와의 거리 : 뒤쪽에서 접근해도 인지할 수 있도록
        
        //거리가 가깝거나 감지 거리상에 플레이어가 있다면.
        float distance = Vector2.Distance(_enemyBase.transform.position, _player.transform.position);

        if(distance < 2  || (hit && !_enemyBase.IsObstacleInLine(hit.distance)) )
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Battle);
            return;
        }
    }
}
