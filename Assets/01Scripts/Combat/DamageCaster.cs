using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    public Transform attackChecker;
    public float attackCheckRadius;

    public Vector2[] knockbackPower;

    [SerializeField] private int _maxHitCount = 5; //최대로 때릴 수 있는 적 갯수
    public LayerMask whatIsEnemy;
    private Collider2D[] _hitResult;

    private Entity _owner;
    
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

        //논얼로케이터는 이렇게 쓰는거다!
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(whatIsEnemy);
        int cnt = Physics2D.OverlapCircle(attackChecker.position, 
            attackCheckRadius, 
            filter, 
            _hitResult);


        for (int i = 0; i < cnt; ++i)
        {
            //피격방향구하고
            Vector2 direction = (_hitResult[i].transform.position - transform.position).normalized;
            //실제 데미지 적용해야함.
            if (_hitResult[i].TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = 5; //나중에 스탯을 통해 뽑아야와하지만 현재 없으니 이렇게


                Vector2 power = knockbackPower[combo];
                health.ApplyDamage(damage, direction, power, _owner);
            }
        }

        return cnt > 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackChecker != null)
            Gizmos.DrawWireSphere(attackChecker.position, attackCheckRadius);
    }
#endif
}
