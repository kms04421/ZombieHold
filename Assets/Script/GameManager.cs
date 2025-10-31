using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using YourGame.AI;
public class GameManager : Singleton<GameManager>
{
    [Header("플레이어저장용")]
    public List<PlayerController> PlayerList;

    [Header("보급상자")]
    [SerializeField] private GameObject box;
    //좀비 스포너 구독용
    public static event Action<int> OnSpawnZombie;
    // ID 세팅 완료 이벤트
    public event Action OnPlayerIDAssigned;
    //날짜용
    public int dayCount = 1;
    private void Start()
    {
        StartCoroutine(WaveStart());
        OnSpawnZombie?.Invoke(1);
        Invoke("OnBox", 2);
    }
 
    public void AddMultiPlayer(PlayerSpawnData data)
    {
        Debug.Log("playerSp");
        string Key = "Player/Player";

        Addressables.LoadAssetAsync<GameObject>(Key).Completed += (handle) =>
        {
            GameObject prefab = handle.Result;
            if (prefab != null)
            {
                PlayerController player = Instantiate(prefab).GetComponent<PlayerController>();
                player.playerData.id = data.id;
                MultiClient.Instance.SendPlayerRegisterToServer(player.playerData.id, player.playerData.MaxHp);            
                PlayerList.Add(player);
                if (player.playerData.id == MultiClient.Instance.myPlayerID)
                {
                    TriggerPlayerIDAssigned();
                }
            }
            else
                Debug.Log("Player 프리팹 없습니다");

        };
      
    }
    public void TriggerPlayerIDAssigned()
    {
        OnPlayerIDAssigned?.Invoke();
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
    public PlayerController GetPlayerID
    {
        get
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].playerData.id == MultiClient.Instance.myPlayerID)
                {
                    return PlayerList[i];
                }
            }
            Debug.Log("일치하는 id없음" + MultiClient.Instance.myPlayerID);
            return null;
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
    /// <summary>
    /// 박스 활성화 (조건생각중)
    /// </summary>
    private void OnBox()
    {
        box.SetActive(true);
    }
}
