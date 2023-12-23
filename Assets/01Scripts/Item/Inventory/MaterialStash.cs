using UnityEngine;

public class MaterialStash : Stash
{
    public MaterialStash(Transform parent) : base(parent)
    {
    }

    public override void AddItem(ItemDataSO item, int addIndex)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            int idx = addIndex;
            if(idx < 0)
                idx = FindEmptySlotIndex();
            InventoryItem newItem = new InventoryItem(item, idx);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
            
            SortStashBySlotIndex(); //재정렬
        }
    }

    public override void RemoveItem(ItemDataSO item, int count)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= count)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashValue.RemoveStack(count);
            }
        }
    }

    public override bool CanAddItem(ItemDataSO itemDataSo)
    {
        //동일 종류 아이템이 존재하면 스택만 증가시키면 되니 가능
        if (stashDictionary.TryGetValue(itemDataSo, out InventoryItem inventoryItem))
        {
            return true;
        }
        
        if (stash.Count >= _itemSlots.Length)
        {
            Debug.Log("No more space in stash");
            return false;
        }
        return true;
    }

    
}
