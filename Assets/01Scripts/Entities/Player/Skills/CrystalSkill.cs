using System.Collections.Generic;
using UnityEngine;

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
    
    [Header("Moving Crystal")] 
    public bool canMoveToEnemy;
    public float moveSpeed;
    public float findEnemyRadius = 10f;
    
    [Header("Multiple Crystal")]
    public bool isMultipleCrystal; 
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
    
    private CrystalSkillController _currentCrystal;

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
        _cooldownTimer = _cooldown;
    }
}
