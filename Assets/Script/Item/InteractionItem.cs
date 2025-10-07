using UnityEngine;
using System;
using System.Collections;
public class InteractionItem : Interactable
{

    public ItemSO tset;
    private Item item;
    //아이템 카운터 다운 설정
    const int MaxCount = 60; // 최대 시간
    private int currentCount = 0;//현재 시간
    // WaitForSeconds 캐싱용 1초
    WaitForSeconds seconds = new WaitForSeconds(1);
    // 현재오브젝트의 부모오브젝트 저장용
    private Transform parentTransform;

    private void Start()
    {
        item = new Item(tset);
        parentTransform = gameObject.transform.parent;
    }
    private void OnEnable()
    {
        StartCoroutine(DisableCount());
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
        parentTransform.gameObject.SetActive(false);
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
