using System.Collections.Generic;
using UnityEngine;
using YourGame.AI;

public class ZombiePool : ObjectPoolBase<Zombie>
{
    private void Awake()
    {
        InitializePool();
        PoolManager.Instance.RegisterPool(this);
    }
    /// <summary>
    /// 풀을 미리 채워둠
    /// </summary>
    protected override void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            Zombie drop = Instantiate(prefab,transform);  // ObjectPoolBase의 prefab   
            drop.gameObject.SetActive(false);
            pool.Enqueue(drop); // pool은 protected로 열어두면 접근 가능
        }
    }
}
