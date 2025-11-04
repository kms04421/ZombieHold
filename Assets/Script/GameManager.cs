using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class GameManager : Singleton<GameManager>
{
    [Header("플레이어저장용")]
    public Dictionary<string, PlayerController> PlayerDic;

    [Header("보급상자")]
    [SerializeField] private GameObject box;
    //좀비 스포너 구독용
    public static event Action<int> OnSpawnZombie;
    // ID 세팅 완료 이벤트
    public event Action OnPlayerIDAssigned;
    //날짜용
    public int dayCount = 1;
    protected override void Awake()
    {
        base.Awake();
        PlayerDic = new Dictionary<string, PlayerController>();
    }
    private void Start()
    {
        StartCoroutine(WaveStart());
        OnSpawnZombie?.Invoke(1);
        Invoke("OnBox", 2);
    }

    public void AddMultiPlayer(ActorData data)
    {
    
        string Key = "Player/Player";

        Addressables.LoadAssetAsync<GameObject>(Key).Completed += (handle) =>
        {
            GameObject prefab = handle.Result;
            if (prefab != null)
            {
                // 기존 반복문 제거
                if (!PlayerDic.ContainsKey(data.id))
                {
                    PlayerController player = Instantiate(prefab).GetComponent<PlayerController>();
                    player.playerData.id = data.id;
                    PlayerDic.Add(data.id, player);
                    //클라이언트 작동시
                    if (player.playerData.id == MultiClient.Instance.myPlayerID)
                    {
                        
                        TriggerPlayerIDAssigned(); // 자신의 플레이어컨트로러 저장
                    }

                

                    // 서버등록시
                    if (player.playerData.id == MultiClient.Instance.myPlayerID)
                    {
                        MultiClient.Instance.SendPlayerRegisterToServer(player.playerData.id, player.playerData.MaxHp);
                    }
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
    /// PlayerDic에서 Random player정보 get
    /// </summary>
    /// <returns></returns>
    public PlayerController GetPlayer
    {
        get
        {
            int randomIndex = UnityEngine.Random.Range(0, PlayerDic.Count);
            return PlayerDic.Values.ElementAt(randomIndex);
        }
    }
    public PlayerController GetPlayerID
    {
        get
        {
            // 기존 반복문 제거
            if (PlayerDic.ContainsKey(MultiClient.Instance.myPlayerID))
            {
                return PlayerDic[MultiClient.Instance.myPlayerID];
            }
            else
            {
                return null;
            }
        }
    }
    public int GetPlayerCount
    {
        get
        {
            return (int)PlayerDic.Count;
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
