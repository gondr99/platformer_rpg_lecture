using System.Collections;
using UnityEngine;

public class BlinkFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _delaySecond;
    [SerializeField] private float _blinkValue;
    private readonly int _blinkShaderParam = Shader.PropertyToID("_BlinkValue");

    private Material _material;
    private bool _isFinished;
    private Coroutine _delayCoroutine = null;

    private void Awake()
    {
        _material = _spriteRenderer.material;
    }

    public void CreateFeedback()
    {
        _material.SetFloat(_blinkShaderParam, _blinkValue);
        _delayCoroutine = StartCoroutine(SetNormalAfterDelay());
        Debug.Log("feedback");
    }

    private IEnumerator SetNormalAfterDelay()
    {
        _isFinished = false;
        yield return new WaitForSeconds(_delaySecond);

        if (_isFinished == false)
        {
            CompleteFeedback();
        }
    }

    public void CompleteFeedback()
    {
        if(_delayCoroutine != null)
        {
            StopCoroutine(_delayCoroutine);
        }
        _isFinished = true;
        _material.SetFloat(_blinkShaderParam, 0);
    }
}
