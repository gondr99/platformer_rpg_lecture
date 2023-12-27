using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DeathBringerStateEnum
{
    Idle,
    Move,
    Teleport,
    Attack,
    Stun,
    Dead,
    Cast
}
public class EnemyDeathBringer : Enemy
{

    public EnemyStateMachine<DeathBringerStateEnum> StateMachine { get; protected set; }

    public Vector2 sizeBox;
    public BoxCollider2D boundBox;
    
    [SerializeField] protected float _counterStunTime;
    public int stunCount = 0;

    protected override void Awake()
    {
        base.Awake();
        FacingDirection = -1;
        StateMachine = new EnemyStateMachine<DeathBringerStateEnum>(); //state machine make by generic

        foreach (DeathBringerStateEnum state in Enum.GetValues(typeof(DeathBringerStateEnum)))
        {
            string typeName = state.ToString();
            Type t = Type.GetType($"DB{typeName}State");

            if (t != null)
            {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<DeathBringerStateEnum>;
                StateMachine.AddState(state, enemyState);
            }
            else
            {
                Debug.LogError($"Enemy DeathBringer : no state [ {typeName} ]");
            }
        }
    }

    protected void Start()
    {
        StateMachine.Initialize(DeathBringerStateEnum.Idle, this);
    }

    protected void Update()
    {

        StateMachine.CurrentState.UpdateState();

        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            StateMachine.ChangeState(DeathBringerStateEnum.Teleport);
        }
    }

    public override void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }


    public override void Attack()
    {
        DamageCasterCompo.CastDamage(0);
    }

    public override void Stun(float time)
    {
        //이녀석은 스턴시간 무시하고 자신만의 스턴시간을 가진다.
        if (isDead) return;

        stunDuration = _counterStunTime;
        StateMachine.ChangeState(DeathBringerStateEnum.Stun);
    }

    protected override void HandleDead(Vector2 direction)
    {
        
    }

    public RaycastHit2D GroundBelow(Vector3 position)
    {
        return Physics2D.Raycast(position, Vector2.down, 2f, _whatIsGroundAndWall);
    }

    public bool ObstacleCheck(Vector3 position)
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)position + new Vector2(0, sizeBox.y * 0.5f), sizeBox, 0, _whatIsGroundAndWall);
        return collider != null;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Vector3 offset = new Vector3(0, sizeBox.y * 0.5f);
        Gizmos.DrawWireCube(transform.position + offset, sizeBox);
        Gizmos.color = Color.white;
    }
#endif
}
