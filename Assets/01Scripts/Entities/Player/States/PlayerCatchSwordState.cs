
public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        SwordSkill skill = SkillManager.Instance.GetSkill<SwordSkill>();
        SwordSkillController sword = skill.generatedSword;
        
        if ((_player.transform.position.x > sword.transform.position.x && _player.FacingDirection > 0) ||
            (_player.transform.position.x < sword.transform.position.x && _player.FacingDirection < 0))
        {
            _player.Flip();
        }
        
        _player.SetVelocity(skill.returnImpactPower * - _player.FacingDirection, _rigidbody.velocity.y, true);
        
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
