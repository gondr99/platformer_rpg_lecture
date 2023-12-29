using System;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    public Transform attackChecker;
    public float attackCheckRadius;

    public Vector2[] knockbackPower;

    [SerializeField] private int _maxHitCount = 5; 
    public LayerMask whatIsEnemy;
    private Collider2D[] _hitResult;

    [SerializeField] private float _damageMultiplier = 1f;
    private Entity _owner;

    public Action OnCriticalHitSuccess;

    private void Awake()
    {
        _hitResult = new Collider2D[_maxHitCount];
    }

    public void SetOwner(Entity owner)
    {
        _owner = owner;
    }

    public bool CastDamage(int combo)
    {

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(whatIsEnemy);
        int cnt = Physics2D.OverlapCircle(attackChecker.position, 
            attackCheckRadius, 
            filter, 
            _hitResult);


        for (int i = 0; i < cnt; ++i)
        {
            
            Vector2 direction = (_hitResult[i].transform.position - transform.position).normalized;
            
            if (_hitResult[i].TryGetComponent<Health>(out Health health))
            {

                int damage =  Mathf.RoundToInt(_owner.Stat.GetMeleeDamage() * _damageMultiplier);

                damage = CalculateDamage(damage); //계산증뎀
                Vector2 power = knockbackPower[combo];
                health.ApplyDamage(damage, direction, power, _owner);

                if(health.isLastHitCritical)
                {
                    OnCriticalHitSuccess?.Invoke();
                }
            }
        }

        return cnt > 0;
    }

    public void CastDamageWithStun(Enemy target, float multiplier, Vector2 attackDireciton, Vector2 stunPower, float stunTime)
    {
        int damage = Mathf.CeilToInt( _owner.Stat.GetMeleeDamage() * multiplier); 
        damage = CalculateDamage(damage);
        target.HealthCompo.ApplyDamage(damage, attackDireciton, stunPower, _owner);
        target.Stun(stunTime);
    }

    public void SetDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
    }
    
    private int CalculateDamage(int damage)
    {
        return Mathf.CeilToInt(damage * _damageMultiplier);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackChecker != null)
            Gizmos.DrawWireSphere(attackChecker.position, attackCheckRadius);
    }
#endif
}
