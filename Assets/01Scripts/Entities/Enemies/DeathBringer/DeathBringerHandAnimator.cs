using UnityEngine;

public class DeathBringerHandAnimator : MonoBehaviour
{
    private DeathBringerHand _hand;

    private void Awake()
    {
        _hand = transform.parent.GetComponent<DeathBringerHand>();
    }

    private void AttackTrigger()
    {
        _hand.Attack();
    }

    private void EndOfAnimationTrigger()
    {
        _hand.EndOfAnimation();
    }
}
