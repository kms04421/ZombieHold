using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 10f;
            spawnPos.y = 0;

            ZombiePoolManager.Instance.GetZombie(spawnPos, Quaternion.identity);
        }
    }
}
