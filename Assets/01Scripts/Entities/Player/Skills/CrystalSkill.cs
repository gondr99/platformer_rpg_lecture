using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystalType
{
    None = 0,
    Moving = 1,
    Multiple = 2
}
public class CrystalSkill : Skill
{
    [Header("default settings")]
    [SerializeField] private CrystalSkillController _crystalPrefab;
    public float timeOut = 5f;
    
    public float damageMultiplier = 1f;
    public Vector2 knockbackPower;
    
    [Header("Explosion Crystal")] 
    public bool canExplode;
    public float explosionRadius = 3f;

    public CrystalType crystalType;
    [Header("Moving Crystal")]
    public float moveSpeed;
    public float findEnemyRadius = 10f;
    
    [Header("Multiple Crystal")]
    public int amountOfCrystal;
    public float multiCrystalCooldown;
    [HideInInspector] public List<CrystalSkillController> crystalList;
    private bool _readyToLaunch = false;
    //maximum crystal count is 5
    private Vector2[] offsets =
    {
        new Vector2(0.3f, 0.3f),
        new Vector2(-0.3f, 0.3f),
        new Vector2(0.3f, -0.3f),
        new Vector2(-0.3f, -0.3f),
        new Vector2(0, 0)
    };

    [Header("Ailment")] 
    public bool isChillable;
    public float chillingPercent;

    private CrystalSkillController _currentCrystal;

    [Header("skill tree")] 
    [SerializeField] private SkillTreeSlotUI _skillEnable; //5회
    [SerializeField] private SkillTreeSlotUI _addExplosion;  // 3회
    [SerializeField] private SkillTreeSlotUI _addAilment; //5회
    [SerializeField] private SkillTreeSlotUI _addChase; //5회
    [SerializeField] private SkillTreeSlotUI _addMultiple; //5회

    #region skill tree
    //2개의 데미지 증가 스킬슬롯이 있어서 합쳐서 반영한다.
    private float _skillDamageMultiplier = 0;
    private float _chaseDamageMultiplier = 0;
    private void Awake()
    {
        _skillEnable.UpgradeEvent += HandleSkillEnableEvent;
        _addExplosion.UpgradeEvent += HandleExplosionEvent;
        _addAilment.UpgradeEvent += HandleAilmentEvent;
        _addChase.UpgradeEvent += HandleChaseEvent;
        _addMultiple.UpgradeEvent += HandleMultipleEvent;
    }

    private void OnDestroy()
    {
        _skillEnable.UpgradeEvent -= HandleSkillEnableEvent;
        _addExplosion.UpgradeEvent -= HandleExplosionEvent;
        _addAilment.UpgradeEvent -= HandleAilmentEvent;
        _addChase.UpgradeEvent -= HandleChaseEvent;
        _addMultiple.UpgradeEvent -= HandleMultipleEvent;
    }

    private void HandleSkillEnableEvent(int currentCount)
    {
        skillEnabled = true;
        _skillDamageMultiplier = 1 + (currentCount - 1) * 0.2f;
        damageMultiplier = _skillDamageMultiplier + _chaseDamageMultiplier;
    }

    private void HandleExplosionEvent(int currentCount)
    {
        canExplode = true;
        explosionRadius = 1.5f + currentCount * 0.5f;
    }

    private void HandleAilmentEvent(int currentCount)
    {
        isChillable = true;
        chillingPercent = 20 + currentCount * 10f; //최대 70% 확률로 동결
    }

    private void HandleChaseEvent(int currentCount)
    {
        crystalType = CrystalType.Moving;
        _chaseDamageMultiplier = 0.5f + currentCount * 0.2f;
        damageMultiplier = _skillDamageMultiplier + _chaseDamageMultiplier; 
    }

    private void HandleMultipleEvent(int currentCount)
    {
        crystalType = CrystalType.Multiple;
        amountOfCrystal = 2 + currentCount;
        multiCrystalCooldown = 5f;
    }

    #endregion
    
    
    

    public override bool AttemptUseSkill()
    {
        if (_cooldownTimer <= 0 && skillEnabled)
        {
            UseSkill(); //스킬을 사용하고
            return true;
        }
        Debug.Log("Skill cooldown or locked");
        return false;
    }

    public override void UseSkill()
    {
        if (!skillEnabled) return;
        
        base.UseSkill();

        //다중 크리스탈은 조금 다르게 동작한다.
        if (crystalType == CrystalType.Multiple)
        {
            //만들어진 크리스탈이 없다면.
            if (crystalList.Count == 0)
            {
                StartCoroutine(MakeMultipleCrystal());
            }
            else if(_readyToLaunch) //만들어진 것이 있고 발사 가능하다면
            {
                Transform target = FindClosestEnemy(_player.transform, findEnemyRadius);
                if (target != null)
                {
                    //뒤에서부터 1번째
                    crystalList[^1].LaunchToTarget(target);
                    crystalList.RemoveAt(crystalList.Count - 1);

                    if (crystalList.Count == 0)
                    {
                        _cooldownTimer = multiCrystalCooldown;
                    }
                }
            }
            return;
        }
        

        if (_currentCrystal == null)
        {
            CreateCrystal(_player.transform.position); //create crystal on player position
        }
        else
        {
            //플레이어와 위치를 바꾸기
            ExchangeToCrystalPosition();
        }
    }

    private IEnumerator MakeMultipleCrystal()
    {
        //지정된 갯수만큼 크리스탈을 만든다.
        for (int i = 0; i < amountOfCrystal; ++i)
        {
            //Vector2 offset = Random.insideUnitCircle * 0.5f;
            CrystalSkillController instance = Instantiate(_crystalPrefab, _player.backTrm.position + (Vector3)offsets[i], Quaternion.identity);
            instance.transform.localScale = Vector3.one * 0.5f; //절반 크기
            instance.SetupCrystal(this); //5배시간.
            crystalList.Add(instance);
            instance.transform.parent = _player.backTrm; //부모 지정.
            
            instance.StartPulseMove();
            yield return new WaitForSeconds(0.1f);
        }

        _readyToLaunch = true; //발사 준비 완료.
    }
    
    private void ExchangeToCrystalPosition()
    {
        Transform a = _player.transform;
        Transform b = _currentCrystal.transform;
        (a.position, b.position) = (b.position, a.position);
        
        _currentCrystal.EndOfCrystal();
    }
    
    
    public void CreateCrystal(Vector3 position)
    {
        _currentCrystal = Instantiate(_crystalPrefab, position, Quaternion.identity);
        _currentCrystal.SetupCrystal(this);
    }

    public void UnlinkCrystal()
    {
        _currentCrystal = null;
        if(crystalType != CrystalType.Multiple)
            _cooldownTimer = _cooldown;
    }
}
