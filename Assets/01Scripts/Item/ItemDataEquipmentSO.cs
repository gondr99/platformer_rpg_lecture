using System;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[Serializable]
public struct StatValue
{
    public StatType statType;
    public int value;
}

[CreateAssetMenu(menuName = "SO/Items/Equipment", fileName = "New Item data")]
public class ItemDataEquipmentSO : ItemDataSO
{
    public EquipmentType equipmentType;
    private int _descriptionLength;
    
    [TextArea]
    public string itemEffectDescription;
    
    [Header("item effect")]
    public ItemEffectSO[] effectList;

    public List<StatValue> additionalStats;
    
    public void AddModifiers()
    {
        PlayerStat playerStat = PlayerManager.Instance.Player.PStat;
        if (playerStat == null)
            return;

        foreach (StatValue statValue in additionalStats)
        {
            Stat stat = playerStat.GetStatByType(statValue.statType);
            stat.AddModifier( statValue.value );
        }
    }

    public void RemoveModifiers()
    {
        PlayerStat playerStat = PlayerManager.Instance.Player.PStat;
        if (playerStat == null)
            return;

        foreach (StatValue statValue in additionalStats)
        {
            Stat stat = playerStat.GetStatByType(statValue.statType);
            stat.RemoveModifier( statValue.value );
        }
    }

    public override string GetDescription()
    {
        _stringBuilder.Clear();
        _descriptionLength = 0;
        foreach (StatValue statValue in additionalStats)
        {
            AddItemDescription( statValue.statType.ToString(), statValue.value );
        }

        if (_descriptionLength < 5)
        {
            for (int i = _descriptionLength; i < 5; ++i)
            {
                _stringBuilder.AppendLine();
                _stringBuilder.Append("");
            }
        }

        if (!string.IsNullOrEmpty(itemEffectDescription))
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append(itemEffectDescription);
        }
        
        return _stringBuilder.ToString();
    }

    private void AddItemDescription(string name, int value)
    {
        //multi locale service must be implement here
        if (value != 0)
        {
            if (_stringBuilder.Length > 0)
            {
                _stringBuilder.AppendLine();
            }

            ++_descriptionLength;
            _stringBuilder.Append($"{name} : {value.ToString()}");
        }
    }

    public void UseEffect(EffectType type)
    {
        foreach (ItemEffectSO effect in effectList)
        {
            effect.AttemptUseEffect(type);
        }
    }
}
