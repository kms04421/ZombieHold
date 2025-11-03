using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using YourGame.AI;

[Serializable]
public struct MessageData
{
    public string type;
    public string msg;
}

public class MultiClient : Singleton<MultiClient>
{
    private WebSocket ws;
    public Dictionary<string, Zombie> zombies;
    public string myPlayerID;
    // 예시: 간단한 메인 스레드 큐
    private Queue<Action> mainThreadActions = new Queue<Action>();
    void Start()
    {
        zombies = new Dictionary<string, Zombie>();
        ws = new WebSocket("ws://localhost:3000");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("서버 연결 성공!");
            // struct 객체 생성
            MessageData data = new MessageData
            {
                type = "greeting",
                msg = "Hello Server from Unity!"
            };
            string json = JsonUtility.ToJson(data);
            ws.Send(json);
        };

        ws.OnMessage += (sender, e) =>
        {
            // Debug.Log("서버 메시지 수신: " + e.Data);
            OnServerMessage(e.Data); // 반드시 호출
        };

        ws.Connect();
    }
    /// <summary>
    /// 플레이어 정보 서버에 등록
    /// </summary>
    /// <param name="id"></param>
    /// <param name="hp"></param>
    /// <param name="MaxHP"></param>
    public void SendPlayerRegisterToServer(string id, float MaxHP)
    {
        NetworkMessage msg = new NetworkMessage
        {
            type = "registerPlayer",
            data = new ActorData
            {
                id = id,
                hp = MaxHP,
                maxHP = MaxHP
            }
      
        };
        string json = JsonUtility.ToJson(msg);
        ws.Send(json);
    }

    /// <summary>
    /// Player피격시 정보 서버에 보냄
    /// </summary>
    /// <param name="id"></param>
    /// <param name="damage"></param>
    public void SendHitPlayerToServer(string id, float damage)
    {
        NetworkMessage msg = new NetworkMessage
        {
            type = "damagePlayer",
            data = new ActorData
            {
                id = id,
                damage = damage
            }
   
        };
        string json = JsonUtility.ToJson(msg);
        ws.Send(json);
    }

    /// <summary>
    /// 서버에 좀비 정보 보내기
    /// </summary>
    /// <param name="zombieId"></param>
    /// <param name="hp"></param>
    /// <param name="templateName"></param>
    public void SendZombieRegisterToServer(string id, float hp)
    {
        NetworkMessage msg = new NetworkMessage
        {
            type = "registerZombie",
            data = new ActorData
            {
                id = id,
                hp = hp
            }
          
        };

        string json = JsonUtility.ToJson(msg);
        ws.Send(json);

        //  Debug.Log($"서버에 좀비 등록: ID={id}, HP={hp}");
    }
    /// <summary>
    /// 좀비 피격시
    /// </summary>
    /// <param name="id">Zombie id</param>
    /// <param name="damage"></param>
    public void SendHitZombieToServer(string id, float damage)
    {

        NetworkMessage msg = new NetworkMessage
        {
            type = "damageZombie",
            data = new ActorData
            {
                id = id,
                damage = damage
            }

               
        };
        string json = JsonUtility.ToJson(msg);
        ws.Send(json);

        // Debug.Log($"서버 데미지 전송: ID={id} 에게 HP={damage} 줍니다");
    }
    public void SendPlayerPosToSerber(ActorData data)
    {
        NetworkMessage msg = new NetworkMessage
        {
            type = "playerUpdate",
            data = data
        };
        ws.Send(JsonUtility.ToJson(msg));
    }
    void Update()
    {
        while (mainThreadActions.Count > 0)
            mainThreadActions.Dequeue()?.Invoke(); //문제 발생 일부 로직이 작동안함 Unity의 대부분의 API(Animator, NavMeshAgent, GameObject, Coroutine 등)는 메인 스레드에서만 안전하게 호출가능
                                                   //Unity에서 안전하게 처리하려면 UnityMainThreadDispatcher 같은 구조를 사용하거나, 자체 큐를 만들어 Update()에서 처리합니다.
    }

    void OnServerMessage(string json)
    {
        var msg = JsonUtility.FromJson<NetworkMessage>(json);
        switch (msg.type)
        {
            case "zombieHit":
                // Debug.Log(json);
                var multiHit = JsonUtility.FromJson<ZombieHitMessageFromServer>(json);
                if (multiHit.hits != null && multiHit.hits.Length > 0)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        foreach (var h in multiHit.hits)
                        {
                            if (zombies.TryGetValue(h.id, out var zombie))
                            {
                                zombie.OnHit(h.hp, h.dead);
                            }
                        }
                    });
                }
                else
                {
                    // 단일 히트 처리
                    var singleHit = JsonUtility.FromJson<ZombieDataMesage>(json);
                    mainThreadActions.Enqueue(() =>
                    {
                        if (zombies.TryGetValue(singleHit.id, out var zombie))
                            zombie.OnHit(singleHit.hp, singleHit.dead);
                    });
                }
                break;
            case "NewPlayer":
                Debug.Log("NewPlayer : " + msg.data.id);
                mainThreadActions.Enqueue(() =>
                {
                    GameManager.Instance.AddMultiPlayer(msg.data);
                
                });

                break;
            case "existingPlayers":           
                PlayerMessage players = JsonUtility.FromJson<PlayerMessage>(json);
                mainThreadActions.Enqueue(() =>
                {
                    foreach (var pData in players.data)
                    {
                        if (!GameManager.Instance.PlayerDic.ContainsKey(pData.id))
                        {
                            GameManager.Instance.AddMultiPlayer(pData); // 기존 AddMultiPlayer 재사용 가능

                        }

                    }
                });
                break;
            case "AssingID":
                Debug.Log("AssingID : " + msg.data.id);
                myPlayerID = msg.data.id; // 내 로컬 플레이어 ID
               
        
                break;
            case "playerPosUpdate":

                Debug.Log(json);
                Debug.Log("접근" + msg.data.position.x);
                if (GameManager.Instance.PlayerDic.ContainsKey(msg.data.id))
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        // 서버 좌표를 Vector3로 변환
                       
                        Vector3 pos = msg.data.position.ToVector3();
                        // 또는 부드럽게 이동시키고 싶다면
                        Transform t = GameManager.Instance.PlayerDic[msg.data.id].transform;

                        // t.position = Vector3.Lerp(t.position, pos, 0.5f);
                        t.position = pos;
                        Debug.Log("이동중");
                    });
                }
                break;
                // 다른 타입도 처리 가능
        }
    }
    void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
            ws.Close();
    }
}
