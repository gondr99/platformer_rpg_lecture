using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }

    private void AnimationFinishTrigger()
    {
        _enemy.AnimationFinishTrigger();
    }

    private void AttackAnimationTrigger()
    {
        _enemy.Attack();
    }

    private void CounterAttackTrigger() => _enemy.OpenCounterAttackWindow();
    private void CounterAttackEndTrigger() => _enemy.CloseCounterAttackWindow();
}
