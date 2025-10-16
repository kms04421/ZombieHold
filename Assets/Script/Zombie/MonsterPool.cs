using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using YourGame.AI;

public class MonsterPool : ObjectPoolBase<Zombie>
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
        // 서버나 설정에서 몬스터 이름을 받아옴
        string monsterKey = "Monsters/" + GetMonsterNameFromServer();

        // Addressables로 프리팹을 비동기로 로드
        Addressables.LoadAssetAsync<GameObject>(monsterKey).Completed += OnMonsterLoaded;
    }
    /// <summary>
    /// 로드한 비동기 오브젝트로 풀 채워둠
    /// </summary>
    /// <param name="op"></param>
    private void OnMonsterLoaded(AsyncOperationHandle<GameObject> op)
    {
        if (op.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load monster prefab: {op.DebugName}");
            return;
        }

        // 로드 완료된 프리팹
        GameObject loadedPrefab = op.Result;

        // 풀 미리 채우기
        for (int i = 0; i < PoolSize; i++)
        {
            Zombie zombie = Instantiate(loadedPrefab, transform).GetComponent<Zombie>();
            zombie.gameObject.SetActive(false);
            pool.Enqueue(zombie);
        }
    }
    /// <summary>
    /// 서버에서 이름 가져오기
    /// </summary>
    /// <returns></returns>
    private string GetMonsterNameFromServer()
    {
        // 서버에서 몬스터 이름받기
        return "Zombie";
    }
}
