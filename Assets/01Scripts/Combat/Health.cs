using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public int maxHealth;
    [SerializeField] private int _currentHealth;


    public Action OnHit;
    public Action<Vector2> OnDead;
    public Action<Vector2> OnKnockBack;

    private Entity _owner;

    #region infomation for feedback

    public bool isLastHitCritical = false; 
    public Vector2 lastAttackDirection; 
    public bool isHitByMelee;

    #endregion

    public void SetOwner(Entity owner)
    {
        _owner = owner;
        _currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage, Vector2 attackDirection, Vector2 knockbackPower, Entity dealer)
    {
        if (_owner.isDead) return; //나중에 무적로직도 추가

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        isHitByMelee = true;
        lastAttackDirection = (transform.position - dealer.transform.position).normalized;

        knockbackPower.x *= attackDirection.x; //y값은 고정으로.
        AfterHitFeedbacks(knockbackPower);
    }

    private void AfterHitFeedbacks(Vector2 knockbackPower)
    {
        OnHit?.Invoke();
        OnKnockBack?.Invoke(knockbackPower);

        if (_currentHealth <= 0 )
        {
            _owner.isDead = true;
            OnDead?.Invoke(knockbackPower);
        }
    }
}
