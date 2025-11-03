using System.Collections;

using UnityEngine;
using YourGame.AI;
public class ZombieSpawner : MonoBehaviour
{
    //캐싱용
    private Coroutine coroutine; 
    private PoolManager poolManager; //오브젝트 풀메니저

    private WaitForSeconds delay; // 생성 딜레이

    //스폰 주기
    private float spawnInterval = 1f;
    private void Awake()
    {
        
        GameManager.OnSpawnZombie += SpawnZombie; //게임메니저에 있는 OnSpawnZombie에 구독 
        poolManager = PoolManager.Instance;
        delay = new WaitForSeconds(spawnInterval);

    }

    /// <summary>
    /// 좀비 스폰 코루틴시작
    /// </summary>
    /// <param name="day"></param>
    public void SpawnZombie(int day)
    {
        if (coroutine != null) return;
        coroutine = StartCoroutine(SpawnRoutine(day));
    }
    /// <summary>
    /// 좀비를 정해진 카운터만큼 생성
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private IEnumerator SpawnRoutine(int day)
    {
        int zombieCount = day * 2;
        while (zombieCount > 0)
        {
            zombieCount--;
            yield return new WaitUntil(() => poolManager.GetPool<Zombie>().GetPoolCount() > 0);
            Zombie zombie = poolManager.GetPool<Zombie>().Get(); //풀에서 좀비 가져옴
            MultiClient.Instance.zombies.Add(zombie.data.id, zombie);
            zombie.transform.localPosition = transform.localPosition;
            zombie.transform.localRotation = Quaternion.identity;
            zombie.gameObject.SetActive(true);
            MultiClient.Instance.SendZombieRegisterToServer(zombie.data.id, zombie.data.maxHp);
            yield return delay;
        }
        coroutine = null;
    }

}
