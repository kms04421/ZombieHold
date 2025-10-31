using System;
using UnityEngine.UIElements;

[Serializable]
public struct RegisterZombieMessage 
{
    public string type;
    public int id;
    public float hp;
}
[Serializable]
public struct ZombieHitMessage
{
    public string type;
    public int id;
    public float damage;
}
[Serializable]
public struct ZombieHitMessageFromServer
{
    public SingleZombieHit[] hits;
}
[Serializable]
public class SingleZombieHit
{
    public int id;
    public float hp;
    public bool dead;
}
[Serializable]
public class ServerMessage
{
    public string type;
    public string id;
    public string data; // 나중에 JSON 문자열로 담아서 타입별로 다시 파싱
}
[Serializable]
public struct RegisterPlayerMessage 
{
    public string type;
    public string id;
    public float currentHP;
    public float maxHP;
}
[Serializable]
public struct PlayerHitMessage
{
    public string type;
    public string id;
    public float damage;
}
[System.Serializable]
public class ServerPacket
{
    public string type; // 패킷 타입 (예: SPAWN_PLAYER, PLAYER_HIT 등)
    public PlayerSpawnData data; // data는 또 다른 클래스(혹은 string)
}
[System.Serializable]
public class PlayerSpawnData
{
    public string id;
    public Position position;
}
[System.Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}