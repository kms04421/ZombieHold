using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> PlayerList;

    public int dayCount = 1;
    public bool isNight = false;

    public ZombieSpawner spawner;
    // public UIManager ui;
    protected override void Awake()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        PlayerList.AddRange(playerObjects);
    }
    private void Start()
    {
        StartCoroutine(DayLoop());
    }

    public Transform GetPlayer()
    {

        int randomIndex = Random.Range(0, PlayerList.Count);
        return PlayerList[randomIndex].transform;
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
