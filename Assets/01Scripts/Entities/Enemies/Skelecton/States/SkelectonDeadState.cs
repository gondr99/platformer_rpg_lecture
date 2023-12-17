using DG.Tweening;
using UnityEngine;

public class SkelectonDeadState : EnemyState<SkelectonStateEnum>
{
    public SkelectonDeadState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _enemyBase.StartDelayCallback(3f, () =>
        {
            _rigidbody.gravityScale = 0;
            _enemyBase.Collder2DCompo.enabled = false;
            _enemyBase.SpriteRendererCompo.DOFade(0f, 1f).OnComplete(() => GameObject.Destroy(_enemyBase.gameObject));
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
