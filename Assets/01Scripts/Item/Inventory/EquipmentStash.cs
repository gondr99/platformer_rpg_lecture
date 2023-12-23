using UnityEngine;

public class EquipmentStash : Stash
{
    public EquipmentStash(Transform parent) : base(parent)
    {
    }

    //장비는 다 새로운 것들로 판단.
    public override void AddItem(ItemDataSO item, int addIndex)
    {
        int idx = addIndex;
        if(idx < 0)
            idx = FindEmptySlotIndex();
        InventoryItem newItem = new InventoryItem(item, idx);
        stash.Add(newItem);
        stashDictionary.Add(item, newItem);
        SortStashBySlotIndex();// resorting items;
    }

    public override void RemoveItem(ItemDataSO item, int count)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            stash.Remove(stashValue);
            stashDictionary.Remove(item);
        }
    }

    public override bool CanAddItem(ItemDataSO item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            Debug.Log("Item already collected");
            return false;
        }
        if (stash.Count >= _itemSlots.Length)
        {
            Debug.Log("No more space in stash");
            return false;
        }
        
        return true;
    }
}