using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSlots
{
    public List<InventoryItem> equipments;
    public Dictionary<ItemDataEquipmentSO, InventoryItem> equipmentDictionary;
    
    protected Transform _parentTrm;
    protected EquipmentSlotUI[] _equipmentSlots;

    protected Inventory _inventory;
    
    public EquipmentSlots(Transform parent, Inventory inventory)
    {
        _parentTrm = parent;
        _inventory = inventory;
        
        equipments = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipmentSO, InventoryItem>();

        _equipmentSlots = parent.GetComponentsInChildren<EquipmentSlotUI>();
        for (int i = 0; i < _equipmentSlots.Length; ++i)
        {
            _equipmentSlots[i].slotIndex = i;
        }
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < _equipmentSlots.Length; ++i)
        {
            //해당 슬롯에 들어갈 수 있는 아이템을 보유중이라면
            ItemDataEquipmentSO slotEquipment = equipmentDictionary.Keys.ToList().Find(x => x.equipmentType == _equipmentSlots[i].slotType);
            if (slotEquipment != null)
            {
                _equipmentSlots[i].UpdateSlot(equipmentDictionary[slotEquipment]);
            }
        }
    }
    
    public ItemDataEquipmentSO GetEquipmentByType(EquipmentType type)
    {
        ItemDataEquipmentSO equipItem = null;
    
        foreach (KeyValuePair<ItemDataEquipmentSO, InventoryItem> equipKeyValue in equipmentDictionary)
        {
            //지정된 장비 타입을 장착하고 있다면 가져온다. 
            if (equipKeyValue.Key.equipmentType == type)
            {
                equipItem = equipKeyValue.Key;
                break;
            }
        }
        return equipItem;
    }
    
    public void EquipItem(ItemDataEquipmentSO newEquipment)
    {
        InventoryItem newItem = new InventoryItem(newEquipment, 0);
        
        ItemDataEquipmentSO oldEquipment = null; 
        foreach (KeyValuePair<ItemDataEquipmentSO, InventoryItem> equipKeyValue in equipmentDictionary)
        {
            //같은 장비 타입이면. 기존 장비 해제하고 
            if (equipKeyValue.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = equipKeyValue.Key; //삭제할 아이템 저장.
                break;
            }
        }

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
        }
        
        equipments.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers(); //장착한 아이템의 효과 적용.
        
    }

    public void UnEquipItem(ItemDataEquipmentSO oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipments.Remove(value);
            equipmentDictionary.Remove(oldEquipment); //리스트와 딕셔너리에서 모두 빼준다.
            oldEquipment.RemoveModifiers(); //빼준 아이템의 효과 빼준다.

            if (value.reservedSlotIndex >= 0)
            {
                value.reservedSlotIndex = -1;
                _inventory.AddItem(oldEquipment, value.reservedSlotIndex); //삭제하고 인벤토리에 다시 넣어준다.
            }
            else
            {
                _inventory.AddItem(oldEquipment);
            }
            
        }
    }
}
