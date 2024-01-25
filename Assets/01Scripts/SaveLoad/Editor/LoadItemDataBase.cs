using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class LoadItemDataBase : Editor
{
    private SaveManager _saveManager;
    private string _soName = "itemDB";
    private void OnEnable()
    {
        _saveManager = (SaveManager)target; //타겟 가져오고
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Generate Item database"))
        {
            CreateAssetDatabase();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void CreateAssetDatabase()
    {
        List<ItemDataSO> loadedItemList = new List<ItemDataSO>();
        string[] assetNames = AssetDatabase.FindAssets("",
            new[] { "Assets/08SO/Items" });

        foreach (string soName in assetNames)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(soName);
            ItemDataSO itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>(assetPath);
            if (itemData != null)
                loadedItemList.Add(itemData);
        }
        
        
        ItemDatabaseSO itemDB;
        itemDB = AssetDatabase.LoadAssetAtPath<ItemDatabaseSO>($"Assets/08SO/{_soName}.asset");
        string filename = AssetDatabase.GenerateUniqueAssetPath($"Assets/08SO/{_soName}.asset");
        
        if (itemDB == null)
        {
            itemDB = ScriptableObject.CreateInstance<ItemDatabaseSO>();
            itemDB.itemList = loadedItemList;
            AssetDatabase.CreateAsset(itemDB, filename);
            Debug.Log($"item database created at {filename}");
        }
        else
        {
            itemDB.itemList = loadedItemList;
            
            EditorUtility.SetDirty(itemDB);
            Debug.Log($"item database updated at {filename}");
        }
        
    }
}
