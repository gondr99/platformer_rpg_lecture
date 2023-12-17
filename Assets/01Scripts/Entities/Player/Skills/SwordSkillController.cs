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

        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0) //라이프타임이 끝났다면
        {
            _isDestroyed = true;
            _swordSkill.DestroyGenerateSword(); //만들어진 소드를 널로 만들고
            _spriteRenderer.DOFade(0, 0.8f).OnComplete(() => Destroy(gameObject));
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isReturning)
            return; 
        
        //해당 오브젝트에 꼽혀서 정지되도록.
        StuckIntoTarget(other);
    }
    
    private void StuckIntoTarget(Collider2D other)
    {
        _canSpin = false; //더이상 회전하지 않게 
        _circleCollider.enabled = false;

        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

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
