using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private readonly int _successCounterHash = Animator.StringToHash("SuccessCounter");

    private float _counterTimer;
    private Collider2D[] _hitResult;


    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _hitResult = new Collider2D[1];
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _counterTimer = _player.counterAttackDuration; 
        _player.AnimatorCompo.SetBool(_successCounterHash, false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _counterTimer -= Time.deltaTime;

        CheckCounter();

        if (_counterTimer < 0 || _triggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void CheckCounter()
    {
        DamageCaster caster = _player.DamageCasterCompo;
        int cnt = Physics2D.OverlapCircle(
            caster.attackChecker.position, 
            caster.attackCheckRadius, 
            new ContactFilter2D { layerMask = caster.whatIsEnemy, useLayerMask = true }, 
            _hitResult);
            

        for (int i = 0; i < cnt; ++i)
        {
            if (_hitResult[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (enemy.CanBeStunned())
                {
                    _counterTimer = 5f; //일단 5초로 놓으면 나중에 애니메이션에 의해 트리거
                    _player.AnimatorCompo.SetBool(_successCounterHash, true);

                    Vector2 direction = new Vector2(_player.stunDirection.x * _player.FacingDirection, _player.stunDirection.y);
                    caster.CastDamageWithStun(enemy, 2f, direction, _player.stunDirection, _player.stunDuration);

                    SkillManager.Instance.GetSkill<CloneSkill>()?.CreateCloneOnCounterAttack(enemy.transform);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
