using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static PoolManager Instance;

    private Dictionary<PoolingType, Pool<PoolableMono>> _pools = new Dictionary<PoolingType, Pool<PoolableMono>>();

    private Transform _trmParent;
    public PoolManager(Transform trmParent)
    {
        _trmParent = trmParent;
    }

    public void CreatePool(PoolableMono prefab, PoolingType poolingType, int count = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, poolingType, _trmParent, count);
        _pools.Add(poolingType, pool);
    }

    public PoolableMono Pop(PoolingType type)
    {
        if (!_pools.ContainsKey(type))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }

        PoolableMono item = _pools[type].Pop();
        item.ResetPooingItem();
        return item;
    }

    public void Push(PoolableMono obj, bool resetParent = false)
    {
        if(resetParent)
            obj.transform.parent = _trmParent;
        _pools[obj.poolingType].Push(obj);
    }

}