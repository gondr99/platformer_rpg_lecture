using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Enemy _target;

    private Animator _animator;

    private bool _isHit = false;
    private readonly int _hashHitTrigger = Animator.StringToHash("Hit");
    private Transform _visualTrm;
    private ThunderStrikeSkill _skill;
    private void Awake()
    {
        _visualTrm = transform.Find("Visual");
        _animator = _visualTrm.GetComponent<Animator>();
    }

    public void SetupThunder(ThunderStrikeSkill skill, Enemy enemy)
    {
        _target = enemy;
        _skill = skill;
    }

    private void Update()
    {
        if (!_target) //적이 이미 사라졌다면.
        {
            Destroy(gameObject);
            return;
        }

        transform.position =
            Vector2.MoveTowards(transform.position,
                _target.GroundCheckerPosition,
                _speed * Time.deltaTime);
        Vector2 direction = _target.GroundCheckerPosition - transform.position;

        if (Vector2.Distance(transform.position, _target.GroundCheckerPosition) < 0.1f && !_isHit)
        {
            _isHit = true;
            HitProcess(direction);
        }
        else if (!_isHit)
        {
            _visualTrm.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f);
        }
    }

    private void HitProcess(Vector2 direction)
    {
        _visualTrm.rotation = Quaternion.identity;
        transform.localScale = Vector3.one * 1.2f;

        Player player = PlayerManager.Instance.Player;

        int magicDamage = player.PStat.GetMagicDamage(StatType.LightingDamage);
        
        _target.HealthCompo.ApplyMagicDamage(
            magicDamage,
            direction.normalized,
            new Vector2(1.5f, 3f),
            player);

        _animator.SetTrigger(_hashHitTrigger);

    }

    
}
