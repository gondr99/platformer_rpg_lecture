using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private CrystalSkill _skill;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private float _lifeTime;
    
    private bool _isDestroyed = false;
    private Transform _closestTarget = null;
    private bool _isLaunched = false;
    
    private readonly int _hashExplodeTrigger = Animator.StringToHash("Explode");
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    public void SetupCrystal(CrystalSkill crystalSkill)
    {
        _skill = crystalSkill;

        if (crystalSkill.crystalType == CrystalType.Multiple)
        {
            _lifeTime = _skill.timeOut * 5; //5배 시간    
        }
        else
        {
            _lifeTime = _skill.timeOut;
        }
        _isDestroyed = false;
    }
    
    public void LaunchToTarget(Transform targetTrm)
    {
        _closestTarget = targetTrm;
        transform.DOKill(); //모든 트윈 제거.
        transform.DOLocalMoveY(2f, 0.3f).OnComplete(() =>
        {
            _isLaunched = true;
            transform.parent = null;
        });
    }
    public void StartPulseMove()
    {
        Vector3 pos = transform.localPosition;
        transform.DOLocalMoveY(pos.y + 0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0 && !_isDestroyed)
        {
            EndOfCrystal();
            return;
        }

        //가장 가까운 적을 찾아둔다.
        if (_closestTarget == null)
        {
            _closestTarget = _skill.FindClosestEnemy(transform, _skill.findEnemyRadius);
        }

        if (_skill.crystalType == CrystalType.Moving && _closestTarget != null)
        {
            ChaseToTarget();
        }

        if (_skill.crystalType == CrystalType.Multiple && _isLaunched)
        {
            ChaseToTarget(3f); //3배속
        }
    }

    private void ChaseToTarget(float speedMultiplier = 1f)
    {
        if (_closestTarget == null)
        {
            EndOfCrystal();
            return;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, _closestTarget.position,
            _skill.moveSpeed * speedMultiplier * Time.deltaTime);

        if (_isDestroyed) return;
        
        if (Vector2.Distance(transform.position, _closestTarget.position) < 1f)
        {
            EndOfCrystal();
        }
    }

    public void EndOfCrystal()
    {
        _isDestroyed = true;

        if (_skill.canExplode) //폭발성이면 터지도록
        {
            Vector3 endValue = Vector3.one * 2.5f;
            transform.DOScale(endValue, 0.03f);
            _animator.SetTrigger(_hashExplodeTrigger);
        }
        else
        {
            DestroySelf();
        }
    }
    
    private void DestroySelf(float time = 0.5f)
    {
        _skill.UnlinkCrystal();
        _spriteRenderer.DOFade(0f, time).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    
    private void EndOfExplosionAnimation()
    {
        transform.DOKill();
        DestroySelf(0.1f);
    }
    
    private void AnimationExplodeEvent()
    {
        List<Enemy> enemies = _skill.FindEnemiesInRange(transform, _skill.explosionRadius);

        Player player = PlayerManager.Instance.Player;
        int damage = player.PStat.GetMagicDamage(StatType.IceDamage) + player.PStat.GetMagicDamage(StatType.LightingDamage); 
        foreach (Enemy enemy in enemies) 
        {
            Vector2 dir = enemy.transform.position - transform.position;
            //배율에 따라 증가된 값으로 데미지
            int calculatedDamage = Mathf.RoundToInt(damage * _skill.damageMultiplier);
            enemy.HealthCompo.ApplyMagicDamage(calculatedDamage, dir.normalized, _skill.knockbackPower, player);
        }
    }
    
#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        
        if (_skill != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _skill.explosionRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}
