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

    protected virtual void Awake()
    {
        _material = _spriteRenderer.material;
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
        
    }
    #endregion
}
