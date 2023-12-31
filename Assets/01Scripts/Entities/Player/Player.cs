using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Vector2[] attackMovement; //attack movement when combo attack
    public Transform backTrm; //후면에 따라다니는 오브젝트를 위한
    [HideInInspector] public int currentComboCounter = 0;

    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] private InputReader _inputReader;
    public InputReader PlayerInput => _inputReader;

    //player stat
    public PlayerStat PStat => _characterStat as PlayerStat;

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum state in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            
            string typeName = state.ToString();
            try
            {
                Type t = Type.GetType($"Player{typeName}State");
                var playerState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

                StateMachine.AddState(state, playerState);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} is loading error, Message :");
                Debug.Log(ex);
            }
        }

        DamageCasterCompo.OnCriticalHitSuccess += HandleCriticalHitSuccess;
    }

    private void HandleCriticalHitSuccess()
    {
        Inventory.Instance.UseItemEffect(EffectType.Critical);
    }

    #region input event Handling section
    private void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
        PlayerInput.UltiSkillEvent += HandleUltiSkillEvent;
        PlayerInput.CrystalSkillEvent += HandleCrystalSkillEvent;
    }

    private void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
        PlayerInput.UltiSkillEvent -= HandleUltiSkillEvent;
        PlayerInput.CrystalSkillEvent -= HandleCrystalSkillEvent;
    }

    private void HandleDashEvent()
    {
        if (IsWallDetected())
            return;

        if (SkillManager.Instance.GetSkill<DashSkill>().AttemptUseSkill())
        {
            StateMachine.ChangeState(PlayerStateEnum.Dash);
        }
    }
    
    private void HandleUltiSkillEvent()
    {
        BlackholeSkill skill = SkillManager.Instance.GetSkill<BlackholeSkill>(); 
        if (skill.AttemptUseSkill())
        {
            StateMachine.ChangeState(PlayerStateEnum.Blackhole);
        }else if (skill.isReadyToAttack)
        {
            skill.ReleaseAttack();
        }
    }

    private void HandleCrystalSkillEvent()
    {
        SkillManager.Instance.GetSkill<CrystalSkill>()?.AttemptUseSkill();
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

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PStat.IncreaseStatBy(10, 3f, PStat.GetStatByType(StatType.Damage));
        }
    }

    public override void Attack()
    {
        bool hitSuccess = DamageCasterCompo.CastDamage(currentComboCounter); 

        if (hitSuccess)
        {
            Inventory.Instance.UseItemEffect(EffectType.Melee);
            //hit success item effect must implementing here
            if(currentComboCounter == 2)
                SkillManager.Instance.GetSkill<ThunderStrikeSkill>().AttemptUseSkill();
        }
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public void FadePlayer(bool fadeOut, float sec)
    {
        float endValue = fadeOut ? 0 : 1f;
        SpriteRendererCompo.DOFade(endValue, sec);
    }
    
    public override void Stun(float time)
    {
        //currently do nothing here!
    }

    protected override void HandleDead(Vector2 direction)
    {
        StateMachine.ChangeState(PlayerStateEnum.Dead);
    }



    public override void SlowEntityBy(float percent)
    {
        float slowedSpeed = moveSpeed * (1 - percent);
        if (moveSpeed <= slowedSpeed) return; //이미 슬로우 상태면 추가 적용 없이 종료.
        moveSpeed = slowedSpeed;
        jumpForce *= 1 - percent;
        dashSpeed *= 1 - percent;
        AnimatorCompo.speed *= 1 - percent;
    }

    public override void ReturnDefaultSpeed()
    {
        moveSpeed = _defaultMoveSpeed;
        jumpForce = _defaultJumpForce;
        dashSpeed = _defaultDashSpeed;
        AnimatorCompo.speed = 1f;
    }
}
