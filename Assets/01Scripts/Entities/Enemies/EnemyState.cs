using System;
using UnityEngine;

public class EnemyState<T> where T : Enum
{
    protected EnemyStateMachine<T> _stateMachine;
    protected Enemy _enemyBase;

    protected bool _triggerCalled;
    protected int _animBoolHash;

    protected Rigidbody2D _rigidbody;

    public EnemyState(Enemy enemyBase, EnemyStateMachine<T> stateMachine, string animBoolName)
    {
        _enemyBase = enemyBase;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }

    public virtual void UpdateState()
    {

    }

    public virtual void Enter()
    {
        _triggerCalled = false;
        _enemyBase.AnimatorCompo.SetBool(_animBoolHash, true);
        _rigidbody = _enemyBase.RigidbodyCompo;
    }

    public virtual void Exit()
    {
        _enemyBase.AnimatorCompo.SetBool(_animBoolHash, false);
        _enemyBase.AssignLastAnimHash(_animBoolHash); //마지막으로 실행한 해시를 저장한다.
    }

    public void AnimationFinishTrigger()
    {
        _triggerCalled = true;
    }
}
