using System;
using UnityEngine;

public enum SkelectonStateEnum
{
    Idle,
    Move,
    Battle,
    Attack,
    Hit,
    Stun,
    Dead
}

public class EnemySkelecton : Enemy
{
    public EnemyStateMachine<SkelectonStateEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<SkelectonStateEnum>(); //state machine make by generic

        foreach (SkelectonStateEnum state in Enum.GetValues(typeof(SkelectonStateEnum)))
        {
            string typeName = state.ToString();
            Type t = Type.GetType($"Skelecton{typeName}State");

            if (t != null)
            {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<SkelectonStateEnum>;
                StateMachine.AddState(state, enemyState);
            }
            else
            {
                Debug.LogError($"Enemy skelecton : no state [ {typeName} ]");
            }
        }

    }

    
    protected override void HandleHitEvent()
    {
        base.HandleHitEvent();
        StateMachine.ChangeState(SkelectonStateEnum.Hit);
    }

    protected void Start()
    {
        StateMachine.Initialize(SkelectonStateEnum.Idle, this);
    }

    protected void Update()
    {
        
        StateMachine.CurrentState.UpdateState();
    }

    public override void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public override void Attack()
    {
        DamageCasterCompo.CastDamage(combo:0);
    }

    public override void Stun(float time)
    {
        if (isDead) return;

        stunDuration = time; 
        StateMachine.ChangeState(SkelectonStateEnum.Stun);
    }

    protected override void HandleDead(Vector2 direction)
    {
        //direction은 차후 쓸수도 있어서 받아둠.
        StateMachine.ChangeState(SkelectonStateEnum.Dead, true);
    }
}
