using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private readonly int _successCounterHash = Animator.StringToHash("SuccessCounter");

    private float _counterTimer;
    private Collider2D[] _hitResult;

    private bool _cloneCreated = false;

    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _hitResult = new Collider2D[1]; //ī���ʹ� �Ѹ�.
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _counterTimer = _player.counterAttackDuration; //ī���;��� �ð� �ʱ�ȭ.
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
                    _counterTimer = 5f; //�ϴ� ũ�� �־��ָ� �ִϸ����Ϳ� ���ؼ� �˾Ƽ� ����ȴ�.
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
