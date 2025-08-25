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
    public int Defense { get; private set; }
    public PlayerData()
    {
        PlayerName = "test";
        MaxHp = 100;
        Level = 0;
        AttackPower = 0;
        Defense = 1;
    }

    public void IncreaseMaxHp(float amount)
    {
        MaxHp += amount;
    }

    public void IncreaseAttackPower(float amount)
    {
        AttackPower += amount;
    }

    public void LevelUp()
    {
        Level += 1;
    }

    public void SetName(string newName)
    {
        PlayerName = newName;
    }
}
