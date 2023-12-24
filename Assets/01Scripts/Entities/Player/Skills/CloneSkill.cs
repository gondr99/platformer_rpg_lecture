using System;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone_info")]
    [SerializeField] private CloneController _clonePrefab;
    public float cloneDuration;
    public bool createCloneOnDashStart;
    public bool createCloneOnDashOver;
    public bool createCloneOnCounterAttack;

    [SerializeField] private float _counterCloneOffset = 2f;
    [SerializeField] private float _delayToCreateCounterClone = 0.4f;
    public float findEnemyRadius = 8f;
    public float damageMultiplier = 1f; //증뎀량.
    
    [Header("Duplicate clone")]
    public bool canDuplicateClone; //공격 종료후 다른 클론을 만들어 낼 수 있는가?
    public float duplicatePercent;
    public int maxDuplicateCounter;
    public float reducePercentByCount;

    [Header("Skill tree")]
    [SerializeField] private SkillTreeSlotUI _createCloneOnStartSlot;
    [SerializeField] private SkillTreeSlotUI _createCloneOnOverSlot;
    [SerializeField] private SkillTreeSlotUI _createCloneOnCounterSlot;
    [SerializeField] private SkillTreeSlotUI _duplicateCloneSlot;

    #region skill tree 
    private void Awake()
    {
        _createCloneOnStartSlot.UpgradeEvent += HandleCreateCloneOnStartEvent;
        _createCloneOnOverSlot.UpgradeEvent += HandleCreateCloneOnOverEvent;
        _createCloneOnCounterSlot.UpgradeEvent += HandleCreateCloneOnCounterEvent;
        _duplicateCloneSlot.UpgradeEvent += HandleDuplicateCloneEvent;
    }

    private void OnDestroy()
    {
        _createCloneOnStartSlot.UpgradeEvent -= HandleCreateCloneOnStartEvent;
        _createCloneOnOverSlot.UpgradeEvent -= HandleCreateCloneOnOverEvent;
        _createCloneOnCounterSlot.UpgradeEvent -= HandleCreateCloneOnCounterEvent;
        _duplicateCloneSlot.UpgradeEvent -= HandleDuplicateCloneEvent;
    }

    private void HandleCreateCloneOnStartEvent(int currentCount)
    {
        skillEnabled = true;
        createCloneOnDashStart = true;
    }

    private void HandleCreateCloneOnOverEvent(int currentCount)
    {
        createCloneOnDashOver = true;
    }

    private void HandleCreateCloneOnCounterEvent(int currentCount)
    {
        createCloneOnCounterAttack = true;
    }

    private void HandleDuplicateCloneEvent(int currentCount)
    {
        canDuplicateClone = true;
        maxDuplicateCounter = currentCount;
        reducePercentByCount = 25f; //25퍼씩 경감
        duplicatePercent = 20 + currentCount * 15f;
    }

    #endregion
    


    public void CreateClone(Transform originTrm, Vector3 offset, int duplicatedCount = 1)
    {
        CloneController newClone = Instantiate(_clonePrefab);
        newClone.SetupClone(this, originTrm, offset, duplicatedCount);
    }
    
    public void CreateCloneOnDashStart()
    {
        if(createCloneOnDashStart)
            CreateClone(_player.transform, Vector3.zero);
    }
    
    public void CreateCloneOnDashOver()
    {
        if(createCloneOnDashOver)
            CreateClone(_player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemy)
    {
        if (createCloneOnCounterAttack)
        {
            _player.StartDelayCallback(_delayToCreateCounterClone, () =>
            {
                CreateClone(enemy.transform, new Vector3(_counterCloneOffset * _player.FacingDirection, 0, 0));
            });
        }
    }
}
