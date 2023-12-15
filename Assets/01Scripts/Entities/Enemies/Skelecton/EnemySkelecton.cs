using System;
using UnityEngine;

public enum SkelectonStateEnum
{
    Idle,
    Move,
    Battle,
    Attack,
    Hit
}

public class EnemySkelecton : Enemy
{
    public EnemyStateMachine<SkelectonStateEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<SkelectonStateEnum>(); //상태머신 만들고

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
        StateMachine.Initialize(SkelectonStateEnum.Idle);
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

    }

}
