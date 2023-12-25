using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Toggle _showDamageTextToggle;
    [SerializeField] private Toggle _showHealthBarToggle;

    public event Action<bool> ToggleDamageTextEvent;
    public event Action<bool> ToggleEnemyHealthBarEvent;
    private void Start()
    {
        _showDamageTextToggle.onValueChanged.AddListener((value) => ToggleDamageTextEvent?.Invoke(value));
        _showHealthBarToggle.onValueChanged.AddListener((value) => ToggleEnemyHealthBarEvent?.Invoke(value));
    }

    
}
