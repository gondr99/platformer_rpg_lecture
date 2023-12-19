using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotKeyController : MonoBehaviour
{
    [SerializeField] private KeyParticle _keyParticlePrefab;
    [SerializeField] private HoyKeyIconSO _hotKeyIcon;
    private SpriteRenderer _spriteRenderer;

    private BlackholeSkillController _skillController;
    private Enemy _myEnemy;
    private bool _isDisable = false;
    private Key _myHotKey;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupHotKey(Key hotKey, Enemy enemy, BlackholeSkillController skillController)
    {
        _myHotKey = hotKey;
        _spriteRenderer.sprite = _hotKeyIcon.GetSpriteByKey(_myHotKey);
        _myEnemy = enemy;
        _skillController = skillController;
    }

    private void Update()
    {
        if (Keyboard.current[_myHotKey].wasPressedThisFrame && !_isDisable)
        {
            _skillController.AddEnemyToTargetList(_myEnemy);

            //여기서 파티클로 터지는 효과 만들기
            KeyParticle keyParticle = Instantiate(_keyParticlePrefab, transform.position, Quaternion.identity);
            keyParticle.Play(); 
            
            _isDisable = true;
            _spriteRenderer.sprite = null;
        }
    }
}