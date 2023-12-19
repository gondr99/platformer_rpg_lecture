using System.Collections.Generic;
using UnityEngine;

public delegate void CooldownInfoEvent(float current, float total);

public abstract class Skill : MonoBehaviour
{
    public bool skillEnabled = false;
    public LayerMask whatIsEnemy;
    [SerializeField] protected float _cooldown;
    [SerializeField] protected int _maxCheckEnemy = 5; //최대 5마리 중에서 가까운 적 선택

    protected float _cooldownTimer;
    protected Player _player;
    protected Collider2D[] _colliders;

    public bool IsCooldown => _cooldownTimer > 0f; 
    public event CooldownInfoEvent OnCooldownEvent;

    protected virtual void Start()
    {
        _player = PlayerManager.Instance.Player;
        _colliders = new Collider2D[_maxCheckEnemy];
    }

    protected virtual void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
            }

            OnCooldownEvent?.Invoke(_cooldownTimer, _cooldown);
        }
    }

    public virtual bool AttemptUseSkill()
    {
        if (_cooldownTimer <= 0 && skillEnabled)
        {
            _cooldownTimer = _cooldown;
            UseSkill(); //스킬을 사용하고
            return true;
        }
        Debug.Log("Skill cooldown or locked");
        return false;
    }

    public virtual void UseSkill()
    {
        //스킬을 쓸 때마다 해당 스킬을 썼음을 알려주는 피드백 필요.
    }

    public virtual void UseSkillWithoutCooltimeAndEffect()
    {
        //자동으로 발생되는 스킬들을 이용하기 위해 만든 함수.
    }

    public virtual Transform FindClosestEnemy(Transform checkTransform, float radius)
    {
        Transform closestEnemy = null;
        int cnt = Physics2D.OverlapCircle(checkTransform.position, radius, new ContactFilter2D { layerMask = whatIsEnemy, useLayerMask = true}, _colliders);

        float closestDistance = Mathf.Infinity;

        for(int i = 0; i < cnt; ++i)
        {
            if (_colliders[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                if(enemy.isDead) continue;
                float distanceToEnemy = Vector2.Distance(checkTransform.position, _colliders[i].transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = _colliders[i].transform;
                }
            }
        }
        
        return closestEnemy;
    }

    public List<Enemy> FindEnemiesInRange(Transform checkTransform, float radius)
    {
        int cnt = Physics2D.OverlapCircle(checkTransform.position, radius, new ContactFilter2D { layerMask = whatIsEnemy, useLayerMask = true}, _colliders);
        List<Enemy> list = new List<Enemy>();

        for (int i = 0; i < cnt; ++i)
        {
            if (_colliders[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                if(!enemy.isDead) //사망하지 않은 적만 리스트에.
                    list.Add(enemy);
            }
        }

        return list;
    }
}
