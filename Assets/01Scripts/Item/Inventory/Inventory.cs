using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public MaterialStash materialStash;

    [Header("Inventory UI")]
    [SerializeField] private Transform _materialStashSlotParent;

    private void Awake()
    {
        materialStash = new MaterialStash(_materialStashSlotParent);//재료 창고 제작
    }

    private void UpdateSlotUI()
    {
        materialStash.UpdateSlotUI(); //재료창고 다시그리기.
    }

    public bool AddItem(ItemDataSO item)
    {
        bool itemAdded = false;
        if (item.itemType == ItemType.Material && materialStash.CanAddItem())
        {
            materialStash.AddItem(item);
            itemAdded = true;
        }
        if (itemAdded)
        {
            UpdateSlotUI();
        }

        return itemAdded;
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
        }
        UpdateSlotUI();
    }
}
