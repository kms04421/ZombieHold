using UnityEngine;
using System.Collections.Generic;
public class CompensationInventory : MonoBehaviour
{
    [SerializeField] private List<Slot> slot;

    /// <summary>
    /// Compensation 슬롯초기화
    /// </summary>
    public void Init()
    {
        for (int i = 0; i < slot.Count; i++)
        {
            slot[i].InitSlot();
        }
    }

    /// <summary>
    /// 아이템을 리스트로 받아서 추가
    /// </summary>
    /// <param name="newItems"></param>
    /// <returns></returns>
    public bool AddItems(List<Item> newItems)
    {
        bool allAdded = true;

        foreach (var item in newItems)
        {
            bool added = AddItem(item); // 기존 AddItem(Item) 재사용
            if (!added)
            {
                allAdded = false; // 일부 아이템 추가 실패 시 false
            }
        }

        return allAdded;
    }

    /// <summary>
    /// Compensation 아이템 추가
    /// </summary>
    /// <param name="_item"></param>
    /// <returns></returns>
    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < slot.Count; i++)
        {
            if (slot[i].item == null)
            {
                slot[i].SetSlot(newItem);
                return true;
            }
            else if (slot[i].item.template.id == newItem.template.id)
            {
                slot[i].AddSlot(newItem.currentCount);
            }
        }
        return false;
    }

}