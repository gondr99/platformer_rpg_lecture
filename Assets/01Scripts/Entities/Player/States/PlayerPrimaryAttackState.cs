using UnityEngine;


public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastTimeAttacked;
    private float _comboWindow = 0.8f; //콤보가 이어지는 시간
    private Coroutine _delayCoroutine; //콤보 코루틴

    private readonly int _comboCountHash = Animator.StringToHash("ComboCounter");
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //마지막 공격으로부터 _comboWindow만큼 시간이 흘렀다면 다시 0번 콤보부터 이어서.
        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCountHash, _comboCounter);

        _player.currentComboCounter = _comboCounter;
        //애니메이션 속도를 조절
        _player.AnimatorCompo.speed = _player.attackSpeed;

        float attackDirection = _player.FacingDirection;
        float xInput = _player.PlayerInput.XInput;
        if(Mathf.Abs(xInput) > 0.05f)
        {
            attackDirection = Mathf.Sign(xInput);
        }

        Vector2 movement = _player.attackMovement[_comboCounter];
        _player.SetVelocity(movement.x * attackDirection, movement.y);

        //0.1초후 모션정지
        _delayCoroutine = _player.StartDelayCallback(0.1f, () => _player.StopImmediately(false));
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_triggerCalled)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    public override void Exit()
    {
        ++_comboCounter;
        _lastTimeAttacked = Time.time;
        //애니메이션 속도 원상복귀.
        _player.AnimatorCompo.speed = 1;

        _player.StopCoroutine(_delayCoroutine); //피격등을 이유로 상태변화시를 위해.
        base.Exit();
    }

}
