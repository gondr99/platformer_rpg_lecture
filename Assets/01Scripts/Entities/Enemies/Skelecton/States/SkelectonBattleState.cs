
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

        //�̵��ӵ��� ��� ����
        _enemyBase.AnimatorCompo.SetFloat(_xVelocityHash, Mathf.Abs(_rigidbody.velocity.x));

        if (_player.transform.position.x > _enemyBase.transform.position.x)
            _moveDirection = 1;
        else
            _moveDirection = -1;
        _enemyBase.SetVelocity(_enemyBase.moveSpeed * _moveDirection, _rigidbody.velocity.y);

        RaycastHit2D hit = _enemyBase.IsPlayerDetected();
        //����ĳ��Ʈ������ü �ȿ� operator bool �� ������ �Ǿ� ����. hit.collider == null üũ��.

        //���� �����ϴٸ� ����
        if (hit && !_enemyBase.IsObstacleInLine(hit.distance))
        {
            _timer = _enemyBase.battleTime; //Ÿ�̸� ����

            if (hit.distance < _enemyBase.attackDistance && CanAttack())
            {
                _stateMachine.ChangeState(SkelectonStateEnum.Attack);
                return;
            }
        }


        float distance = Vector2.Distance(_player.transform.position, _enemyBase.transform.position);


        //���� �����̰ų� ���� �ٰŸ����.
        if (!_enemyBase.IsGroundDetected() || (distance <= _enemyBase.attackDistance))
        {
            _enemyBase.StopImmediately(false);  //���� Ʈ���� ���� ���� ������ �̵��� ������
            return;
        }

        if (_timer >= 0 && distance < _enemyBase.runAwayDistance)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            // �����ð��� �ʰ��߰ų� �νİŸ� ������ ����ٸ�
            _stateMachine.ChangeState(SkelectonStateEnum.Idle); 
        }
    }

    //�� ��ġ�� �ٶ󺸵��� �ڵ�
    private void SetDirectionToEnemy()
    {
        _enemyBase.FlipController(_player.transform.position.x - _enemyBase.transform.position.x);
    }

    private bool CanAttack()
    {
        return Time.time >= _enemyBase.lastTimeAttacked + _enemyBase.attackCooldown;
    }
}
