using System;
using System.Collections;
using UnityEngine;
using YourGame.AI;
public class ItemDrop : Interactable
{

    public ItemSO test;
    private Item item;
    //아이템 카운터 다운 설정
    const int MaxCount = 60; // 최대 시간
    private int currentCount = 0;//현재 시간
    // WaitForSeconds 캐싱용 1초
    WaitForSeconds seconds = new WaitForSeconds(1);
    private void Start()
    {
        if(test != null)
        {
            item = new Item(test);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(DisableCount());
    }
    public void SetItme(ItemSO _item , int Count)
    {
        if(item == null)
        {
            item = new Item(_item, Count);
        }
        else
        {
            item.template = _item;
            item.currentCount = Count;
        }
    }
    /// <summary>
    /// 초기화
    /// </summary>
    private void InIt()
    {
        item = null;
 
        StopInteract();
        inputHandler.interactable = null;
    }
    /// <summary>
    /// 비활성화시 사용 오브젝트
    /// </summary>
    private void Disable()
    {
        PoolManager.Instance.GetPool<ItemDrop>().ReturnToPool(this);
    }
    /// <summary>
    /// 아이템이 사라지기까지 카운터 다운
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableCount()
    {
        currentCount = 0;
        while (currentCount <= MaxCount)
        {
            currentCount++;
           
            yield return seconds;
        }
        InIt();
        Disable();
    } 
    protected override void OnStartInteract()
    {
        SlotManager.Instance.inventory.AddItem(item, item.currentCount);
        Disable();
    }
    protected override void OnStopInteract()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputHandler.interactable = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputHandler.interactable == this)
            {
                inputHandler.interactable = null;
                StopInteract();
            }
        }
    }
}
