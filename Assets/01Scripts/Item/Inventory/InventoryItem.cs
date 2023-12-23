using System;

[Serializable]
public class InventoryItem
{
    public ItemDataSO data;
    public int stackSize;
    public int slotIndex;
    public int reservedSlotIndex = -1;

    public InventoryItem(ItemDataSO newItemData, int slotIndex)
    {
        data = newItemData;
        this.slotIndex = slotIndex;
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
