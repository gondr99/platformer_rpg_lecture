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
        _enemyBase.lastTimeAttacked = Time.time; //���������� ������ �ð��� �����.
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_triggerCalled) //�ִϸ��̼��� �����ٸ�
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Battle); //�������·� �ٽ� ��ȯ.
        }
    }
}
