using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, Playing, GameOver }

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> PlayerList;

    public GameState state = GameState.MainMenu;
    public int dayCount = 1;
    public bool isNight = false;

    public ZombieSpawner spawner;
   // public UIManager ui;
    protected override void Awake()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        PlayerList.AddRange(playerObjects);
    }

    void Start()
    {
        StartNight();
    }

    public Transform GetPlayer()
    {
       
        int randomIndex = Random.Range(0, PlayerList.Count);
        return PlayerList[randomIndex].transform;
    }

    public void StartDay()
    {
        isNight = false;
    //    ui.ShowMessage("Day " + dayCount + " 시작!");
       spawner.StopSpawning();
    }

    public void StartNight()
    {
        isNight = true;
     //   ui.ShowMessage("Night " + dayCount + " 웨이브!");
        spawner.StartSpawning(dayCount);
    }

    public void EndDay()
    {
        dayCount++;
        StartDay();
    }

    public void EndNight()
    {
        StartDay();
    }

    public void ChangeState(GameState newState)
    {
        state = newState;
        // 필요하면 상태에 맞는 처리
    }
}
