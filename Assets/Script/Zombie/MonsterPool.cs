using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using YourGame.AI;

public class MonsterPool : ObjectPoolBase<Zombie>
{
    private List<ZombieData> zombieDatas;
    [SerializeField] private DBManager DBManager;
    private void Awake()
    {
        zombieDatas = new List<ZombieData>();
        InitializePool();
        PoolManager.Instance.RegisterPool(this);
    }
    /// <summary>
    /// 풀을 미리 채워둠
    /// </summary>
    protected override void InitializePool()
    {
        GetMonsterNameFromServer((server) =>
        {
            if (server == null || server.Count == 0)
            {
                Debug.Log("좀비 데이터가 없습니다");
                return;
            }
            zombieDatas = server;

            for (int i = 0; i < 1; i++)
            {
                int index = i;
                string monsterKey = "Monsters/" + zombieDatas[index].name;

                Addressables.LoadAssetAsync<GameObject>(monsterKey).Completed += (handle) =>
                {
                    GameObject prefab = handle.Result;
                    if (prefab != null)
                        OnMonsterLoaded(handle);
                    else
                        Debug.Log(zombieDatas[index].name + "프리팹 없습니다");

                };
            }

        });
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
            zombie.data = zombieDatas[0];
            zombie.gameObject.SetActive(false);
            pool.Enqueue(zombie);
        }
    }
    /// <summary>
    /// 서버에서 정보 가져오기
    /// </summary>
    /// <returns></returns>
    private void GetMonsterNameFromServer(System.Action<List<ZombieData>> onCompleted)
    {
        // 서버에서 몬스터 이름받기
        DBManager.DBZombiesRequest(onCompleted);
    }
}
