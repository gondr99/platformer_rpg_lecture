using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStat : ScriptableObject
{
    [Header("Major stat")]
    public Stat strength; // 1포인트당 데미지 증가, 크증뎀 1%
    public Stat agility; // 1포인트당 회피 1%, 크리티컬 1%
    public Stat intelligence; // 1포인트당 마법데미지 1증가, 마법저항 3증가, 도트 데미지에 지능의 10% 증뎀(지능10당 도트뎀 10퍼 증가)
    public Stat vitality; // 1포인트당 체력 5증가.


    [Header("Defensive stats")]
    public Stat maxHealth; //체력
    public Stat armor; //방어도
    public Stat evasion; //회피도
    public Stat magicResistance; //마법방어 (저항 퍼센트로 이 값만큼 각종 데미지 저항)

    [Header("Offensive stats")]
    public Stat damage;
    public Stat criticalChance;
    public Stat criticalDamage;


    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat ignitePercent;
    public Stat iceDamage;
    public Stat chillPercent;
    public Stat lightingDamage;
    public Stat shockPercent;

    public Stat ailmentDuration;

    protected Entity _owner;

    protected Dictionary<StatType, Stat> _statDictionary;

    public virtual void SetOwner(Entity owner)
    {
        _owner = owner;
    }

    public virtual void IncreaseStatBy(int modifyValue, float duration, Stat statToModify)
    {
        _owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
    }

    private IEnumerator StatModifyCoroutine(int modifyValue, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifyValue);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifier(modifyValue);
    }

    protected virtual void OnEnable()
    {
         _statDictionary = new Dictionary<StatType, Stat>();
    }

    public int GetMeleeDamage()
    {
        return damage.GetValue() + strength.GetValue();
    }

    public bool CanEvasion()
    {
        int total = evasion.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < total)
        {
            return true; //회피 성공. 차후 메시지 띄워주기.
        }
        return false;
    }

    public int ArmoredDamage(int incomingDamage)
    {
        //동상일때는 20% 아머 피어싱.차후 구현
        float multiplier = _owner.HealthCompo.ailmentStat.HasAilment(Ailment.Chilled) ? 0.8f : 1f; 
        return Mathf.Max(1, Mathf.RoundToInt(incomingDamage - armor.GetValue() * multiplier));
    }

    public bool IsCritical(ref int incomingDamage)
    {
        int totalCritical = criticalChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritical)
        {
            //크리티컬 증뎀 시키고.
            incomingDamage = CalculateCriticalDamage(incomingDamage);
            return true;
        }
        return false;
    }

    protected int CalculateCriticalDamage(int incomingDamage)
    {
        int percent = criticalDamage.GetValue() + strength.GetValue();
        return Mathf.RoundToInt(incomingDamage * percent * 0.01f); //0.01f 곱하면 백분율이다.
    }

    
    public virtual int GetMagicDamage(StatType magicCategory)
    {
        int value = _statDictionary[magicCategory].GetValue();

        //지능에 해당 매직 데미지를 더한 값을 리턴
        return value + intelligence.GetValue();
    }

    public virtual float GetMagicRegistance()
    {
        return (100 - magicResistance.GetValue()) * 0.01f;
    }

    public int GetMaxHealth()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public virtual int GetMagicDamageAfterResist(int incomingDamage)
    {
        int resistTotal = magicResistance.GetValue() + intelligence.GetValue() * 3;

        return Mathf.Max(1, incomingDamage - resistTotal);
    }
}