using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    public void AnimationFinishTrigger()
    {
        _player.AnimationFinishTrigger();
    }
}
