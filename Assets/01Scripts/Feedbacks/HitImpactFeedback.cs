using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitImpactFeedback : MonoBehaviour, IFeedback
{
    [ColorUsage(showAlpha: true, hdr:true)]
    [SerializeField] private Color _effectColr;

    [SerializeField] private Health _targetHealth;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _scaleMinMax;
    [SerializeField] private Vector2 _criticalScaleMinMax;
    public void CreateFeedback()
    {
        if (!_targetHealth.isHitByMelee) return; //밀리 공격에 대해서만.
        
        float x = Random.Range(-_offset.x, _offset.x);
        float y = Random.Range(-_offset.y, _offset.y);
        Vector3 offset = new Vector3(x, y);
        Vector3 position = _targetHealth.transform.position + offset;
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        
        
        AnimatorEffect effect = PoolManager.Instance.Pop(PoolingType.HitImpactVFX) as AnimatorEffect;
        effect.SetEffectColor(_effectColr);
        effect.PlayAnimation(position, rot, Vector3.one * Random.Range(_scaleMinMax.x, _scaleMinMax.y));

        if (_targetHealth.isLastHitCritical)
        {
            AnimatorEffect slashEffect = PoolManager.Instance.Pop(PoolingType.SlashVFX) as AnimatorEffect;
            slashEffect.SetEffectColor(_effectColr);
            Vector2 direction = _targetHealth.lastAttackDirection;
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion towardRot = Quaternion.Euler(0, 0, zRotation); 
            slashEffect.PlayAnimation(position, towardRot, Vector3.one * Random.Range(_criticalScaleMinMax.x, _criticalScaleMinMax.y));
        }
    }

    public void CompleteFeedback()
    {
        
    }
}
