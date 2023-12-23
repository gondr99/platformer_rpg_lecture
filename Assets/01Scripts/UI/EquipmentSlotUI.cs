using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;
    
    private void OnValidate()
    {
        gameObject.name = $"Equip Slot [ {slotType.ToString()} ]";
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        DragItem dragItem = MenuWindow.Instance.dragItem;
        //슬롯에 드랍되지 않았다면.
        if (!dragItem.sucessDrop)
        {
            UpdateSlot(item); //다시 활성화
        }
        else
        {
            InventoryItem draggedItem = dragItem.GetDraggedItem();
            Inventory.Instance.UnEquipItem(draggedItem.data as ItemDataEquipmentSO);
        }
        MenuWindow.Instance.dragItem.EndDrag();

    }

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        ItemSlotUI slot = gameObject.GetComponent<ItemSlotUI>();

        ItemDataEquipmentSO equipment = slot.item.data as ItemDataEquipmentSO;
        if (equipment == null) return; //올바르지 않은 칸 또는 올바르지 않은 아이템 타입
        if (equipment.equipmentType != slotType) return;
        
        MenuWindow.Instance.dragItem.sucessDrop = true; //성공적으로 드랍한경우
        
        Inventory.Instance.EquipItem(slot.item.data);
    }
}
