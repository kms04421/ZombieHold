using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    private float spawnInterval = 0.1f;
    private Coroutine spawnCoroutine;

    
    private IEnumerator SpawnZombies(int dayCount)
    {
        int spawnMaxCount =20 + (dayCount *2);
        int spawnCount = 0;
        while (spawnCount < spawnMaxCount)
        {
            spawnCount++;   
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 10f;
            spawnPos.y = 0;

            ZombiePoolManager.Instance.GetZombie(spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StartSpawning(int dayCount)
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnZombies(dayCount));
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
