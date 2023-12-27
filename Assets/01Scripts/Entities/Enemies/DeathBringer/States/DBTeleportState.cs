using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBTeleportState : EnemyState<DeathBringerStateEnum>
{
    public enum TeleportState
    {
        Hide,
        HideEnd,
        Show
    }
    protected TeleportState _state;

    protected EnemyDeathBringer _deathBringer;

    protected int _hashShowTrigger = Animator.StringToHash("ShowTrigger");
    
    public DBTeleportState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _deathBringer = enemyBase as EnemyDeathBringer;
    }

    public override void Enter()
    {
        base.Enter();
        _state = TeleportState.Hide;
        _enemyBase.SpriteRendererCompo.DOFade(0, 0.8f);
        _enemyBase.StartDelayCallback(1f, () => _state = TeleportState.HideEnd);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(_state == TeleportState.HideEnd)
        {
            if(GetPlayerBehindPosition(out Vector3 position))
            {
                _enemyBase.transform.position = position;
                _state = TeleportState.Show;
                _enemyBase.AnimatorCompo.SetTrigger(_hashShowTrigger);
                _enemyBase.SpriteRendererCompo.DOFade(1, 0.8f);
            }
        }

        if(_state == TeleportState.Show && _triggerCalled)
        {
            _stateMachine.ChangeState(DeathBringerStateEnum.Move);
        }
    }

    private bool GetPlayerBehindPosition(out Vector3 position)
    {
        Player player = PlayerManager.Instance.Player;
        int playerFacing = player.FacingDirection;
        Vector3 playerPosition = player.transform.position;

        float offset = 2.2f;
        
        position = playerPosition + new Vector3(playerFacing * -offset, 0, 0);
        RaycastHit2D hit = _deathBringer.GroundBelow(position);
        
        if(!_deathBringer.ObstacleCheck(position) && hit.collider != null) //뒤쪽에 장애물이 없고 땅이 있다면
        {
            return true;
        }

        position = playerPosition + new Vector3(playerFacing * offset, 0, 0);
        hit = _deathBringer.GroundBelow(position);
        if (!_deathBringer.ObstacleCheck(position) && hit.collider != null) //뒤쪽에 장애물이 없다면
        {
            return true;
        }

        return false;
    }

    private bool GetRandomPositionOnGround(out Vector3 point)
    {
        Bounds bounds = _deathBringer.boundBox.bounds;
        float safeOffset = 2f;

        bool find = false;
        point = Vector3.zero;

        for(int i = 0; i < 30; ++i)
        {
            float x = Random.Range(bounds.min.x + safeOffset, bounds.max.x - safeOffset);
            float y = Random.Range(bounds.min.y + safeOffset, bounds.max.y - safeOffset);

            Vector2 tempPosition = new Vector2(x, y);
            RaycastHit2D hit = _deathBringer.GroundBelow(tempPosition);
            if(hit.collider)
            {
                point = hit.point;
                if(!_deathBringer.ObstacleCheck(tempPosition))
                {
                    find = true;
                    break;
                }
            }
        }
        
        return find;
    }

    
}
