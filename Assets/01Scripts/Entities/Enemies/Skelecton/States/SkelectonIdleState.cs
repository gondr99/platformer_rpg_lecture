using UnityEngine;

public class SkelectonIdleState : SkelectonGroundState
{
    private Coroutine _delayCoroutine = null;

    public SkelectonIdleState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(_delayCoroutine != null) 
            _enemyBase.StopCoroutine(_delayCoroutine);
        _enemyBase.StopImmediately(false);
        //idle���ð���ū ����ߴٰ� �̵�����.
        _delayCoroutine = _enemyBase.StartDelayCallback(_enemyBase.idleTime, () =>
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Move);
        });
    }

    public override void Exit()
    {
        //���������� �ڷ�ƾ �����Ű�� ����. ���� �ٸ� ���ǿ� ���ؼ� Idle���� �ٸ� ���·� ���ٸ� �����ؾ���.
        if (_delayCoroutine != null)
            _enemyBase.StopCoroutine(_delayCoroutine);

        //������ ���� �ְų� �������� �ȵǸ�
        if (_enemyBase.IsWallDetected() || !_enemyBase.IsGroundDetected())
        {
            _enemyBase.Flip();
        }
        base.Exit();
    }

    public override void UpdateState()
    {
        
        base.UpdateState();
    }
}
