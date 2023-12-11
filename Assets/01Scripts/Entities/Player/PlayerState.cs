using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected Rigidbody2D _rigidbody;

    protected int _animBoolHash;
    protected readonly int _yVelocityHash = Animator.StringToHash("y_velocity");
    protected bool _triggerCalled;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
        _rigidbody = _player.RigidbodyCompo;
    }

    public virtual void Enter()
    {
        _player.AnimatorCompo.SetBool(_animBoolHash, true);
        _triggerCalled = false;
    }

    public virtual void UpdateState()
    {
        _player.AnimatorCompo.SetFloat(_yVelocityHash, _rigidbody.velocity.y);
    }

    public virtual void Exit()
    {
        _player.AnimatorCompo.SetBool(_animBoolHash, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        _triggerCalled = true;
    }
}
