using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(menuName = "SO/Items/Item", fileName = "New Item data")]
public class ItemDataSO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemID;

    [Range(0, 100)]
    public float dropChance;
    
    protected StringBuilder _stringBuilder = new StringBuilder();
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path); //이 에셋의 GUID
#endif
    }

    public virtual string GetDescription()
    {
        return String.Empty;
    }
}
