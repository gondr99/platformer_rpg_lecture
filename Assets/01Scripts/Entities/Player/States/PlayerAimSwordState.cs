using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    private bool _releaseKey;
    public PlayerAimSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
            
        _releaseKey = false;
        _player.PlayerInput.ThrowSwordEvent += OnThrowAim;
        SkillManager.Instance.GetSkill<SwordSkill>()?.OnThrowAim(true);
        
        SkillManager.Instance.GetSkill<SwordSkill>()?.DotsActive(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Vector2 mousePosition = CameraManager.Instance.MainCam.ScreenToWorldPoint(_player.PlayerInput.AimPosition);

        if ((_player.transform.position.x > mousePosition.x && _player.FacingDirection > 0) ||
            (_player.transform.position.x < mousePosition.x && _player.FacingDirection < 0))
        {
            _player.Flip();
        }
       
        
        if (_releaseKey && _triggerCalled)
        {
            //키도 눌렸고 애니메이션도 종료되었다면 Idle상태로 전환
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        SkillManager.Instance.GetSkill<SwordSkill>()?.DotsActive(false);
        _player.PlayerInput.ThrowSwordEvent -= OnThrowAim;
        base.Exit();
    }
    
    private void OnThrowAim(bool state)
    {
        
        //키를 놓는것만 체크한다.
        if (!state)
        {
            SkillManager.Instance.GetSkill<SwordSkill>()?.OnThrowAim(state); //소드 발사되도록
            _releaseKey = true;
            _player.AnimatorCompo.SetBool(_animBoolHash, false); //여기서 꺼줘야 던지는 애니메이션으로 감.
        }
    }
}
