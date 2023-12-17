
using DG.Tweening.Core.Easing;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkelectonBattleState : EnemyState<SkelectonStateEnum>
{
    protected Player _player;
    private int _moveDirection;
    private float _timer;

    private readonly int _xVelocityHash = Animator.StringToHash("x_velocity");

    public SkelectonBattleState(Enemy enemyBase, EnemyStateMachine<SkelectonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player = PlayerManager.Instance.Player;
        SetDirectionToEnemy();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //이동속도를 계속 갱신
        _enemyBase.AnimatorCompo.SetFloat(_xVelocityHash, Mathf.Abs(_rigidbody.velocity.x));

        if (_player.transform.position.x > _enemyBase.transform.position.x)
            _moveDirection = 1;
        else
            _moveDirection = -1;
        _enemyBase.SetVelocity(_enemyBase.moveSpeed * _moveDirection, _rigidbody.velocity.y);

        RaycastHit2D hit = _enemyBase.IsPlayerDetected();
        //레이캐스트힛구조체 안에 operator bool 이 재정의 되어 있음. hit.collider == null 체크함.

        //공격 가능하다면 공격
        if (hit && !_enemyBase.IsObstacleInLine(hit.distance))
        {
            _timer = _enemyBase.battleTime; //타이머 설정

            if (hit.distance < _enemyBase.attackDistance && CanAttack())
            {
                _stateMachine.ChangeState(SkelectonStateEnum.Attack);
                return;
            }
        }


        float distance = Vector2.Distance(_player.transform.position, _enemyBase.transform.position);


        //앞이 절벽이거나 적이 근거리라면.
        if (!_enemyBase.IsGroundDetected() || (distance <= _enemyBase.attackDistance))
        {
            _enemyBase.StopImmediately(false);  //블렌드 트리로 값에 따라 정지와 이동이 나오게
            return;
        }

        if (_timer >= 0 && distance < _enemyBase.runAwayDistance)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            // 전투시간을 초과했거나 인식거리 밖으로 벗어났다면
            _stateMachine.ChangeState(SkelectonStateEnum.Idle); 
        }
    }

    //적 위치를 바라보도록 코드
    private void SetDirectionToEnemy()
    {
        _enemyBase.FlipController(_player.transform.position.x - _enemyBase.transform.position.x);
    }

    private bool CanAttack()
    {
        return Time.time >= _enemyBase.lastTimeAttacked + _enemyBase.attackCooldown;
    }
}
