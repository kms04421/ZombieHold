using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 모든 풀을 관리
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<System.Type, object> pools = new Dictionary<System.Type, object>();

    /// <summary>
    /// 풀 등록
    /// </summary>
    public void RegisterPool<T>(ObjectPoolBase<T> pool) where T : Component
    {
        pools[typeof(T)] = pool;
    }
    /// <summary>
    /// 타입 기반으로 풀 접근
    /// </summary>
    public ObjectPoolBase<T> GetPool<T>() where T : Component
    {
        if (pools.TryGetValue(typeof(T), out var pool))
            return pool as ObjectPoolBase<T>;
        return null;
    }
}
