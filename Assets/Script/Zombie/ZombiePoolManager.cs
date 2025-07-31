using System.Collections.Generic;
using UnityEngine;

public class ZombiePoolManager : MonoBehaviour
{
    public static ZombiePoolManager Instance;

    [Header("좀비 프리팹")]
    public GameObject zombiePrefab;

    [Header("풀 크기")]
    public int poolSize = 20;

    [SerializeField]private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        // 풀 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.SetActive(false);
            pool.Enqueue(zombie);
        }
    }

    public GameObject GetZombie(Vector3 position, Quaternion rotation)
    {
        GameObject zombie;

        if (pool.Count > 0)
        {
            zombie = pool.Dequeue();
        }
        else
        {
            zombie = Instantiate(zombiePrefab);
        }

        zombie.transform.position = position;
        zombie.transform.rotation = rotation;
        zombie.SetActive(true);
        
        return zombie;
    }

    public void ReturnZombie(GameObject zombie)
    {
        Debug.Log(pool.Count);
        zombie.SetActive(false);
        pool.Enqueue(zombie);
    }
}
