using UnityEngine;

public class MonsterDrop : MonoBehaviour
{
    [SerializeField] private ItemSO[] dropItems; // 드랍 가능 아이템 리스트

    /// <summary>
    /// 몬스터 사망 시 호출
    /// </summary>
    public void DropLoot()
    {
        foreach (var item in dropItems)
        {
            if (Random.value <= item.dropChance)
            {
                int dropCount = Random.Range(item.minDropCount, item.maxDropCount + 1);

                SpawnItem(item, dropCount);
                break;
            }
        }
    }

    /// <summary>
    /// 아이템 생성 (풀에서 가져옴)
    /// </summary>
    private void SpawnItem(ItemSO item, int dropCount)
    {
        // 풀에서 가져오기
        var pool = PoolManager.Instance.GetPool<ItemDrop>();
        if (pool == null) return;

        ItemDrop itemDrop = pool.Get();
        Vector3 spawnPos = transform.localPosition;  // 몬스터 월드 위치 기준
        spawnPos.y += 1f;
        itemDrop.transform.localPosition = spawnPos; // 몬스터 위치 기준
        itemDrop.transform.localRotation = Quaternion.identity;
        itemDrop.gameObject.SetActive(true);
        itemDrop.SetItme(item, dropCount);


    }
}