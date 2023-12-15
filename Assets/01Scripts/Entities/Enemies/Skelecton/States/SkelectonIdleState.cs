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
        //idle대기시간만큰 대기했다가 이동시작.
        _delayCoroutine = _enemyBase.StartDelayCallback(_enemyBase.idleTime, () =>
        {
            _stateMachine.ChangeState(SkelectonStateEnum.Move);
        });
    }

    public override void Exit()
    {
        //나가기전에 코루틴 종료시키고 간다. 만약 다른 조건에 의해서 Idle에서 다른 상태로 간다면 종료해야해.
        if (_delayCoroutine != null)
            _enemyBase.StopCoroutine(_delayCoroutine);

        //나갈때 벽에 있거나 땅감지가 안되면
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
