using UnityEngine;

public class ThunderAnimation : MonoBehaviour
{
    private void HitAnimationEndTrigger()
    {
        Destroy(transform.parent.gameObject);
    }
}
