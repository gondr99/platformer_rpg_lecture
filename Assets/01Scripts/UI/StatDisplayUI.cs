using UnityEngine;

public class StatDisplayUI : MonoBehaviour
{
    private StatSlotUI[] _statSlots;

    private void Awake()
    {
        _statSlots = GetComponentsInChildren<StatSlotUI>();
    }

    private void Start()
    {
        Inventory.Instance.OnEquipmentChanged += HandleUpdateStat;
        HandleUpdateStat();
    }

    private void HandleUpdateStat()
    {
        for (int i = 0; i < _statSlots.Length; ++i)
        {
            _statSlots[i].UpdateStatValueUI();
        }
    }
}
