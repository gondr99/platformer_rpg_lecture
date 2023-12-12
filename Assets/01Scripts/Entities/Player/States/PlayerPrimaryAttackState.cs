using UnityEngine;


public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastTimeAttacked;
    private float _comboWindow = 0.8f; //�޺��� �̾����� �ð�
    private Coroutine _delayCoroutine; //�޺� �ڷ�ƾ

    private readonly int _comboCountHash = Animator.StringToHash("ComboCounter");
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //������ �������κ��� _comboWindow��ŭ �ð��� �귶�ٸ� �ٽ� 0�� �޺����� �̾.
        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCountHash, _comboCounter);

        _player.currentComboCounter = _comboCounter;
        //�ִϸ��̼� �ӵ��� ����
        _player.AnimatorCompo.speed = _player.attackSpeed;

        float attackDirection = _player.FacingDirection;
        float xInput = _player.PlayerInput.XInput;
        if(Mathf.Abs(xInput) > 0.05f)
        {
            attackDirection = Mathf.Sign(xInput);
        }

        Vector2 movement = _player.attackMovement[_comboCounter];
        _player.SetVelocity(movement.x * attackDirection, movement.y);

        //0.1���� �������
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
        //�ִϸ��̼� �ӵ� ���󺹱�.
        _player.AnimatorCompo.speed = 1;

        _player.StopCoroutine(_delayCoroutine); //�ǰݵ��� ������ ���º�ȭ�ø� ����.
        base.Exit();
    }

}
