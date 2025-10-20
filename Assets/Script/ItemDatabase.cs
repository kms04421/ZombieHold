using System.Collections.Generic;
using UnityEngine;
public class ItemDatabase : Singleton<ItemDatabase>
{
    public List<Item> allItems;
    public DBManager dBManager;
    public void LoadItemsFromDB()
    {
        dBManager.DBItemsRequest((serverItems) =>
        {
            if(serverItems == null || serverItems.Count == 0)
            {
                Debug.LogError("아이템 정보를 가지고 오지 못했습니다");
               return;
            }

            allItems = serverItems;
        });
       
    }

    public List<Item> GetRandomItems() // 수정필요
    {
        List<Item> result = new List<Item>();
        if (allItems.Count == 0) return result;

        int count = Random.Range(0, 5); // 보상 개수
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, allItems.Count);

            // 아이템 복사해서 새 객체 생성
            Item item = allItems[index];
            int stack = Random.Range(item.template.minRandomItemCount, item.template.maxRandomItemCount); // 랜덤 수량 
            if (stack > item.template.maxStack)
            {
                item.currentCount = item.template.maxStack;
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
