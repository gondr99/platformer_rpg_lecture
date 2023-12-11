using System;
using UnityEngine;

public class Player : Entity
{
    [Header("Setting values")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    private float _defaultMoveSpeed;
    private float _defaultJumpForce;
    private float _defaultDashSpeed;

    [Header("Attack Settings")]
    public float attackSpeed = 1f;
    public float counterAttackDuration = 0.2f;
    [HideInInspector] public int currentComboCounter = 0;



    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] private InputReader _inputReader;
    public InputReader PlayerInput => _inputReader; 


    public bool CanStateChangeable { get; private set; } = true;

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum state in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = state.ToString();
            Type t = Type.GetType($"Player{typeName}State");
            var playerState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

            StateMachine.AddState(state, playerState);
            try
            {
                //Type t = Type.GetType($"Player{typeName}State");
                //var playerState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

                //StateMachine.AddState(state, playerState);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                Debug.LogError($"{typeName} is loading error, Message : {ex.Message}");
            }
        }
    }

    #region input event Handling section
    private void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
    }

    private void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
    }

    private void HandleDashEvent()
    {
        StateMachine.ChangeState(PlayerStateEnum.Dash);
    }
    #endregion

    protected void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
        
        //save default value for ailment
        _defaultMoveSpeed = moveSpeed;
        _defaultJumpForce = jumpForce;
        _defaultDashSpeed = dashSpeed;
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public override void Attack()
    {
        
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    
}
