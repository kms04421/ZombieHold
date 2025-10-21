using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 제네릭 오브젝트 풀 기본 클래스.
/// Instantiate/Destroy 대신 미리 생성한 객체를 재사용하여 성능을 최적화.
/// </summary>
/// <typeparam name="T">풀에서 관리할 컴포넌트 타입 (예: ItemDrop, Bullet 등)</typeparam>
public abstract class ObjectPoolBase<T> : MonoBehaviour where T : Component
{
    [Header("고정 프리펩")]
    [SerializeField] protected T prefab;
    [Header("풀 생산량")]
    [SerializeField] private int poolSize = 20;
    [HideInInspector]public int PoolSize
    {
        get { return poolSize; }
    }
    protected Queue<T> pool = new Queue<T>();


    public virtual T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            return obj;
        }
        else
        {

            return null;
        }
    }

    public virtual void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public virtual int GetPoolCount()
    {
        return pool.Count;
    }
    protected abstract void InitializePool();
}