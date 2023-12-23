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
        for (int i = 0; i < _itemSlots.Length; ++i)
        {
            _itemSlots[i].slotIndex = i;
        }
    }

    public virtual void UpdateSlotUI()
    {
        for (int i = 0; i < _itemSlots.Length; ++i)
        {
            _itemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stash.Count; ++i)
        {
            _itemSlots[stash[i].slotIndex].UpdateSlot(stash[i]);
            //_itemSlots[i].UpdateSlot(stash[i]);
        }
    }

    protected int FindEmptySlotIndex()
    {
        for (int i = 0; i < _itemSlots.Length; ++i)
        {
            InventoryItem item = stash.Find(x => x.slotIndex == i);
            if (item == null) return i;
        }

        return -1;
    }

    protected void SortStashBySlotIndex()
    {
        //오름차순으로 인덱스순 정렬
        stash.Sort((a, b)=>  a.slotIndex - b.slotIndex );
    }
    
    public virtual bool HasItem(ItemDataSO itemData)
    {
        return stashDictionary.ContainsKey(itemData);
    }

    public abstract void AddItem(ItemDataSO itemData, int addIndex);

    public abstract void RemoveItem(ItemDataSO itemData, int count);

    public abstract bool CanAddItem(ItemDataSO itemData);
}
