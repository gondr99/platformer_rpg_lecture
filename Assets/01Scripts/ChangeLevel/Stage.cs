using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Level _firstLevel;

    private void Start()
    {
        _firstLevel.LoadLevel();
    }
}
