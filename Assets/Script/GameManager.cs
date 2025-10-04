using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public List<PlayerController> PlayerList;

    public int dayCount = 1;
    public bool isNight = false;

    public ZombieSpawner spawner;
    // public UIManager ui;
    protected override void Awake()
    {
        PlayerController[] playerObjects = GameObject
     .FindGameObjectsWithTag("Player")
     .Select(go => go.GetComponent<PlayerController>())
     .Where(pc => pc != null)
     .ToArray();

        PlayerList.AddRange(playerObjects);
    }
    private void Start()
    {
        StartCoroutine(DayLoop());
    }

    public PlayerController GetPlayer()
    {

        int randomIndex = Random.Range(0, PlayerList.Count);
        return PlayerList[randomIndex];
    }

    public IEnumerator DayLoop()
    {
        while (true)
        {
            int waitTime = Random.Range(300, 501);
            yield return new WaitForSeconds(waitTime);
            dayCount++;
            yield return StartCoroutine(spawner.SpawnZombies(dayCount));

        }
    }

}
