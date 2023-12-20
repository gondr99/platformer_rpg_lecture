using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public int maxHealth;
    [SerializeField] private int _currentHealth;


    public Action OnHit;
    public Action<Vector2> OnDead;
    public Action<Vector2> OnKnockBack;

    public AilmentStat ailmentStat;
    
    private Entity _owner;

    #region infomation for feedback

    public bool isLastHitCritical = false; 
    public Vector2 lastAttackDirection; 
    public bool isHitByMelee;

    #endregion
    
    //상태이상 아이콘을 띄워주기 위한 이벤트
    public UnityEvent<Ailment, Ailment> OnAilmentChanged;

    private void Awake()
    {
        ailmentStat = new AilmentStat();
        ailmentStat.AilmentChangeEvent += HandAilmentChangeEvent;
        ailmentStat.DotDamageEvent += HandleDotDamageEvent;
    }

    private void OnDestroy()
    {
        ailmentStat.AilmentChangeEvent -= HandAilmentChangeEvent;
        ailmentStat.DotDamageEvent -= HandleDotDamageEvent;
    }

    //도트데미지 관리
    private void HandleDotDamageEvent(Ailment ailmentType, int damage)
    {
        Debug.Log($"{ailmentType.ToString()} dot damaged : {damage}");
        OnHit?.Invoke();
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        AfterHitFeedbacks(Vector2.zero);
    }

    //상태이상 변경관리
    private void HandAilmentChangeEvent(Ailment oldAilment, Ailment newAilment)
    {
        OnAilmentChanged?.Invoke(oldAilment, newAilment);
    }
    
    //상태이상 걸기.
    public void SetAilment(Ailment ailment, float duration, int damage)
    {
        ailmentStat.ApplyAilments(ailment, duration, damage);
    }
    
    protected void Update()
    {
        ailmentStat.UpdateAilment(); //질병 업데이트
    }

    public void SetOwner(Entity owner)
    {
        _owner = owner;
        _currentHealth = maxHealth;
    }

    public bool ApplyDamage(int damage, Vector2 attackDirection, Vector2 knockbackPower, Entity dealer)
    {
        if (_owner.isDead) return true; // if dead then return

        if (_owner.Stat.CanEvasion())
        {
            Debug.Log($"{_owner.gameObject.name} is evasion attack!");
            return false;
        }
        
        if (dealer.Stat.IsCritical(ref damage))
        {
            Debug.Log($"Critical! : {damage}"); //데미지 증뎀되었음.
            isLastHitCritical = true;
        }
        else
        {
            isLastHitCritical = false;
        }
        
        damage = _owner.Stat.ArmoredDamage(damage); 
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        
        isHitByMelee = true;
        lastAttackDirection = (transform.position - dealer.transform.position).normalized;

        knockbackPower.x *= attackDirection.x; //y value stay to default
        return AfterHitFeedbacks(knockbackPower);
    }
    public void ApplyMagicDamage(int damage, Vector2 attackDirection, Vector2 knockbackPower, Player player)
    {
        int magicDamage = _owner.Stat.GetMagicDamageAfterResist(damage);
        _currentHealth = Mathf.Clamp(_currentHealth - magicDamage, 0, maxHealth);

        isHitByMelee = false;
        knockbackPower.x *= attackDirection.x; //y값은 고정으로.
        AfterHitFeedbacks(knockbackPower);
    }

    private bool AfterHitFeedbacks(Vector2 knockbackPower)
    {
        OnHit?.Invoke();
        OnKnockBack?.Invoke(knockbackPower);

        if (_currentHealth <= 0 )
        {
            OnDead?.Invoke(knockbackPower);
            return true;
        }

        return false;
    }
}
