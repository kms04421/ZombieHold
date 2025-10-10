using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    [Header("플레이어저장용")]
    public List<PlayerController> PlayerList;

    //좀비 스포너 구독용
    public static event Action<int> OnSpawnZombie;

    //날짜용
    public int dayCount = 1;

    protected override void Awake()
    {
        SetPlayerList();

    }
    private void Start()
    {
        StartCoroutine(WaveStart());
        OnSpawnZombie?.Invoke(1);
    }
    /// <summary>
    /// 플레이어 PlayerList에 세팅
    /// </summary>
    private void SetPlayerList()
    {
        PlayerController[] playerObjects = GameObject
            .FindGameObjectsWithTag("Player")
            .Select(go => go.GetComponent<PlayerController>())
            .Where(pc => pc != null)
            .ToArray();

        PlayerList.AddRange(playerObjects);
    }
    /// <summary>
    /// PlayerList에서 player정보 get
    /// </summary>
    /// <returns></returns>
    public PlayerController GetPlayer
    {
        get
        {
            int randomIndex = UnityEngine.Random.Range(0, PlayerList.Count);
            return PlayerList[randomIndex];
        }
    }
    /// <summary>
    /// 게임 웨이브 시작 
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaveStart()
    {
        while (true)
        {
            int waitTime = UnityEngine.Random.Range(180, 240);
            yield return new WaitForSeconds(waitTime);
            dayCount++;
            OnSpawnZombie?.Invoke(dayCount);

        }
    }

}
