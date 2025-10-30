using System;

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
    public string data; // 나중에 JSON 문자열로 담아서 타입별로 다시 파싱
}