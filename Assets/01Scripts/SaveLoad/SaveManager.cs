using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField] private string fileName;
    [SerializeField] private bool _isEncrypt;
    
    private GameData _gameData;
    private List<ISaveable> _saveManagerList;
    private FileDataHandler _fileDataHandler;
    

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, _isEncrypt);
        _saveManagerList = FindAllSaveManagers();
        

        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();
        if (_gameData == null)
        {
            Debug.Log("No save data found");
            NewGame();
        }

        foreach (ISaveable manager in _saveManagerList)
        {
            manager.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveable manager in _saveManagerList)
        {
            manager.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindAllSaveManagers()
    {
        //interface is not Unityengine object. so use oftype linq
        return FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToList();
    }

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        //context is launching unity editor time. so make instance 
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, _isEncrypt);
        _fileDataHandler.DeleteSaveData();
    }


    public bool HasSaveData()
    {
        return _fileDataHandler.Load() != null;
    }
}
