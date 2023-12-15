using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    public Transform attackChecker;
    public float attackCheckRadius;

    public Vector2[] knockbackPower;

    [SerializeField] private int _maxHitCount = 5; //�ִ�� ���� �� �ִ� �� ����
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

        //���������ʹ� �̷��� ���°Ŵ�!
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(whatIsEnemy);
        int cnt = Physics2D.OverlapCircle(attackChecker.position, 
            attackCheckRadius, 
            filter, 
            _hitResult);


        for (int i = 0; i < cnt; ++i)
        {
            //�ǰݹ��ⱸ�ϰ�
            Vector2 direction = (_hitResult[i].transform.position - transform.position).normalized;
            //���� ������ �����ؾ���.
            if (_hitResult[i].TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = 5; //���߿� ������ ���� �̾ƾ߿������� ���� ������ �̷���


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
