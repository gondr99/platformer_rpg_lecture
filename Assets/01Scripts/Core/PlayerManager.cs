using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private Player _player;

    public Player Player => _player;
    public Transform PlayerTrm => _player.transform;
    
    
}
