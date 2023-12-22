using System;

[Serializable]
public class InventoryItem
{
    public ItemDataSO data;
    public int stackSize;

    public InventoryItem(ItemDataSO newItemData)
    {
        data = newItemData;
        AddStack();
    }

    public void AddStack()
    {
        ++stackSize;
    }

    public void RemoveStack(int count = 1)
    {
        stackSize -= 1;
    }
}
