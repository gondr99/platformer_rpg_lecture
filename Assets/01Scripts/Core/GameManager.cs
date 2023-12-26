using System;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>, ISaveable
{
    [SerializeField] private PoolingListSO _poolingList;
    [SerializeField] private Transform _poolingTrm;

    //all checkpoint list
    [SerializeField] private SavePoint[] _savePoints;
    private string _lastVisitedSavePointId = string.Empty;

    public event Action<bool> HealthBarShow;
    private bool _showEnemyHealthbar;
    public bool ShowEnemyHealthBar
    {
        get => _showEnemyHealthbar;
        set
        {
            _showEnemyHealthbar = value;
            HealthBarShow?.Invoke(_showEnemyHealthbar);
        }
    }
    private void Awake()
    {
        PoolManager.Instance = new PoolManager(_poolingTrm);
        foreach (PoolingPair pair in _poolingList.list)
        {
            PoolManager.Instance.CreatePool(pair.prefab, pair.type, pair.count);
        }
        
        //닷트윈 초기 셋업도 여기서
        DOTween.Init(recycleAllByDefault: true, useSafeMode: true, LogBehaviour.Verbose).SetCapacity(400, 100);

        SavePoint.SavePointActiveEvent += HandleSavePointActive;
    }

    
    private void Start()
    {
        MenuWindow.Instance.OptionUI.ToggleEnemyHealthBarEvent += value => ShowEnemyHealthBar = value;
        LoadLevelCheckPoint();
    }
    
    private void HandleSavePointActive(string savePointGuid)
    {
        _lastVisitedSavePointId = savePointGuid;
        SaveManager.Instance.SaveGame();
    }

    private void OnDestroy()
    {
        SavePoint.SavePointActiveEvent -= HandleSavePointActive;
    }

    public void LoadLevelCheckPoint()
    {
        _savePoints = FindObjectsOfType<SavePoint>();
    }
    
    public SavePoint GetLastCheckPoint()
    {
        if (_lastVisitedSavePointId == string.Empty) return null;

        foreach (var point in _savePoints)
        {
            if (point.checkpointID == _lastVisitedSavePointId)
                return point;
        }
        return null;
    }

    public void LoadData(GameData data)
    {
        LoadLevelCheckPoint();
        foreach (SavePoint point in _savePoints)
        {
            if (data.savePoints.TryGetValue(point.checkpointID, out bool value))
            {
                //활성화된 체크포인트면
                if (value)
                {
                    point.ActiveCheckPoint();
                }
            }
        }

        _lastVisitedSavePointId = data.lastVisitedSavePointID;
        SavePoint lastPoint = GetLastCheckPoint();
        Debug.Log(_lastVisitedSavePointId);
        if (lastPoint != null)
        {
            PlayerManager.Instance.PlayerTrm.position = lastPoint.transform.position + new Vector3(0, 1.5f);
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (SavePoint point in _savePoints)
        {
            if (data.savePoints.TryGetValue(point.checkpointID, out bool saved))
            {
                data.savePoints[point.checkpointID] = point.isActivated;
            }
            else
            {
                data.savePoints.Add(point.checkpointID, point.isActivated);
            }
            data.lastVisitedSavePointID = _lastVisitedSavePointId;
        }
    }
}
