using System;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private PoolingListSO _poolingList;
    [SerializeField] private Transform _poolingTrm;

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
    }

    private void Start()
    {
        MenuWindow.Instance.OptionUI.ToggleEnemyHealthBarEvent += value => ShowEnemyHealthBar = value;
    }
    
}
