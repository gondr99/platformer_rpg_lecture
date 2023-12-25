using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour, IDamageable
{
    public int maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private Vector3 _popupTextOffset;

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
        Vector3 position = _owner.transform.position + _popupTextOffset;
        PopupTextManager.Instance.PopupDamageText(position, damage.ToString(), DamageCategory.Debuff);
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        AfterHitFeedbacks(Vector2.zero, false);
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
        _currentHealth = maxHealth = owner.Stat.GetMaxHealth();
    }

    public bool ApplyDamage(int damage, Vector2 attackDirection, Vector2 knockbackPower, Entity dealer)
    {
        if (_owner.isDead) return true; // if dead then return

        Vector3 position = _owner.transform.position + _popupTextOffset;

        if (_owner.Stat.CanEvasion())
        {
            PopupTextManager.Instance.PopupDamageText(position, "Evasion", DamageCategory.Debuff);
            return false;
        }
        
        if (dealer.Stat.IsCritical(ref damage))
        {
            isLastHitCritical = true;
        }
        else
        {
            isLastHitCritical = false;
        }
        
        damage = _owner.Stat.ArmoredDamage(damage); 
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        PopupTextManager.Instance.PopupDamageText(position, damage.ToString(), isLastHitCritical ? DamageCategory.Critical : DamageCategory.Normal);


        isHitByMelee = true;
        lastAttackDirection = (transform.position - dealer.transform.position).normalized;

        CheckAdditionalDamage(damage); //추가데미지 계산
        
        knockbackPower.x *= attackDirection.x; //y value stay to default
        return AfterHitFeedbacks(knockbackPower);
    }
    
    
    public void ApplyMagicDamage(int damage, Vector2 attackDirection, Vector2 knockbackPower, Player player)
    {
        Vector3 position = _owner.transform.position + _popupTextOffset;

        int magicDamage = _owner.Stat.GetMagicDamageAfterResist(damage);
        _currentHealth = Mathf.Clamp(_currentHealth - magicDamage, 0, maxHealth);

        PopupTextManager.Instance.PopupDamageText(position, magicDamage.ToString(), DamageCategory.Normal);

        isHitByMelee = false;
        knockbackPower.x *= attackDirection.x; //y값은 고정으로.
        AfterHitFeedbacks(knockbackPower);
    }

    private bool AfterHitFeedbacks(Vector2 knockbackPower, bool withFeedback = true)
    {
        if (withFeedback)
        {
            OnHit?.Invoke();
            OnKnockBack?.Invoke(knockbackPower);
        }

        if (_currentHealth <= 0 )
        {
            OnDead?.Invoke(knockbackPower);
            return true;
        }

        return false;
    }

    //쇼크시 추가데미지 계산
    private void CheckAdditionalDamage(int damage)
    {
        //감점시 10% 추뎀
        if (ailmentStat.HasAilment(Ailment.Shocked))
        {
            Vector3 position = _owner.transform.position + _popupTextOffset;

            int shockDamage = Mathf.Min(1, Mathf.RoundToInt(damage * 0.1f));
            _currentHealth = Mathf.Clamp(_currentHealth - shockDamage, 0, maxHealth);
            PopupTextManager.Instance.PopupDamageText(position, shockDamage.ToString(), DamageCategory.Debuff);
            //Debug.Log($"{gameObject.name} : shocked damage added = {shockDamage}");
        }
    }

    public float GetNormalizedHealth()
    {
        if (maxHealth <= 0) return 0;
        return Mathf.Clamp((float)_currentHealth / maxHealth, 0, 1f);
    }
}
