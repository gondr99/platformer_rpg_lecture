
using UnityEngine;

public class DashSkill : Skill
{
    [SerializeField] private SkillTreeSlotUI _enableSkillSlot;

    private void Awake()
    {
        _enableSkillSlot.UpgradeEvent += HandleSkillEnable;
    }

    private void OnDestroy()
    {
        _enableSkillSlot.UpgradeEvent -= HandleSkillEnable;
    }

    private void HandleSkillEnable(int currentCount)
    {
        skillEnabled = true;
        _cooldown = 3 - currentCount * 0.1f;
    }
}
