using UnityEngine;

public interface IDamageable
{
    public bool ApplyDamage(int damage, Vector2 attackDirection, Vector2 knockbackPower, Entity dealer);

    
}
