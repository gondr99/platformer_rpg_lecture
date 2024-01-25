
using UnityEngine;

public class DBCastState : EnemyState<DeathBringerStateEnum>
{
    private int _currentHandCount = 0;
    private int _totalHandCount = 10;
    private float _handGenerateTerm = 1f;
    private float _attackDelay = 0.5f;
    private float _lastHandTime;

    private EnemyDeathBringer _deathBringer;
    private Player _player;
    private bool _createHandComplete = false;
    private readonly int _hashCastEndTrigger = Animator.StringToHash("CastEndTrigger");
    public DBCastState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _deathBringer = enemyBase as EnemyDeathBringer;
    }
    public override void Enter()
    {
        base.Enter();
        _currentHandCount = 0;
        _deathBringer.stunCount = 0; //다음에 다시 안들어오도록
        _lastHandTime = Time.time;
        _createHandComplete = false;
        _player = PlayerManager.Instance.Player;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_lastHandTime + _handGenerateTerm <= Time.time && _currentHandCount <= _totalHandCount)
        {
            CreateHand();
            _lastHandTime = Time.time;
            ++ _currentHandCount;
        }

        
        if (_currentHandCount > _totalHandCount && !_createHandComplete)
        {
            _createHandComplete = true;
            _enemyBase.AnimatorCompo.SetTrigger(_hashCastEndTrigger);
        }

        if (_triggerCalled)
        {
            //enemy change to move state after create hand
            _enemyBase.StartDelayCallback(1f, () => _stateMachine.ChangeState(DeathBringerStateEnum.Move));
        }
    }

    private void CreateHand()
    {
        Vector3 pos = _player.transform.position + new Vector3(0, -1f, 0);
        _deathBringer.CreateHandAtPosition(pos, 0.8f + _currentHandCount * 0.1f, _attackDelay - _currentHandCount * 0.05f);
    }


    public override void Exit()
    {
        base.Exit();
    }
}
