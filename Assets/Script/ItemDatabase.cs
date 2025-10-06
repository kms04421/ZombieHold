using System.Collections.Generic;
using UnityEngine;
public class ItemDatabase : Singleton<ItemDatabase>
{
    public List<ItemSO> allItems;

    public void LoadItemsFromDB()
    {
        //  allItems = DBManager.GetAllItems(); // DB에서 전체 아이템 조회
    }

    public List<Item> GetRandomItems(int minCount, int maxCount, int minStack = 1, int maxStack = 5) // 수정필요
    {
        List<Item> result = new List<Item>();
        if (allItems.Count == 0) return result;

        int count = Random.Range(minCount, maxCount + 1); // 보상 개수
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, allItems.Count);

            // 아이템 복사해서 새 객체 생성
            Item item = new Item(allItems[index]);
            int stack = Random.Range(minStack, maxStack + 1); // 랜덤 수량 
            if (stack > item.template.maxStack)
            {
                item.currentCount = maxStack;
            }
            else
            {
                item.currentCount = stack;
            }


            result.Add(item);
        }

        return result;
    }
}
