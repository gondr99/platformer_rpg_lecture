using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/Effect/Heal")]
public class HealEffectSO : ItemEffectSO
{
    [SerializeField] private bool _percentHeal;
    [SerializeField] private int _healValue;
    [Range(0f, 100f)][SerializeField] private float _healPercent;
    
    public override bool UseEffect()
    {
        //check chance percent
        if (Random.Range(0, 100) > effectChance) return false;

        Player player = PlayerManager.Instance.Player;
        PlayerStat stat = player.Stat as PlayerStat;

        int healAmount = _healValue;
        if (_percentHeal)
        {
            healAmount = Mathf.Max(1, Mathf.RoundToInt(stat.GetMaxHealth() * _healPercent / 100f));
        }

        player.HealthCompo.ApplyHeal(healAmount);

        return true;
    }

}
