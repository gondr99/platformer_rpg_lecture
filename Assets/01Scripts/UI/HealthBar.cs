using System;
using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _fillTrm;
    [SerializeField] private Entity _owner;
    
    private void Start()
    {
        GameManager.Instance.HealthBarShow += HandleHealthBarShow;
        
        _owner.HealthCompo.OnHit += HandleHealthChanged;
        _owner.OnFlip += HandleFlip;
    }

    private void OnDestroy()
    {
        GameManager.Instance.HealthBarShow -= HandleHealthBarShow;
        _owner.HealthCompo.OnHit -= HandleHealthChanged;
        _owner.OnFlip -= HandleFlip;
    }

    private void HandleFlip(int direction)
    {
        FlipUI();
    }

    private void HandleHealthChanged()
    {
        _fillTrm.DOKill();
        _fillTrm.DOScaleX(_owner.HealthCompo.GetNormalizedHealth(), 0.2f);
    }

    private void HandleHealthBarShow(bool value)
    {
        gameObject.SetActive(value);
    }

    public void FlipUI()
    {
        transform.Rotate(0, 180, 0); //180도 회전.
    }
}