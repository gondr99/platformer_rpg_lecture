using System.Collections.Generic;
using UnityEngine;

public class EntityFXPlayer : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Transform _vfxPosition;

    [ColorUsage(showAlpha: true, hdr: true)] 
    [SerializeField] protected Color _chillColor, _igniteColor, _shockColor;
    protected Material _material;

    protected readonly int _hashIsEffect = Shader.PropertyToID("_IsEffect");
    protected readonly int _hashEffectColor = Shader.PropertyToID("_EffectColor");
    protected readonly int _hashEffectIntensity = Shader.PropertyToID("_EffectIntensity");


    protected Player _player;
    protected Dictionary<PoolingType, ParticleEffect> _effects;

    protected virtual void Awake()
    {
        _material = _spriteRenderer.material;
        _effects = new Dictionary<PoolingType, ParticleEffect>();
    }

    protected virtual void Start()
    {
        _player = PlayerManager.Instance.Player;
    }

    #region ailment effect
    public void HandleAilmentChange(Ailment beforeAilment, Ailment newAilment)
    {

        _material.SetInt(_hashIsEffect, newAilment > 0 ? 1 : 0);

        //ailment visual priority => ignite -> shocked -> chill
        if ((newAilment & Ailment.Ignited) > 0)
        {
            _material.SetColor(_hashEffectColor, _igniteColor);
        }
        else if ((newAilment & Ailment.Shocked) > 0)
        {
            _material.SetColor(_hashEffectColor, _shockColor);
        }
        else if ((newAilment & Ailment.Chilled) > 0)
        {
            _material.SetColor(_hashEffectColor, _chillColor);
        }

        VisualizeIcon(beforeAilment, newAilment);
    }

    protected virtual void VisualizeIcon(Ailment beforeAilment, Ailment newAilment)
    {
        CheckVisualIcon(beforeAilment, newAilment, Ailment.Chilled, PoolingType.ChillVFX);
        CheckVisualIcon(beforeAilment, newAilment, Ailment.Ignited, PoolingType.IgniteVFX);
        CheckVisualIcon(beforeAilment, newAilment, Ailment.Shocked, PoolingType.ShockVFX);
    }

    protected void CheckVisualIcon(Ailment beforeAilment, Ailment newAilment, Ailment ailmentType, PoolingType effectType)
    {
        bool hasAilment = (newAilment & ailmentType) > 0;
        bool ailmentChanged = ((newAilment ^ beforeAilment) & ailmentType) > 0;
        
        if(!ailmentChanged) return; //변함없다면 할게 없음.
        
        //새로 생긴거면 추가하고 사라진거면 삭제하고
        if (hasAilment)
        {
            ParticleEffect effect = PoolManager.Instance.Pop(effectType) as ParticleEffect;
            _effects.Add(effectType, effect); 
            effect.transform.parent = transform;
            effect.transform.position = _vfxPosition.position;
            effect.PlayParticle();
        }else if (!hasAilment)
        {
            ParticleEffect effect = _effects[effectType];
            effect.StopParticle();
            PoolManager.Instance.Push(effect, true);
            _effects.Remove(effectType);
        }
    }

    #endregion
}
