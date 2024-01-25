using DG.Tweening;
using UnityEngine;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    [SerializeField] private FadeScreenUI _fadeScreen;

    private void Start()
    {
        _fadeScreen.ResetScreen(true); //검은색으로 칠하고
        _fadeScreen.FadeIn(0.5f);
    }

    public Tween FadeIn(float time) => _fadeScreen.FadeIn(time);
    public Tween FadeOut(float time) => _fadeScreen.FadeOut(time);
}
