
using UnityEngine;

public class SkelectonHitState : EnemyState<SkelectonStateEnum>
{
    private readonly int _hitClipNameHash = Animator.StringToHash("hit");

    public SkelectonHitState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //힛클립은 계속 재생되어야 하므로 이런방식으로 처리
        _enemyBase.AnimatorCompo.Play(_hitClipNameHash, layer: -1, normalizedTime: 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_triggerCalled)
        {
            //맞았다가 풀리면 전투상태로 들어간다.
            _stateMachine.ChangeState(SkelectonStateEnum.Battle);
        }
    }
}
