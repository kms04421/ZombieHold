using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    private float spawnInterval = 0.1f;

    public IEnumerator SpawnZombies(int dayCount)
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
}
