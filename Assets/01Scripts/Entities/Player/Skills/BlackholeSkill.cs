using System;
using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private BlackholeSkillController _blackholeSkillPrefab;
    
    [Header("Attack info")] 
    public int amountOfAttack = 4;
    public float cloneAttackCooldown = 0.3f;
    public float holdBlackholeTime = 2f; //2초간 유지. 2초간 아무것도 안누르면 취소.
    
    [Header("Effect info")]
    public float maxSize;
    public float growSpeed;

    public float maxRiffleCount = 2;
    public float maxRiffleSpeed = 1f;

    public bool isReadyToAttack;
    
    public event Action SkillEffectEnd;
    
    private BlackholeSkillController _blackholeSkill;

    [Header("Skill Tree")] 
    [SerializeField] private SkillTreeSlotUI _skillEnable;
    [SerializeField] private SkillTreeSlotUI _increaseClone;
    [SerializeField] private SkillTreeSlotUI _decreaseCooldown;

    #region skill tree

    private void Awake()
    {
        _skillEnable.UpgradeEvent += HandleSkillEnableEvent;
        _increaseClone.UpgradeEvent += HandleIncreaseCloneEvent;
        _decreaseCooldown.UpgradeEvent += HandleDecreaseCloneEvent;
    }

    private void OnDestroy()
    {
        _skillEnable.UpgradeEvent -= HandleSkillEnableEvent;
        _increaseClone.UpgradeEvent -= HandleIncreaseCloneEvent;
        _decreaseCooldown.UpgradeEvent -= HandleDecreaseCloneEvent;
    }

    private void HandleSkillEnableEvent(int currentCount)
    {
        skillEnabled = true;
        maxSize = 5 + currentCount * 4;
    }

    private void HandleIncreaseCloneEvent(int currentCount)
    {
        amountOfAttack = 4 + currentCount;
    }

    private void HandleDecreaseCloneEvent(int currentCount)
    {
        _cooldown = 150 - currentCount * 10f;
    }

    #endregion
    
    
    protected override void Start()
    {
        base.Start();
        //한개 가지고 계속 쓸거라서 미리 만들어놓고 액티브 꺼주면 된다.
        _blackholeSkill = Instantiate(_blackholeSkillPrefab, transform.position, Quaternion.identity);
        _blackholeSkill.transform.parent = transform;

        _blackholeSkill.InitSkillValue(this);
        
        _blackholeSkill.gameObject.SetActive(false);
    }
    
    public void BlackholeFieldOpen(Vector3 position)
    {
        _blackholeSkill.transform.position = position;
        _blackholeSkill.SetUpSkill();
        _blackholeSkill.gameObject.SetActive(true);
    }

    public void ReleaseAttack()
    {
        _blackholeSkill.ReleaseCloneAttack();
    }

    public void SkillControllerEnd()
    {
        _blackholeSkill.gameObject.SetActive(false);
        SkillEffectEnd?.Invoke();
    }
}
