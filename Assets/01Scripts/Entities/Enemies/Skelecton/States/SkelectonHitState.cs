
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
        //��Ŭ���� ��� ����Ǿ�� �ϹǷ� �̷�������� ó��
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
            //�¾Ҵٰ� Ǯ���� �������·� ����.
            _stateMachine.ChangeState(SkelectonStateEnum.Battle);
        }
    }
}
