using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>, ISaveable
{
    public MaterialStash materialStash;
    public EquipmentStash equipmentStash;//장비 창고
    public EquipmentSlots equipmentSlots; //장착칸

    [Header("ItemDatabase")]
    [SerializeField] private ItemDatabaseSO _itemDB;

    [Header("Inventory UI")]
    [SerializeField] private Transform _materialStashSlotParent;
    [SerializeField] private Transform _equipStashSlotParent;
    [SerializeField] private Transform _equipmentSlotParent;

    public event Action OnEquipmentChanged;
    
    private void Awake()
    {
        materialStash = new MaterialStash(_materialStashSlotParent);//재료 창고 제작
        equipmentStash = new EquipmentStash(_equipStashSlotParent);//장비 창고 제작
        equipmentSlots = new EquipmentSlots(_equipmentSlotParent, this); 
    }

    private void UpdateSlotUI()
    {
        materialStash.UpdateSlotUI(); //재료창고 다시그리기.
        equipmentStash.UpdateSlotUI(); //장비 창고 다시 그리기.
        equipmentSlots.UpdateSlotUI(); //장비칸 새로 그리기
    }

    //장비 장착
    public void EquipItem(ItemDataSO item)
    {
        ItemDataEquipmentSO newEquipment = item as ItemDataEquipmentSO;
        if (newEquipment == null)
        {
            Debug.LogError("can not equip, this item");
            return; //장비 아이템이 아닌경우 장착 불가.
        }
        
        equipmentSlots.EquipItem(newEquipment);
        equipmentStash.RemoveItem(item, 1);//장착한 아이템은 인벤토리에서 삭제한다. (장비칸으로 넘어갔으니까)
        UpdateSlotUI();
        OnEquipmentChanged?.Invoke();
    }

    //장비 장착 해제.
    public void UnEquipItem(ItemDataEquipmentSO oldEquipment)
    {
        if (oldEquipment == null) return;
        equipmentSlots.UnEquipItem(oldEquipment);
        OnEquipmentChanged?.Invoke();
    }
    
    //칸을 지정해서 넣을 꺼면 addIndex에 값 보내기
    public bool AddItem(ItemDataSO item, int count = 1, int addIndex = -1)
    {
        bool itemAdded = false;
        if (item.itemType == ItemType.Material && materialStash.CanAddItem(item))
        {
            materialStash.AddItem(item, count, addIndex);
            itemAdded = true;
        }

        if(item.itemType == ItemType.Equipment )
        {
            ItemDataEquipmentSO equipItem = item as ItemDataEquipmentSO;
            if(equipmentStash.CanAddItem(item) && !equipmentSlots.HasEquipItem(equipItem))
            {
                equipmentStash.AddItem(item, count, addIndex);
                itemAdded = true;
            }
        }
        
        if (itemAdded)
        {
            UpdateSlotUI();
        }

        return itemAdded;
    }

    public bool CanAddItem(ItemDataSO item)
    {
        bool canMaterialAdd = (item.itemType == ItemType.Material && materialStash.CanAddItem(item));
        bool canEquipmentAdd = (item.itemType == ItemType.Equipment && equipmentStash.CanAddItem(item) && !equipmentSlots.HasEquipItem(item as ItemDataEquipmentSO));
        
        return canMaterialAdd || canEquipmentAdd;
    }

    public void RemoveItem(ItemDataSO item, int count = 1)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                if (materialStash.HasItem(item))
                {
                    materialStash.RemoveItem(item, count);
                }
                break;
            case ItemType.Equipment:
                if (equipmentStash.HasItem(item))
                {
                    equipmentStash.RemoveItem(item, count);
                }
                break;
        }
        UpdateSlotUI();
    }

    public void LoadData(GameData data)
    {
        List<ItemDataSO> ItemDB = _itemDB.itemList;

        foreach (var pair in data.inventory)
        {
            ItemDataSO item = ItemDB.Find(x => x.itemID == pair.Key);
            if (item != null)
            {
                AddItem(item, pair.Value.stackSize, pair.Value.slotIndex);
            }
        }

        //장착 장비들 복원
        foreach (string itemID in data.equipmentsIDList)
        {
            ItemDataSO item = ItemDB.Find(x => x.itemID == itemID);
            if (item != null)
            {
                EquipItem(item);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inventory.Clear();
        foreach (var pair in equipmentStash.stashDictionary)
        {
            data.inventory.Add(pair.Key.itemID, pair.Value);
        }

        foreach (var pair in materialStash.stashDictionary)
        {
            data.inventory.Add(pair.Key.itemID, pair.Value);
        }

        foreach(var pair in equipmentSlots.equipmentDictionary)
        {
            data.equipmentsIDList.Add(pair.Key.itemID);
        }
    }

    public void UseItemEffect(EffectType type)
    {
        equipmentSlots.UseEquipItemEffect(type);
    }
}
