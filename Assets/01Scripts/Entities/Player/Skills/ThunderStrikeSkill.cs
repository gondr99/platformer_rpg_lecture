using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThunderStrikeSkill : Skill
{
    [Header("Skill info")]
    [SerializeField] private ThunderStrikeController _skillPrefab;
    public float effectRadius = 15f;
    public float activePercent = 30; //30퍼 확률로 발동.
    public int amountOfThunder = 1; //떨어지는 번개 수 

    private List<Enemy> _targetList = new List<Enemy>();

    private bool _isActivating; //활성화된 상태에서 또 활성화되지 않도록
    
    [Header("Ailment")] 
    public bool isShockable; //감전가능
    public float shockPercent;

    [Header("Skill tree")] 
    [SerializeField] private SkillTreeSlotUI _skillEnable;
    [SerializeField] private SkillTreeSlotUI _increaseAmount;
    [SerializeField] private SkillTreeSlotUI _addAilment;

    #region skilltree

    private void Awake()
    {
        _skillEnable.UpgradeEvent += HandleSkillEnableEvent;
        _increaseAmount.UpgradeEvent += HandleIncreaseAmountEvent;
        _addAilment.UpgradeEvent += HandleAddAilmentEvent;
    }

    private void OnDestroy()
    {
        _skillEnable.UpgradeEvent -= HandleSkillEnableEvent;
        _increaseAmount.UpgradeEvent -= HandleIncreaseAmountEvent;
        _addAilment.UpgradeEvent -= HandleAddAilmentEvent;
    }

    private void HandleSkillEnableEvent(int currentCount)
    {
        skillEnabled = true;
        activePercent = 20 + currentCount * 10f;
    }

    private void HandleIncreaseAmountEvent(int currentCount)
    {
        amountOfThunder = 1 + currentCount;
    }

    private void HandleAddAilmentEvent(int currentCount)
    {
        isShockable = true;
        shockPercent = 30f + currentCount * 10f;
    }

    #endregion
    
    public override void UseSkill()
    {
        if (!skillEnabled) return; //비활성화시 작동안함.
        if (_isActivating) return; //이미 사용중이라면 작동안함
        base.UseSkill();

        if (Random.Range(0, 100) > activePercent)
            return;

        //확률 통과했다면 시작.

        FillTargetList();
        StartCoroutine(DamageToTargets());
    }

    //확률이니 이펙트 상관없이 발동하는것
    public override void UseSkillWithoutCooltimeAndEffect()
    {
        if (_isActivating) return; //이미 사용중이라면 작동안함
        FillTargetList();
        StartCoroutine( DamageToTargets());
    }

    private IEnumerator DamageToTargets()
    {
        Vector3 offset = new Vector3(0, 3.5f); //머리위에서 부터
        foreach (Enemy enemy in _targetList)
        {
            if (enemy == null || enemy.gameObject == null) continue;

            ThunderStrikeController thunderInstance = Instantiate(_skillPrefab, enemy.transform.position + offset, Quaternion.identity);
            thunderInstance.SetupThunder(this, enemy);
            yield return new WaitForSeconds(0.3f);
        }
        //코루틴 구동중에 리스트가 고쳐지게 되면 _targetList에서 오류 발생함.
        _isActivating = false; 
    }

    private void FillTargetList()
    {
        _isActivating = true; //활성화시키고 (활성화된 동안 다시 발동 안하도록)

        _targetList.Clear();
        List<Enemy> closeEnemy = FindEnemiesInRange(_player.transform, effectRadius);

        if (closeEnemy.Count == 0) return; //아무것도 없다면 할게 없다.

        //벼락을 떨굴 적들을 찾아와서 넣는다.
        for (int i = 0; i < amountOfThunder; ++i)
        {
            Enemy enemy = closeEnemy[Random.Range(0, closeEnemy.Count)];
            _targetList.Add(enemy);
        }
    }
}
