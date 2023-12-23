using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragItem : MonoBehaviour
{
    private Image _itemImage;
    private TextMeshProUGUI _amountText;
    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public bool sucessDrop = false;

    private InventoryItem _inventoryItem;
    private void Awake()
    {
        _itemImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        _amountText = transform.Find("Count").GetComponent<TextMeshProUGUI>();
    }

    public InventoryItem GetDraggedItem()
    {
        return _inventoryItem;
    }
    
    public void StartDrag(InventoryItem item)
    {
        _inventoryItem = item;
        _itemImage.sprite = item.data.icon;
        _itemImage.color = Color.white;
        _amountText.text = item.stackSize > 1 ? item.stackSize.ToString() : string.Empty;
        sucessDrop = false;
    }

    public void EndDrag()
    {
        _itemImage.color = Color.clear;
        _amountText.text = string.Empty;
    }
}
