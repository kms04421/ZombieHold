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
        // 부모 영향 제거 (풀 오브젝트 회전/이동 방지)
        itemDrop.transform.SetParent(null, false);

        // 좀비의 현재 월드 위치를 기준으로 스폰
        Vector3 spawnPos = transform.position;
        spawnPos.y += 1f; // 살짝 위로 띄우기 (필요시 조정 가능)

        itemDrop.transform.position = spawnPos;
        itemDrop.transform.rotation = Quaternion.identity;

        // 활성화 및 데이터 세팅
        itemDrop.gameObject.SetActive(true);
        itemDrop.SetItme(item, dropCount);


    }
}