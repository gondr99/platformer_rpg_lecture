using UnityEngine;
public abstract class PoolableMono : MonoBehaviour
{
    public PoolingType poolingType;
    public abstract void ResetPooingItem();
}