
using System;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
        _player.PlayerInput.PrimaryAttackEvent += HandlePrimaryAttackEvent;
        _player.PlayerInput.CounterAttackEvent += HandleCounterAttackEvent;
        _player.PlayerInput.ThrowSwordEvent += HandleOnThrowSwordEvent;
    }

    public override void Exit()
    {
        _player.PlayerInput.JumpEvent -= HandleJumpEvent;
        _player.PlayerInput.PrimaryAttackEvent -= HandlePrimaryAttackEvent;
        _player.PlayerInput.CounterAttackEvent -= HandleCounterAttackEvent;
        _player.PlayerInput.ThrowSwordEvent -= HandleOnThrowSwordEvent;
        base.Exit();
    }
    

    public override void UpdateState()
    {
        base.UpdateState();
        if (!_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }

    #region handling input section
    private void HandleJumpEvent()
    {
        if(_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
        }
    }

    private void HandlePrimaryAttackEvent()
    {
        if(_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.PrimaryAttack);
        }
    }

    private void HandleCounterAttackEvent()
    {
        _stateMachine.ChangeState(PlayerStateEnum.CounterAttack);
    }
    
    private void HandleOnThrowSwordEvent(bool state)
    {
        //이미 칼을 던진상태면 더이상 진행안함.
        SwordSkill swordSkill = SkillManager.Instance.GetSkill<SwordSkill>();
        if (swordSkill == null || swordSkill.skillEnabled == false) 
            return;
        
        bool hasSwordAlready = swordSkill.generatedSword != null;
        if (state && !hasSwordAlready)
        {
            _stateMachine.ChangeState(PlayerStateEnum.AimSword);
        }
        else if (state)
        {
            swordSkill.ReturnGenerateSword();
        }
    }

    #endregion

}
