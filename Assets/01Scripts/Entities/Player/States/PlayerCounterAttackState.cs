using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private readonly int _successCounterHash = Animator.StringToHash("SuccessCounter");

    private float _counterTimer;
    private Collider2D[] _hitResult;

    private bool _cloneCreated = false;

    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _hitResult = new Collider2D[1]; //카운터는 한명만.
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _counterTimer = _player.counterAttackDuration; //카운터어택 시간 초기화.
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
                    _counterTimer = 5f; //일단 크게 넣어주면 애니메이터에 의해서 알아서 종료된다.
                    _player.AnimatorCompo.SetBool(_successCounterHash, true);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
