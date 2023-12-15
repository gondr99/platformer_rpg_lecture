using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [Header("Collision info")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected float _groundCheckBoxWidth;
    [SerializeField] protected LayerMask _whatIsGroundAndWall;
    [SerializeField] protected Transform _wallChecker;
    [SerializeField] protected float _wallCheckerDistance;

    #region components region
    public Animator AnimatorCompo { get; private set; } 
    public Rigidbody2D RigidbodyCompo { get; private set; }
    public SpriteRenderer SpriteRendererCompo { get; private set; }
    public CapsuleCollider2D Collder2DCompo { get; private set; }
    public DamageCaster DamageCasterCompo { get; private set; }
    public Health HealthCompo { get; private set; }
    #endregion

    public int FacingDirection { get; private set; } = 1; //오른쪽을 향하고 있을때 1
    public Action<int> OnFlip;

    [HideInInspector] public bool isDead;

    [Header("Knockback")]
    [HideInInspector] public bool isKnockbacked;
    [SerializeField] protected float _knockbackDuration;
    protected Coroutine _knockbackCoroutine = null;

    [Space]
    [Header("Feedback event")]
    public UnityEvent HitEvent;


    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        SpriteRendererCompo = visualTrm.GetComponent<SpriteRenderer>();
        RigidbodyCompo = GetComponent<Rigidbody2D>();
        Collder2DCompo = GetComponent<CapsuleCollider2D>();
        DamageCasterCompo = transform.Find("DamageCaster").GetComponent<DamageCaster>();
        HealthCompo = GetComponent<Health>();

        DamageCasterCompo.SetOwner(this);
        HealthCompo.SetOwner(this);

        HealthCompo.OnHit += HandleHitEvent;
        HealthCompo.OnKnockBack += HandleKnockbackEvent;
    }

    protected virtual void OnDestroy()
    {
        HealthCompo.OnHit -= HandleHitEvent;
        HealthCompo.OnKnockBack -= HandleKnockbackEvent;
    }

    #region handling event
    protected virtual void HandleHitEvent()
    {
        Debug.Log("asdads");
        HitEvent?.Invoke();
    }

    protected virtual void HandleKnockbackEvent(Vector2 knockbackPower)
    {
        if(_knockbackCoroutine != null)
        {
            StopCoroutine(_knockbackCoroutine);
        }

        isKnockbacked = true;
        SetVelocity(knockbackPower.x, knockbackPower.y, true);
        _knockbackCoroutine  = StartDelayCallback(_knockbackDuration, () => isKnockbacked = false);
    }


    #endregion

    #region Delay Callback Coroutine

    public Coroutine StartDelayCallback(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(delayTime, Callback));
    }

    protected IEnumerator DelayCoroutine(float delayTime, Action Callback)
    {
        yield return new WaitForSeconds(delayTime);
        Callback?.Invoke();
    }

    #endregion

    public abstract void Attack();

    #region Flip controlling

    public virtual void Flip()
    {
        FacingDirection = FacingDirection * -1;
        transform.Rotate(0, 180, 0); //180도 회전. 
        OnFlip?.Invoke(FacingDirection);
    }


    public virtual void FlipController(float x)
    {
        //반대방향으로 눌렀다면
        x = x > 0.05f ? 1 : x < -0.05f ? -1 : 0;
        if ( Mathf.Abs(FacingDirection + x) < 0.5f) 
        {
            Flip();
        }
    }

    #endregion

    #region velocity control

    public void SetVelocity(float x, float y, bool doNotFlip = false)
    {
        RigidbodyCompo.velocity = new Vector2(x, y);
        if (!doNotFlip)
            FlipController(x);
    }

    public void StopImmediately(bool withYAxis)
    {
        if (withYAxis)
            RigidbodyCompo.velocity = Vector2.zero;
        else
            RigidbodyCompo.velocity = new Vector2(0, RigidbodyCompo.velocity.y);
    }
    #endregion

    #region Check Collision
    public virtual bool IsGroundDetected()
    {
        //return Physics2D.Raycast(_groundChecker.position, Vector2.down, _groundCheckDistance, _whatIsGroundAndWall);
        return Physics2D.BoxCast(_groundChecker.position, new Vector2(_groundCheckBoxWidth, 0.05f),
                                0, Vector2.down, 
                                _groundCheckDistance, _whatIsGroundAndWall);
    }

    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(_wallChecker.position, Vector2.right * FacingDirection, _wallCheckerDistance, _whatIsGroundAndWall);

    #endregion

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_groundChecker != null)
        {
            Gizmos.DrawWireCube(_groundChecker.position - new Vector3(0, _groundCheckDistance *0.5f), new Vector3(_groundCheckBoxWidth, _groundCheckDistance, 1f));
        }
            //Gizmos.DrawLine(_groundChecker.position, _groundChecker.position + new Vector3(0, -_groundCheckDistance, 0));
        if (_wallChecker != null)
            Gizmos.DrawLine(_wallChecker.position, _wallChecker.position + new Vector3(_wallCheckerDistance, 0, 0));

        Gizmos.color = Color.white;
    }
#endif
}
