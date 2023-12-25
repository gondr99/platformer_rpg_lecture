using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownIconUI : MonoBehaviour
{
    [SerializeField] private PlayerSkill _skillType;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private TextMeshProUGUI _cooldownText;
    
    private Skill _targetSkill;
    private void Start()
    {
        _targetSkill = SkillManager.Instance.GetSkill(_skillType);
        _cooldownImage.fillAmount = 0;
        _targetSkill.OnCooldownEvent += HandleCooldown;
        
        HandleCooldown(0, 1f);
    }

    private void OnDestroy()
    {
        _targetSkill.OnCooldownEvent -= HandleCooldown;
    }

    private void HandleCooldown(float current, float max)
    {
        _cooldownImage.fillAmount = current / max;
        if (current <= 0.01f)
        {
            _cooldownText.text = string.Empty;
        }
        else
        {
            _cooldownText.text = current.ToString("n2");
        }
    }

    private void OnValidate()
    {
        if (_skillType != 0)
        {
            gameObject.name = $"SkillCooldownUI - [{_skillType.ToString()}]";
        }
    }
}