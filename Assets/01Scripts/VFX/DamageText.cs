using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : PoolableMono
{
    [SerializeField] private Vector3 _moveOffset;
    private TextMeshPro _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TextMeshPro>();
    }

    public void ShowDamageText(Vector3 position, string value, float fontSize, Color fontColor)
    {
        _tmpText.text = value;
        _tmpText.fontSize = fontSize;
        _tmpText.color = fontColor;
        transform.position = position;
        //more effective show method will be added
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(position + _moveOffset, 0.7f));
        seq.Join(_tmpText.DOFade(0, 1f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public override void ResetPooingItem()
    {
        _tmpText.color = Color.white;
    }
}
