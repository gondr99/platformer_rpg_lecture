using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;

    public InventoryItem item;
    private RectTransform _dragTarget;
    [HideInInspector] public int slotIndex;
    
    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        _itemImage.color = Color.white;

        if (item != null)
        {
            _itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                _itemText.text = item.stackSize.ToString();
            }
            else
            {
                _itemText.text = String.Empty;
            }

            item.slotIndex = slotIndex;
        }
        else
        {
            CleanUpSlot();
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;

        _itemText.text = String.Empty;
    }

    
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        item.reservedSlotIndex = -1; //드래그 드랍시 올바른 칸으로 들어가기 위한 코드..이건너무 더럽다.
        MenuWindow.Instance.dragItem.StartDrag(item);
        _dragTarget = MenuWindow.Instance.dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;

        _itemImage.color = Color.clear;
        _itemText.text = String.Empty;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _dragTarget.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //슬롯에 드랍되지 않았다면.
        if (!MenuWindow.Instance.dragItem.sucessDrop)
        {
            UpdateSlot(item); //다시 활성화
        }
        MenuWindow.Instance.dragItem.EndDrag();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        
        
        ItemSlotUI slot = gameObject.GetComponent<ItemSlotUI>();
        InventoryItem inventoryItem = this.item;


        EquipmentSlotUI equipSlot = slot as EquipmentSlotUI;
        if (equipSlot != null && inventoryItem != null) //장비칸에서 땡겨와서 교환해야하는거면
        {
            ItemDataEquipmentSO equipSO = inventoryItem.data as ItemDataEquipmentSO;
            
            //이 칸의 장비가 끌고 오는 장비와 종류가 맞지 않을경우는 거절.
            if (equipSO.equipmentType != equipSlot.slotType) 
                return;
            
            Inventory.Instance.EquipItem(equipSO);

            MenuWindow.Instance.dragItem.GetDraggedItem().reservedSlotIndex = slotIndex;
            return;
        }
        
        UpdateSlot(slot.item); //이 슬롯을 새로 그리고 인덱스 부여하고.
        slot.UpdateSlot(inventoryItem);
        
        MenuWindow.Instance.dragItem.sucessDrop = true; //성공적으로 드랍한경우
    }
}
