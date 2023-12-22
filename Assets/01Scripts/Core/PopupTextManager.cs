using UnityEngine;

public enum DamageCategory
{
    Normal = 0,
    Critical = 1,
    Heal = 2,
    Debuff = 3,
}

public class PopupTextManager : MonoSingleton<PopupTextManager>
{
    public bool popupDamageText; //옵션창에서 조절
    public bool israndomTextPos;

    [Header("normal, critical, heal, debuff")]
    [ColorUsage(true, true)]
    [SerializeField] private Color[] _textColors;
    [SerializeField] private float[] _textSizes;

    public void PopupDamageText(Vector3 position, string value, DamageCategory category)
    {
        if (!popupDamageText) return; //텍스트가 뜨기로 되어 있을 때만 띄운다.

        if(israndomTextPos)
        {
            position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        DamageText _damageText = PoolManager.Instance.Pop(PoolingType.DamageText) as DamageText;

        int idx = (int)category;
        _damageText.ShowDamageText(position, value, _textSizes[idx], _textColors[idx]);
    }

}
