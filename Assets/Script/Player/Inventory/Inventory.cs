using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField] public Dictionary<string, Slot> slots = new Dictionary<string, Slot>();

    /// <summary>
    /// 아이템이 해당 갯수만큼 있는지 확인 
    /// </summary>
    /// <param name="itemId">아이템 id</param>
    /// <param name="count">확인할 갯수</param>
    /// <returns> </returns>
    public bool HasItem(string itemId, int count)
    {
        int totalCount = 0;

        foreach (var kvp in slots)
        {
            Slot slot = kvp.Value;
            if (slot != null && slot.item != null && slot.item.template.id == itemId)
            {
                totalCount += slot.item.currentCount;
            }
        }

        return totalCount >= count;

    }
    /// <summary>
    /// 아이템이 해당 갯수확인 
    /// </summary>
    /// <param name="itemId">아이템 id</param>
    /// <param name="count">확인할 갯수</param>
    /// <returns> </returns>
    public int HasItemCount(string itemId)
    {
        int totalCount = 0;

        foreach (var kvp in slots)
        {
            Slot slot = kvp.Value;
            if (slot.item != null)
            {
               /// Debug.Log(slot.item.template.id);
            }    
            if (slot != null && slot.item != null && slot.item.template.id == itemId)
            {
                totalCount += slot.item.currentCount;
            }
        }

        return totalCount;

    }
    /// <summary>
    /// 인벤토리에 아이템 추가 
    /// </summary>
    /// <param name="itemId">아이템 id</param>
    /// <param name="count">추가할 갯수</param>
    public void AddItem(Item item, int count = 1)
    {
        if (item == null) return;

        // 1. 스택 가능한 경우 기존 슬롯에 합치기
        foreach (var kvp in slots)
        {
            Slot slot = kvp.Value;
            if (slot != null && slot.item != null && slot.item.template.id == item.template.id && item.template.stackable)
            {
                int availableSpace = slot.item.template.maxStack - slot.item.currentCount;
                int toAdd = Mathf.Min(availableSpace, count);
                count -= toAdd;

                if(toAdd > 0)
                {
                    Debug.Log("AddSlot" + toAdd);
                    slot.AddSlot(toAdd);
                }
            }
        }
        if (count <= 0) return;
        Debug.Log("?");
        // 2. 빈 슬롯 찾기
        foreach (var kvp in slots)
        {
            Slot slot = kvp.Value;
            if (slot != null && slot.item == null) // 빈 슬롯
            {
                slot.SetSlot(item);                     // 슬롯에 아이템 설정
                slot.item.currentCount = Mathf.Min(count, item.template.maxStack);
                count -= slot.item.currentCount;

                if (count <= 0)
                {
                    slot.SetSlot(item);
                    return; // 모두 추가 완료
                }
            }
        }


    }
    /// <summary>
    /// 아이템 차감 혹은 삭제 
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="count"></param>
    public void RemoveItem(string itemId, int count = 1)
    {
        foreach (var kvp in slots)
        {
            Slot slot = kvp.Value;
            if (slot != null && slot.item != null && slot.item.template.id == itemId)
            {
                if(slot.item.currentCount >= count)
                {
                    slot.RemoveSlot(count);
                    count = 0;
                }
                else
                {
                    count = count - slot.item.currentCount;  
                    slot.RemoveSlot(slot.item.currentCount);
                }
                if(count == 0)
                {
                    break;
                }
            }
        }
    }
}
