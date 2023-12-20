using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Ailment : int
{
    None = 0,
    Ignited = 1, // 도트데미지 주는 효과 3초에 걸쳐 0.3초당 3씩 
    Chilled = 2, // 4초간 아머 -20 감소
    Shocked = 4  // 피격시마다 쇼크 데미지 추가.(받는 데미지의 10%, 최소 3데미지)
}

public delegate void AilmentChange(Ailment oldAilment, Ailment newAilment);

public delegate void AilmentDotDamageEvent(Ailment ailmentType, int damage);
[Serializable]
public class AilmentStat 
{
    private Dictionary<Ailment, float> _ailmentTimerDictionary;
    private Dictionary<Ailment, int> _ailmentDamageDictionary;

    
    public Ailment currentAilment; //질병 및 디버프 상태

    public event AilmentDotDamageEvent DotDamageEvent; //상태이상 종류와 데미지 이벤트
    public event AilmentChange AilmentChangeEvent; // 상태이상 종료시 발생

    private float _igniteTimer;
    private float _igniteDamageCooldown = 0.5f; //도트데미지는 0.5초틱으로
    
    public AilmentStat()
    {
        //처음 만들어질 때 각 종류별로 타이머와 데미지를 기록함.
        _ailmentTimerDictionary = new Dictionary<Ailment, float>();
        _ailmentDamageDictionary = new Dictionary<Ailment, int>();

        foreach (Ailment ailment in Enum.GetValues(typeof(Ailment)))
        {
            if (ailment != Ailment.None)
            {
                _ailmentTimerDictionary.Add(ailment, 0f);
                _ailmentDamageDictionary.Add(ailment, 0); //데미지와 쿨타임 초기화
            }
        }
    }

    public void UpdateAilment()
    {
        foreach (Ailment ailment in Enum.GetValues(typeof(Ailment)))
        {
            if(ailment == Ailment.None) continue;

            if (_ailmentTimerDictionary[ailment] > 0) //타이머가 돌고 있다면.
            {
                _ailmentTimerDictionary[ailment] -= Time.deltaTime;
                if (_ailmentTimerDictionary[ailment] <= 0)
                {
                    Ailment oldAilment = currentAilment;
                    currentAilment ^= ailment; //XOR로 빼주고
                    AilmentChangeEvent?.Invoke(oldAilment, currentAilment);
                }
            }
        }

        //DOT 데미지들은 여기서 처리.
        IgniteTimer();
    }

    private void IgniteTimer() //점화의 경우 틱데미지를 줘야 하니까.
    {
        if( (currentAilment & Ailment.Ignited) == 0 ) return;
        
        _igniteTimer += Time.deltaTime;
        if (_ailmentTimerDictionary[Ailment.Ignited] > 0 && _igniteTimer > _igniteDamageCooldown)
        {
            _igniteTimer = 0;
            DotDamageEvent?.Invoke(Ailment.Ignited, _ailmentDamageDictionary[Ailment.Ignited]);
        }
    }

    //특정 디버프가 존재하는지 체크
    public bool HasAilment(Ailment ailment)
    {
        return (currentAilment & ailment) > 0;
    }
    
    public void ApplyAilments(Ailment value, float duration, int damage)
    {
        Ailment oldValue = currentAilment;
        currentAilment |= value; //현재 상태이상에 추가 상태이상 덧씌우고
        
        if(oldValue != currentAilment) 
            AilmentChangeEvent?.Invoke(oldValue, currentAilment);

        //상태이상 새로 들어온 애들은 시간 갱신해주고. 
        if ((value & Ailment.Ignited) > 0)
        {
            SetAilment(Ailment.Ignited,duration:duration, damage: damage);
        }
        else if ((value & Ailment.Chilled) > 0)
        {
            SetAilment(Ailment.Chilled, duration, damage);
        }
        else if ((value & Ailment.Shocked) > 0)
        {
            SetAilment(Ailment.Shocked, duration, damage);
        }
    }
    
    //질병효과와 지속시간 셋팅
    private void SetAilment(Ailment ailment, float duration, int damage)
    {
        _ailmentTimerDictionary[ailment] = duration;
        _ailmentDamageDictionary[ailment] = damage;
    }
}
