using UnityEngine;

public abstract class Enemy : Entity
{

    [Header("Setting Values")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime; //전투시간을 초과하면 idle상태로 이동한다.

    private float _defaultMoveSpeed;

    [SerializeField] protected LayerMask _whatIsPlayer;
    [SerializeField] protected LayerMask _whatIsObstacle;

    [Header("Attack Values")]
    public float runAwayDistance;
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    
    protected int _lastAnimationBoolHash; //마지막으로 재생된 애니메이션 해시

    #region freeze variable

    protected bool _isFreeze = false; //얼어있는 상태
    protected bool _isFrozenWithoutTimer = false; //시간제한 없이 프리즈 시킬때

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _defaultMoveSpeed = moveSpeed;

    }

    public virtual void AssignLastAnimHash(int hashCode)
    {
        _lastAnimationBoolHash = hashCode;
    }

    public int GetLastAnimHash()
    {
        return _lastAnimationBoolHash;
    }

    //전방에 플레이어가 있는지 검사.
    public virtual RaycastHit2D IsPlayerDetected()
        => Physics2D.Raycast(_wallChecker.position, Vector2.right * FacingDirection, runAwayDistance, _whatIsPlayer);

    public virtual bool IsObstacleInLine(float distance)
    {
        return Physics2D.Raycast(_wallChecker.position, Vector2.right * FacingDirection, distance, _whatIsObstacle);
    }

    public abstract void AnimationFinishTrigger();

    public override void SlowEntityBy(float percent)
    {
        float slowedSpeed = moveSpeed * (1 - percent);
        if (moveSpeed <= slowedSpeed) return; //중복 적용 막아.
        moveSpeed = slowedSpeed;
        AnimatorCompo.speed = 1 - percent;
    }

    public override void ReturnDefaultSpeed()
    {
        AnimatorCompo.speed = 1f;
        moveSpeed = _defaultMoveSpeed;
    }

    #region counter attack region
    public virtual void OpenCounterAttackWindow()
    {
        _canBeStuned = true;
    }

    public virtual void CloseCounterAttackWindow()
    {
        _canBeStuned = false;
    }

    public virtual bool CanBeStunned()
    {
        if (_canBeStuned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }
    #endregion


    #region Freeze logic

    public virtual void FreezeTime(bool isFreeze, bool isFreezeWithoutTimer = false)
    {
        if(isFreezeWithoutTimer)
            _isFrozenWithoutTimer = isFreezeWithoutTimer;
        
        _isFreeze = isFreeze;
        if (_isFreeze)
        {
            moveSpeed = 0;
            AnimatorCompo.speed = 0;
        }
        else
        {
            moveSpeed = _defaultMoveSpeed;
            AnimatorCompo.speed = 1;
            _isFrozenWithoutTimer = false;
        }
    }

    public virtual void FreezeTimeFor(float freezeTime)
    {
        FreezeTime(true); //freeze enemy
        StartDelayCallback(freezeTime, () =>
        {
            if (!_isFrozenWithoutTimer)
            {
                FreezeTime(false); //unfreeze when not perminant freezing state
            }
        });
    }

    #endregion
    
#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDirection, transform.position.y));
        Gizmos.color = Color.white;
    }
#endif
}
