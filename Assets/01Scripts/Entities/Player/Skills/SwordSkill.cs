using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordSkillType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordSkillType swordSkillType = SwordSkillType.Regular;
    
    [Header("Skill info")] 
    [SerializeField] private SwordSkillController _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravity;
    [SerializeField] private float _returnSpeed = 16f;

    public float freezeTime = 0.7f;
    public float damageMultiplier = 1;
    public Vector2 knockbackPower;
    public float returnImpactPower = 8;
    public float destroyTimer = 7f;

    //여기에 각종 정보들
    [Header("Bouncing info")]
    public float bounceSpeed = 20f;
    public int bounceAmount = 4;
    public float bounceGravity = 3f;
    public float bouncingRadius = 10f;
    
    [Header("Pierce info")] 
    public int pierceAmount;
    public float pierceGravity;
    public float pierceDamageMultiplier = 2f;
    
    [Header("Spin info")]
    public float maxTravelDistance = 7;
    public float spinDuration = 2;
    public float spinGravity = 1;
    public float hitCooldown = 0.35f;
    public int maxHitTargetCount = 5;
    public Vector2 spinKnockbackPower;
    
    [Header("Aiming Dots")] 
    [SerializeField] private int _numberOfDots;
    [SerializeField] private float _spaceBetweenDots;
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private Transform _dotsParent;
    private GameObject[] _dots;
    
    private Vector2 _finalDirection;
    private bool _holdKey = false;

    [HideInInspector] public SwordSkillController generatedSword;

    public bool canFreeze; //타격시 적을 순간적으로 프리즈 시키는가?
    
    protected override void Start()
    {
        base.Start();

        GenerateDots(); //점들을 만들어두고.
        SetupGravity(); //현재 스킬의 종류에 맞게 그라비티 셋팅
    }

    protected override void Update()
    {
        base.Update();
        if (_holdKey && generatedSword == null)
        {
            for (int i = 0; i < _dots.Length; ++i)
            {
                _dots[i].transform.position = DotPositionOnT(i * _spaceBetweenDots);
            }
        }
    }
    
    private void SetupGravity()
    {
        if (swordSkillType == SwordSkillType.Bounce)
        {
            _swordGravity = bounceGravity;
        }

        if (swordSkillType == SwordSkillType.Pierce)
        {
            _swordGravity = pierceGravity;
        }
        if (swordSkillType == SwordSkillType.Spin)
        {
            _swordGravity = spinGravity;
        }
    }
    
    public void DestroyGenerateSword()
    {
        generatedSword = null;
    }

    public void ReturnGenerateSword()
    {
        if (generatedSword != null)
        {
            generatedSword.ReturnSword();
        }
    }
    
    public void CreateSword()
    {
        SwordSkillController newSword = Instantiate(_swordPrefab, _player.transform.position, Quaternion.identity);
        
        newSword.SetupSword(_finalDirection, _swordGravity, _player, this, _returnSpeed);
        
        generatedSword = newSword; //만들어진 검을 할당함.
    }
    
    public void CatchSword()
    {
        //칼을 잡았다면 플레이어를 캐치소드로 변경.
        _player.StateMachine.ChangeState(PlayerStateEnum.CatchSword);
        Destroy(generatedSword.gameObject);
        generatedSword = null;
        _cooldownTimer = _cooldown;
    }
    

    //던지는 키가 눌렸는지를 체크
    public void OnThrowAim(bool state)
    {
        _holdKey = state;
        if (!_holdKey) //키가 떼지는 순간.
        {
            _finalDirection = AimDirection().normalized * _launchForce;
        }
    }
    
    #region Guide Dots region

    //가이드 점을 껐다 켰다.
    public void DotsActive(bool state)
    {
        for (int i = 0; i < _dots.Length; ++i)
        {
            _dots[i].SetActive(state);
        }
    }
    
    //가이드라인 그려주는 함수.
    private void GenerateDots()
    {
        _dots = new GameObject[_numberOfDots]; //포인터만 만든거임.
        for (int i = 0; i < _numberOfDots; ++i)
        {
            //만들어서 부모밑에 넣어둔다.(비활성화 상태)
            _dots[i] = Instantiate(_dotPrefab, _player.transform.position, Quaternion.identity, _dotsParent);
            _dots[i].SetActive(false);
        }
    }
    
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = _player.transform.position;
        Vector2 mousePosition = CameraManager.Instance.MainCam.ScreenToWorldPoint(_player.PlayerInput.AimPosition);

        Vector2 direction = mousePosition - playerPosition;
        
        return direction;
    }

    private Vector2 DotPositionOnT(float t)
    {
        Vector2 normalizedAim = AimDirection().normalized;
        
        Vector2 position = (Vector2)_player.transform.position 
                           //+ new Vector2(normalizedAim.x * _launchForce.x, normalizedAim.y * _launchForce.y ) * t
                           + normalizedAim * _launchForce * t
                           + Physics2D.gravity * (_swordGravity * (t*t) * 0.5f);
        // x축은 힘에 시간을 곱하면 되고
        // y축은 힘에 시간을 곱한거에서 중력 * 시간 제곱을 반으로 나눈걸 빼주면 돼. 이때 gravity가 이미 음수이기 때문에 더해준거.
        //만약 mass가 있다면 질량으로 나눠주면 돼.
        return position;
    }
    
    #endregion
}
