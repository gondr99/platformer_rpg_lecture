using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private float _disappearDistance = 1f;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;
    private Player _player;
    private SwordSkill _swordSkill;
    
    private bool _canSpin = true;
    private bool _isReturning = false;
    private float _returnSpeed;
    private float _lifeTime = 0f;
    private bool _isDestroyed = false;

    #region bouncing variable
    
    private List<Enemy> _targetList = new List<Enemy>();
    private int _targetIndex;
    private int _currentBounceCount = 0;

    #endregion

    #region pierce variable
    private int _pierceAmount; //현재 관통한 횟수.
    #endregion

    #region spinner variables

    private float _maxTravelDistance;
    private float _spinTimer;
    private bool _wasStopped;
    private bool _isSpining;
    private float _hitTimer;
    private float _hitCooldown;
    private float _spinXDirection; //스피너가 나가는 X방향.
    private Collider2D[] hitColliders;
    #endregion
    
    
    private readonly int _spinHash = Animator.StringToHash("Spin");
    
    private void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        _animator = visualTrm.GetComponent<Animator>();
        _spriteRenderer = visualTrm.GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
 
    }
    
    public void SetupSword(Vector2 dir, float gravityScale, Player player, SwordSkill swordSkill, float returnSpeed)
    {
        _rigidbody.velocity = dir;
        _rigidbody.gravityScale = gravityScale;
        _player = player;
        _swordSkill = swordSkill;
        _returnSpeed = returnSpeed;

        _pierceAmount = swordSkill.pierceAmount; //관통횟수 저장
        
        //관통소드일경우 회전하지 않게 해주고.
        if(swordSkill.swordSkillType != SwordSkillType.Pierce)
            _animator.SetBool(_spinHash, true);

        if (swordSkill.swordSkillType == SwordSkillType.Spin)
        {
            _spinXDirection = Mathf.Clamp(_rigidbody.velocity.x, -1, 1);//노멀라이즈된 방향값.
            hitColliders = new Collider2D[_swordSkill.maxHitTargetCount]; 
        }
        
        _lifeTime = _swordSkill.destroyTimer; //시간제한 설정
        _isDestroyed = false;
        
    }
    
    private void Update()
    {
        if (_isDestroyed) return; //이미 파괴된 칼이면 
        
        if(_canSpin)
            transform.right = _rigidbody.velocity;

        if (_isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                _returnSpeed * Time.deltaTime);

            //플레이어와 가까워졌다면 삭제.
            float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
            if (distanceToPlayer < _disappearDistance)
            {
                _swordSkill.CatchSword();
            }

            return;
        }
        
        if (_swordSkill.swordSkillType == SwordSkillType.Bounce )
        {
            BounceProcess();
        }

        if (_swordSkill.swordSkillType == SwordSkillType.Spin)
        {
            SpinProcess();
        }

        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0) //라이프타임이 끝났다면
        {
            _isDestroyed = true;
            _swordSkill.DestroyGenerateSword(); //만들어진 소드를 널로 만들고
            _spriteRenderer.DOFade(0, 0.8f).OnComplete(() => Destroy(gameObject));
        }
        
    }

    
    #region implement for sword type
    private void SpinProcess()
    {
        if (!_wasStopped)
        {
            float distance = Vector2.Distance(_player.transform.position, transform.position);
            if (distance > _swordSkill.maxTravelDistance )
            {
                StopSpinSword();
            }
        }

        //이미 정지된 상태라면 갈갈이 시작.
        if (_wasStopped)
        {
            _spinTimer -= Time.deltaTime;

            //천천히 앞으로 전진
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x + _spinXDirection, transform.position.y), 1.5f * Time.deltaTime);

            if (_spinTimer < 0)
            {
                _isReturning = true;
            }

            _hitTimer -= Time.deltaTime;
            if (_hitTimer < 0)
            {
                _hitTimer = _hitCooldown;
                //여기서 데미지 주는 식.
                int count = Physics2D.OverlapCircle(
                    transform.position,
                    _circleCollider.radius + 0.5f, 
                    new ContactFilter2D{layerMask = _swordSkill.whatIsEnemy, useLayerMask = true}, 
                    hitColliders);
                for (int i = 0; i < count; ++i)
                {
                    if (hitColliders[i].TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        DamageToTarget(enemy);
                    }
                }
            }
        }
    }
    
    //stop spin sword's forward
    private void StopSpinSword()
    {
        _wasStopped = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _swordSkill.spinDuration;
    }
    
    private void BounceProcess()
    {
        //적에게 맞아서 리스트를 뽑았다면.
        if (_targetList.Count > 1)
        {
            Enemy currentTarget = _targetList[_targetIndex];

            transform.position = Vector2.MoveTowards(
                transform.position,
                currentTarget.transform.position, _swordSkill.bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.transform.position) < 0.1f)
            {
                bool isDead = DamageToTarget(currentTarget);

                if (isDead)
                {
                    _targetList.RemoveAt(_targetIndex);
                    if (_targetList.Count <= 1) //남은 리스트가 1개 이하면
                    {
                        _isReturning = true;
                        return;
                    }
                }
                
                _targetIndex = (_targetIndex + 1) % _targetList.Count;
                ++_currentBounceCount;
                //한계만큼 다 튕겼다면.
                if (_currentBounceCount >= _swordSkill.bounceAmount)
                {
                    _isReturning = true;
                }
            }
        }
        
    }
    
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isReturning)
            return;

        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (enemy.isDead) return; //죽은 적은 무시.
            bool dead = DamageToTarget(enemy);
            if (_swordSkill.swordSkillType == SwordSkillType.Bounce && _targetList.Count <= 0)
            {
                _targetList = _swordSkill.FindEnemiesInRange(transform, _swordSkill.bouncingRadius);
            }
            else if (_swordSkill.swordSkillType == SwordSkillType.Regular && dead)
            {
                ReturnSword();
                return;
            }
        }
        
        //해당 오브젝트에 꼽혀서 정지되도록.
        StuckIntoTarget(other);
    }

    private bool DamageToTarget(Enemy enemy)
    {
        Vector2 direction = (enemy.transform.position - transform.position).normalized;
        int statDamage = 10; //나중에 Stat시스템을 통해 불러올거다.

        if (_swordSkill.swordSkillType == SwordSkillType.Pierce)
        {
            statDamage = Mathf.RoundToInt(statDamage * _swordSkill.pierceDamageMultiplier *
                                          (_pierceAmount / (float)_swordSkill.pierceAmount));
        }
        
        int damage = Mathf.RoundToInt( statDamage * _swordSkill.damageMultiplier ); //배율에 따라 증뎀.
        //데미지 줄때마다 소드 스킬 피드백 발동시키기.(소드는 UseSkill을 안써)
        SkillManager.Instance.UseSkillFeedback(PlayerSkill.Sword);
        
        return enemy.HealthCompo.ApplyDamage(damage, direction, _swordSkill.knockbackPower, _player);
    }

    private void StuckIntoTarget(Collider2D other)
    {
        bool isEnemy = other.GetComponent<Enemy>() != null;
        if (_pierceAmount > 0 && isEnemy)
        {
            //적에게 맞혔으나 _pierce횟수가 더 남아있다면 피어싱
            --_pierceAmount;
            return;
        }
        
        //스핀검이 최초로 적과 접지하는 순간 스핀을 멈추고 갈갈이 
        if (_swordSkill.swordSkillType == SwordSkillType.Spin && isEnemy)
        {
            if(!_wasStopped)
                StopSpinSword();
            return;
        }
        
        _canSpin = false; //더이상 회전하지 않게 
        _circleCollider.enabled = false;

        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_swordSkill.swordSkillType == SwordSkillType.Bounce && _targetList.Count > 1) return;
        
        _animator.SetBool(_spinHash, false);
        transform.parent = other.transform;
    }
    
    public void ReturnSword()
    {
        // 모든 제약조건을 걸어서 물리 영향을 안받게하고 transform으로 땡긴다.
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        //컬라이더는 키면 안돼. 그럼 또다시 흡수돼.
        _isReturning = true; //돌아오도록 설정
    }
}
