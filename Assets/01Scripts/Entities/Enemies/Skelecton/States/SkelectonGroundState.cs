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
        //�÷��̿��� �Ÿ� : ���ʿ��� �����ص� ������ �� �ֵ���
        
        //�Ÿ��� �����ų� ���� �Ÿ��� �÷��̾ �ִٸ�.
        float distance = Vector2.Distance(_enemyBase.transform.position, _player.transform.position);

        if(distance < 2  || (hit && !_enemyBase.IsObstacleInLine(hit.distance)) )
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Battle);
            return;
        }
    }
}
