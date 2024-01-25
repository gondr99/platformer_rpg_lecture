using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenUI : MonoBehaviour
{
    private Image _frontFadeImage;

    private void Awake()
    {
        _frontFadeImage = GetComponent<Image>();
    }

    public void ResetScreen(bool isBlack)
    {
        gameObject.SetActive(true);
        if (isBlack)
            _frontFadeImage.color = Color.black;
        else
            _frontFadeImage.color = Color.clear;
    }
    
    public Tween FadeOut(float sec)
    {
        return _frontFadeImage.DOFade(1f, sec);
    }

    public Tween FadeIn(float sec)
    {
        return _frontFadeImage.DOFade(0, sec);
    }
}
