using DG.Tweening;
using UnityEngine;

public class AfterImage : PoolableMono
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartFade(Vector3 position, Sprite sprite, float delayTime, bool isFlip)
    {
        transform.position = position;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.flipX = isFlip;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delayTime);
        seq.Append(_spriteRenderer.DOFade(0f, 0.4f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public override void ResetPooingItem()
    {
        _spriteRenderer.color = Color.white;
    }
}