using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make teleport to boss2023.12.27
//this is boss's battlestate
public class DBMoveState : EnemyState<DeathBringerStateEnum>
{
    protected Player _player;
    protected int _moveDirection;

    public DBMoveState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
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
            if (hit.distance < _enemyBase.attackDistance && CanAttack())
            {
                _stateMachine.ChangeState(DeathBringerStateEnum.Attack);
                return;
            }
        }


        float distance = Vector2.Distance(_player.transform.position, _enemyBase.transform.position);

        float yDistance = Mathf.Abs(_player.transform.position.y - _enemyBase.transform.position.y);

        //앞이 절벽이거나, 적이 너무 멀리 떨어졌다면.
        if (!_enemyBase.IsGroundDetected() || (distance >= _enemyBase.runAwayDistance) || (yDistance > 2.5f && _player.IsGroundDetected()))
        {
            _enemyBase.StopImmediately(false);  //블렌드 트리로 값에 따라 정지와 이동이 나오게
            _stateMachine.ChangeState(DeathBringerStateEnum.Teleport);
            return;
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
