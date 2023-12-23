using System;
using TMPro;
using UnityEngine;

public enum StatCategory
{
    Number,
    Percent,
    Ms,
}
public class StatSlotUI : MonoBehaviour
{
    [SerializeField] private StatType _statType;
    [SerializeField] private string _statName;

    [SerializeField] private StatCategory _statCategory;
    
    [SerializeField] private TextMeshProUGUI _statNameText;
    [SerializeField] private TextMeshProUGUI _statValueText;
    
    [TextArea][SerializeField] private string _statDescription;
    private void OnValidate()
    {
        gameObject.name = $"Stat - {_statType.ToString()}";

        if (!string.IsNullOrEmpty(_statName))
        {
            _statNameText.text = _statName;
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStat playerStat = PlayerManager.Instance.Player.PStat;

        if (playerStat != null)
        {
            int value = playerStat.GetStatByType(_statType).GetValue();
            switch (_statCategory)
            {
                case StatCategory.Number:
                    _statValueText.text = value.ToString();
                    break;
                case StatCategory.Percent:
                    _statValueText.text = $"{value}%";
                    break;
                case StatCategory.Ms:
                    _statValueText.text = (value / 1000).ToString("n2");        
                    break;
            }
        }
    }
}
