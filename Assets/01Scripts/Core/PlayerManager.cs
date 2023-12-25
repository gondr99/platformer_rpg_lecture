using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private Player _player;

    public Player Player => _player;
    public Transform PlayerTrm => _player.transform;
    
    public event Action<int> SkillPointChanged;
    public event Action<int> StatPointChanged;
    public event Action<int> OnGoldChanged;
    
    public event Action ExpChanged;
    private int _statPoint;
    private int _skillPoint;
    
    public int level = 1;
    public int nextExpPoint = 1000;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int _gold = 0;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);
        }
    }
    
    
    
    public int SkillPoint
    {
        get => _skillPoint;
        private set
        {
            _skillPoint = value;
            SkillPointChanged?.Invoke(_skillPoint);
        }
    }
    public int StatPoint
    {
        get => _statPoint;
        private set
        {
            _statPoint = value;
            StatPointChanged?.Invoke(_statPoint);
        }
    }
    
    public bool CanSpendSkillPoint()
    {
        if (SkillPoint <= 0) return false;
        
        SkillPoint -= 1;
        return true;
    }

    public float GetNormalizedExp()
    {
        if (nextExpPoint <= 0) return 0;
        return (float)_currentExp / nextExpPoint;
    }
    
    public void AddExp(int exp)
    {
        _currentExp += exp;
        ExpChanged?.Invoke();
        if (_currentExp >= nextExpPoint)
        {
            LevelUpProcess();
        }
    }

    private void LevelUpProcess()
    {
        _currentExp -= nextExpPoint;
        level += 1;
        SkillPoint += 1;
        StatPoint += 5;
        ExpChanged?.Invoke();
    }

    //디버그용 코드들
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            AddExp(500);
        }
    }
    
}
