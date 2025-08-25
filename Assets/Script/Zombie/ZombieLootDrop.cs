using UnityEngine;

public class ZombieLootDrop : MonoBehaviour
{
    public LootTable lootTable;

    public void DropLoot()
    {
        if (lootTable == null) return;

        // 골드 드랍
        int gold = Random.Range(lootTable.minGold, lootTable.maxGold + 1);
        Debug.Log("드랍 골드: " + gold);
       // PlayerInventory.instance.AddGold(gold);

        // 아이템 드랍
        foreach (var entry in lootTable.lootItems)
        {
            if (Random.value <= entry.dropChance)
            {
                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                Debug.Log(entry.item.itemName + " 드랍 x" + amount);

                // 인벤토리에 추가
//                PlayerInventory.instance.AddItem(entry.item.id, amount);

                // 월드에 떨어지는 프리팹 생성 가능
                if (entry.item.prefab != null)
                {
                    Instantiate(entry.item.prefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
}