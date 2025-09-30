using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public ItemSO item;         // 드랍 아이템
    public float dropChance;  // 확률 (0~1)
    public int minAmount = 1;
    public int maxAmount = 3;
}

[CreateAssetMenu(fileName = "ZombieLootTable", menuName = "Loot/ZombieLootTable")]
public class LootTable : ScriptableObject
{
    public int minGold = 5;
    public int maxGold = 20;
    public LootEntry[] lootItems;
}