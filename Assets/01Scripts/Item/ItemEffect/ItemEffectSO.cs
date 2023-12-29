using System.Collections.Generic;
using UnityEngine;
public enum EffectType
{
    Melee,
    Skill,
    Hit
}
public abstract class ItemEffectSO : ScriptableObject
{
    [Range(0, 100f)]
    public float effectChance;
    public bool usedByMelee; //근접공격시 발동
    public bool usedBySkill; //스킬시전시 발동
    public bool usedByHit; //피격시 발동
    public List<PlayerSkill> activeSkillTypeList;

    public float effectCooldown;
    protected float _lastEffectTime;

    private void OnEnable()
    {
        _lastEffectTime = -3000f;
    }

    protected bool CheckType(EffectType type)
    {
        bool meleeCheck = type == EffectType.Melee && usedByMelee;
        bool skillCheck = type == EffectType.Skill && usedBySkill;
        bool hitCheck = type == EffectType.Hit && usedByHit;

        return meleeCheck || skillCheck || hitCheck;
    }

    public bool AttemptUseEffect(EffectType type)
    {
        if (_lastEffectTime + effectCooldown > Time.time) return false;
        if (!CheckType(type)) return false;

        if(UseEffect())
        {
            _lastEffectTime = Time.time;
        }
        return false;
    }

    public abstract bool UseEffect();
}
