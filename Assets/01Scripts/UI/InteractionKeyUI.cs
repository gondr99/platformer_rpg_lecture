using DG.Tweening;
using UnityEngine;

public class InteractionKeyUI : MonoBehaviour
{
    [SerializeField] private Transform _bar;
    [SerializeField] private Transform _barTrm;
    [SerializeField] private Color _barColor;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Tween _showTween; //키가 나오는 트윈효과
    private void Awake()
    {
        _barTrm.Find("Fill").GetComponent<SpriteRenderer>().color = _barColor;
    }

    public void SetNormalizedGauge(float value)
    {
        _barTrm.localScale = new Vector3(value, 1, 1);
        _bar.gameObject.SetActive(value > 0);
    }

    public void SetActiveState(bool value)
    {
        gameObject.SetActive(value);
        if (_showTween != null && _showTween.IsActive())
        {
            _showTween.Kill();
        }

        if (value)
        {
            _showTween = _spriteRenderer.DOFade(1f, 0.3f);
        }
        else
        {
            _showTween = _spriteRenderer.DOFade(0f, 0.3f);
        }

    }
}
