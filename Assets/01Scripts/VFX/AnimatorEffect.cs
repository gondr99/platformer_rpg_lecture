using UnityEngine;

public class AnimatorEffect : PoolableMono
{
    [SerializeField] private string _clipName;

    private int _clipNameHash;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private readonly int _hdrColorHash = Shader.PropertyToID("_EmissionColor");
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _clipNameHash = Animator.StringToHash(_clipName);
    }

    public void SetEffectColor(Color color)
    {
        Material mat = _spriteRenderer.material;
        mat.SetColor(_hdrColorHash, color);
    }
    
    public void PlayAnimation(Vector3 position, Quaternion rot, Vector3 scale)
    {
        transform.localRotation = rot;
        transform.position = position;
        transform.localScale = scale;
        _animator.enabled = true;
        _animator.Play(_clipNameHash);
    }

    private void OnAnimationEndTrigger()
    {
        PoolManager.Instance.Push(this);
    }
    public override void ResetPooingItem()
    {
        _animator.enabled = false;
    }
}
