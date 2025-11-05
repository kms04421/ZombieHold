using System;
using UnityEngine;

[System.Serializable]
public struct PlayerMessage
{
    public string type; // 메시지 타입 ("existingPlayers", "NewPlayer" 등)
    public ActorData[] data; // 플레이어 정보 배열
}

// 제네릭 메시지 구조
[Serializable]
public class NetworkMessage
{
    public string type;     // "NewPlayer", "ExistingPlayers", "ActorUpdate", "ActorHit"
    public ActorData data;
}

[Serializable]
public struct ActorData
{
    public string id;
    public float hp;
    public float maxHP; // 플레이어만 필요
    public float damage;
    public bool isbool; // 모든 bool값이용 처리용
    public PositionData position;
    public RotationData rotation;
    public string equippedWeapon; // 무기이름
}
[Serializable]
public class ActorListMessage
{
    public string type;     // "ExistingPlayers" 등
    public ActorData[] data;
}
[System.Serializable]
public struct PositionData
{
    public float x;
    public float y;
    public float z;

    public Vector3 ToVector3() => new Vector3(x, y, z);

    
}
[System.Serializable]
public struct RotationData
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
    public static RotationData FromQuaternion(Quaternion q)
    {
        return new RotationData
        {
            x = q.x,
            y = q.y,
            z = q.z,
            w = q.w
        };
    }
}

[Serializable]
public struct ZombieHitMessageFromServer
{
    public ZombieDataMesage[] hits;
}
[Serializable]
public struct ZombieDataMesage
{
    public string id;
    public float hp;
    public bool dead;
}