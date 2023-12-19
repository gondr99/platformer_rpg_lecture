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

    private readonly int _hashExplodeTrigger = Animator.StringToHash("Explode");
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    public void SetupCrystal(CrystalSkill crystalSkill)
    {
        _skill = crystalSkill;
        _lifeTime = _skill.timeOut;
        _isDestroyed = false;
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0 && !_isDestroyed)
        {
            EndOfCrystal();
            return;
        }
    }

    public void EndOfCrystal()
    {
        _isDestroyed = true;
        
        DestroySelf();
    }
    
    private void DestroySelf(float time = 0.5f)
    {
        _skill.UnlinkCrystal();
        _spriteRenderer.DOFade(0f, time).OnComplete(() =>
        {
            Destroy(gameObject);
        });
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
