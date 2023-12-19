using System;
using System.Collections.Generic;

public enum PlayerSkill
{
    Dash = 1,
    Clone = 2,
    Sword = 3,
    Blackhole = 4,
    Crystal = 5,
    //ThunderStrike = 6
}

public class SkillManager : MonoSingleton<SkillManager>
{
    private Dictionary<Type, Skill> _skills;
   
    private void Awake()
    {
        _skills = new Dictionary<Type, Skill>();

        foreach (PlayerSkill skill in Enum.GetValues(typeof(PlayerSkill)))
        {
            Skill skillComponent = GetComponent($"{skill}Skill") as Skill;
            Type type = skillComponent.GetType();
            _skills.Add(type, skillComponent);
        }
    }

    //GetSkill by type
    public T GetSkill<T>() where T : Skill
    {
        Type t = typeof(T);
        if (_skills.TryGetValue(t, out Skill target))
        {
            return target as T;
        }
        return null;
    }

    //GetSkill by enum
    public Skill GetSkill(PlayerSkill skill)
    {
        Type type = Type.GetType($"{skill}Skill");
        if (type == null) return null;
        
        if (_skills.TryGetValue(type, out Skill target))
        {
            return target;
        }
        return null;
    }

    public void UseSkillFeedback(PlayerSkill skillType)
    {
        //check active effect when using skill
        
    }
}
