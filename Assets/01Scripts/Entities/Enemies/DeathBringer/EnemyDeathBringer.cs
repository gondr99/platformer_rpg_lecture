using UnityEngine;

public enum DeathBringerStateEnum
{
    Idle,
    Move,
    Teleport,
    Attack,
    Stun,
    Dead
}
public class EnemyDeathBringer : Enemy
{
    public override void AnimationFinishTrigger()
    {
        
    }

    public override void Attack()
    {
        
    }

    public override void Stun(float time)
    {
        
    }

    protected override void HandleDead(Vector2 direction)
    {
        
    }
}
