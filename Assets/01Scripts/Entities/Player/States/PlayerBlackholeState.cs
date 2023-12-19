using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float _flyTime = 0.25f;
    private float _startTimer;

    private float _originalGravityScale;
    private BlackholeSkill _skill;

    public PlayerBlackholeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if (_player.IsGroundDetected())
            _startTimer = _flyTime;
        else
            _startTimer = 0.05f; //짧게 올라간다.
        
        _originalGravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
        _player.CanStateChangeable = false; //상태를 변경하지 못하도록 하고

        _skill = SkillManager.Instance.GetSkill<BlackholeSkill>();

        _skill.isReadyToAttack = false;
        _skill.SkillEffectEnd += OnSkillEffectEnd;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _startTimer -= Time.deltaTime;
        if (_startTimer > 0)
        {
            _player.SetVelocity(0, 15f, true); //위로 떠오르고
        }

        if (_startTimer <= 0)
        {
            _player.StopImmediately(true);
            if (!_skill.isReadyToAttack)
            {
                _skill.BlackholeFieldOpen(_player.transform.position); //블랙홀 필드를 열고 스킬 사용상태로 만들어준다.
                _skill.isReadyToAttack = true; 
            }
        }
    }

    public override void Exit()
    {
        _skill.SkillEffectEnd -= OnSkillEffectEnd;
        _rigidbody.gravityScale = _originalGravityScale;
        _skill.isReadyToAttack = false; 
        base.Exit();
    }

    private void OnSkillEffectEnd()
    {
        _player.CanStateChangeable = true; //상태변경 가능하도록
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
