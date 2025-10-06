using UnityEngine;

public class PlayerData
{
    public string room;
    public string id;
    public float x;
    public float y;
    public float z;
    public string PlayerName { get; private set; }
    public float MaxHp { get; private set; }
    public int Level { get; private set; }
    public float AttackPower { get; private set; }
    public float Defense { get; private set; }
    public PlayerData()
    {
        PlayerName = "test";
        MaxHp = 100;
        Level = 0;
        AttackPower = 0;
        Defense = 1;
    }

    /// <summary>
    /// 어빌리티 능력치 추가용
    /// </summary>
    /// <param name="_playerData"></param>
    public void AddPlayData(PlayerData _playerData)
    {
        MaxHp += _playerData.MaxHp;
        AttackPower += _playerData.AttackPower;
        Defense += _playerData.Defense;
    }
    /// <summary>
    /// 이름 다시 설정
    /// </summary>
    /// <param name="newName"></param>
    public void SetName(string newName)
    {
        PlayerName = newName;
    }
    /// <summary>
    /// 레벨업
    /// </summary>
    public void LevelUp()
    {
        Level += 1;
    }
    /// <summary>
    /// 최대 HP 설정
    /// </summary>
    /// <param name="amount"></param>
    public void SetMaxHp(float amount)
    {
        MaxHp = amount;
    }
    /// <summary>
    /// 공격력 설정
    /// </summary>
    /// <param name="amount"></param>
    public void SetAttackPower(float amount)
    {
        AttackPower = amount;
    }
    /// <summary>
    /// 방어력 설정
    /// </summary>
    /// <param name="amount"></param>
    public void SetDefense(float amount)
    {
        Defense = amount;
    }
    /// <summary>
    /// 전체 능력치 0으로 초기화
    /// </summary>
    public void ResetPlayData()
    {
        MaxHp = 0;
        Level = 0;
        AttackPower = 0;
        Defense = 0;
    }

}
