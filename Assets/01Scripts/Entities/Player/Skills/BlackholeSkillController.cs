using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    private float _cloneAttackTimer;
    
    [SerializeField] private bool _canGrow;

    private readonly int _HashRiffleSpeed = Shader.PropertyToID("_RiffleSpeed");
    private readonly int _HashRiffleCount = Shader.PropertyToID("_RiffleCount");

    private List<Enemy> _hitTargets = new List<Enemy>();
    private List<Enemy> _freezedTarget = new List<Enemy>();
    
    private Material _riffleMat;
    private SpriteRenderer _spriteRenderer;
    
    private bool _cloneAttackReleased; //공격시작을 알리는 불리언 변수.
    private int _remainAttackAmount = 0; //공격횟수를 저장하는 내부 변수
    private bool _skillEnd = false;
    private float _blackholeSkillTimer;
    
    private BlackholeSkill _skill;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _riffleMat = _spriteRenderer.material;
    }
    
    //최초 생성시 스킬 값을 셋팅
    public void InitSkillValue(BlackholeSkill blackholeSkill)
    {
        _skill = blackholeSkill;
        _riffleMat.SetFloat(_HashRiffleSpeed, _skill.maxRiffleSpeed);
        _riffleMat.SetFloat(_HashRiffleCount, _skill.maxRiffleCount);
    }

    //블랙홀 생성시에 셋업.
    public void SetUpSkill(BlackholeSkill blackholeSkill)
    {
        _canGrow = true;
        _cloneAttackReleased = false;
        _skillEnd = false;
        _remainAttackAmount = _skill.amountOfAttack;
        
        _hitTargets.Clear();
        _freezedTarget.Clear();
        _blackholeSkillTimer = 0;
    }

    //클론을 풀어서 공격시작.
    public void ReleaseCloneAttack()
    {
        _skillEnd = true; //발동
        
        if (_hitTargets.Count <= 0)
        {
            //ShrinkBlackhole(false); //공격종료
        }
        else
        {
            //PlayerManager.Instance.Player.FadePlayer(true, 0.3f); //플레이어를 페이드 아웃 시키고
            _cloneAttackReleased = true;
        }
    }
}
