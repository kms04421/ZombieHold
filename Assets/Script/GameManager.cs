using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> PlayerList;

    protected override void Awake()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        PlayerList.AddRange(playerObjects);
    }

    public Transform GetPlayer()
    {
       
        int randomIndex = Random.Range(0, PlayerList.Count);
        return PlayerList[randomIndex].transform;
    }
}
