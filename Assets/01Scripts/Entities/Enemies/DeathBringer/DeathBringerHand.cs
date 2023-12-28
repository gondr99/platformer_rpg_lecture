using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DeathBringerHand : MonoBehaviour
{
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    private EnemyDeathBringer _enemy;
    private readonly int _hashAttackTrigger = Animator.StringToHash("Attack");
    public void SetUp(EnemyDeathBringer enemy, float damageMultiplier, float delay)
    {
        _enemy = enemy;
        _damageCaster.SetOwner(enemy);
        _damageCaster.SetDamageMultiplier(damageMultiplier);
        StartCoroutine(DelayAttack(delay));
    }

    private IEnumerator DelayAttack(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetTrigger(_hashAttackTrigger);
    }
    
    public void Attack()
    {
        _damageCaster.CastDamage(0);
    }

    public void EndOfAnimation()
    {
        _spriteRenderer.DOFade(0, 0.7f).OnComplete(() => Destroy(gameObject));
    }
}
