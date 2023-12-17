using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    [SerializeField] private int _attackCategoryCount = 3;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private DamageCaster _damageCaster;

    private Transform _closestEnemy;
    
    private readonly int _attackNumberHash = Animator.StringToHash("ComboCounter");
    private int _facingDirection = 1;

    private CloneSkill _skill;
    private int _comboCounter;
    private int _duplicatedCount;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _damageCaster = transform.Find("DamageCaster").GetComponent<DamageCaster>();
    }

    public void SetupClone(CloneSkill skill, Transform originTrm, Vector3 offset, int duplicatedCount)
    {
        _duplicatedCount = duplicatedCount;
        
        _comboCounter = Random.Range(0, _attackCategoryCount);
        _animator.SetInteger(_attackNumberHash, _comboCounter);
        
        _skill = skill;
        _damageCaster.SetOwner(PlayerManager.Instance.Player); 
        _damageCaster.SetDamageMultiplier(_skill.damageMultiplier);//데미지 셋팅 
        
        transform.position = originTrm.position + offset;
        FacingClosetTarget(); //가장 가까운 적 찾아서 바라보고.
        StartCoroutine( FadeAfterDelay(_skill.cloneDuration));
    }
    
    private IEnumerator FadeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _spriteRenderer.DOFade(0, 0.7f).OnComplete(() =>
        {
            Destroy(gameObject); //페이딩 끝나면 삭제. 굳이 풀매니징까지는 안해도 된다.
        });
    }
    
    //생성되면 가장 가까운 적을 향하도록 함.
    private void FacingClosetTarget()
    {
        _closestEnemy = _skill.FindClosestEnemy(transform, _skill.findEnemyRadius);        

        if (_closestEnemy != null)
        {
            if (transform.position.x > _closestEnemy.position.x)
            {
                _facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
    
    private void AnimationFinishTrigger()
    {
        
    }
    
    private void AttackTrigger()
    {
        bool success = _damageCaster.CastDamage(_comboCounter);

        if (success && _skill.canDuplicateClone && _duplicatedCount < _skill.maxDuplicateCounter)
        {
            if (Random.value < _skill.duplicatePercent * _skill.reducePercentByCount)
            {
                _skill.CreateClone(transform, new Vector3(1.5f * _facingDirection,0,0), _duplicatedCount + 1);
            }
        }
    }
}
