using System.Collections.Generic;
using UnityEngine;

public abstract class Stash
{
    public List<InventoryItem> stash;
    public Dictionary<ItemDataSO, InventoryItem> stashDictionary;
    protected Transform _slotParent;
    protected ItemSlotUI[] _itemSlots; //인벤토링 아이템 슬롯(장비등)

    public Stash(Transform parent)
    {
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();

        _itemSlots = parent.GetComponentsInChildren<ItemSlotUI>();
    }

    public virtual void UpdateSlotUI()
    {
        for (int i = 0; i < _itemSlots.Length; ++i)
        {
            _itemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stash.Count; ++i)
        {
            _itemSlots[i].UpdateSlot(stash[i]);
        }
    }

    public virtual bool HasItem(ItemDataSO itemData)
    {
        return stashDictionary.ContainsKey(itemData);
    }

    public abstract void AddItem(ItemDataSO itemData);

    public abstract void RemoveItem(ItemDataSO itemData, int count);

    public abstract bool CanAddItem();
}
