using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Image _healthSlider;
    [SerializeField] private Image _expSlider;
    [SerializeField] private TextMeshProUGUI _goldText;
    private Health _playerHealth;
    
    private void Start()
    {
        _playerHealth = PlayerManager.Instance.Player.HealthCompo;
        _playerHealth.OnHit += HandleHitEvent;
        _playerHealth.OnHealthChanged += HandleHitEvent;

        PlayerManager.Instance.ExpChanged += HandleExpEvent;
        HandleExpEvent(); //최초 한번 실행

        PlayerManager.Instance.OnGoldChanged += HandleGoldChangeEvent;
        HandleGoldChangeEvent(PlayerManager.Instance.Gold);
    }

    private void HandleGoldChangeEvent(int value)
    {
        _goldText.text = value.ToString();
    }

    private void HandleHitEvent()
    {
        _healthSlider.fillAmount = _playerHealth.GetNormalizedHealth();
    }

    private void HandleExpEvent()
    {
        Debug.Log("exp changed");
        _expSlider.fillAmount = PlayerManager.Instance.GetNormalizedExp();
    }
}
