using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler,IEventSystemHandler, IEndDragHandler
{
    [Header("아이템 스크리터블 오브젝트")]
    public ItemSO test;

    [Header("슬롯의 타입")]
    public ItemType slotType;

    [Header("아이템")]
    public Item item;

    [Header("이미지 연결")]
    public Image image;

    [Header("아이템 쿨타임")]
    public Image coolTime;

    [Header("현재 개수")]
    public TextMeshProUGUI currnetCountText;

    private Transform orgTransform;

    private void Awake()
    {
        if (test != null)
        {
            SetSlot(new Item(test, 20));

        }
    }
    /// <summary>
    /// 아이템 정보 슬롯에 Setting
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(Item _item)
    {
        if (_item == null) return;
        item = _item;
        image.gameObject.SetActive(true);
        image.sprite = item.template.icon;

        item.currentCount = _item.currentCount;

        UpdateSlot();
    }
    /// <summary>
    /// 아이템 수량추가
    /// </summary>
    /// <param name="i"></param>
    public void AddSlot(int i)
    {
        int total = i + item.currentCount;
        if (total > item.template.maxStack)
        {
            item.currentCount = item.template.maxStack;
        }
        else
        {
            item.currentCount = total;
        }

        UpdateSlot();
    }

    /// <summary>
    /// 슬롯정보 초기화
    /// </summary>
    public void InitSlot()
    {
        item = null;
        image.sprite = null;
        image.gameObject.SetActive(false);
        currnetCountText.text = "";


    }
    /// <summary>
    /// 주어진 item 정보를 기반으로 업데이트
    /// </summary>
    private void UpdateSlot()
    {
        if (item != null)
        {
            currnetCountText.text = item.currentCount.ToString();
            SlotManager.Instance.SetPlayerUI();
        }
    }
    /// <summary>
    /// 슬롯 사용
    /// </summary>
    public void Use(PlayerController player)
    {
        if (item == null || item.currentCount <= 0) return;
        switch(item.template.type)
        {
            case ItemType.Placeable:
                PlacementManager.Instance.StartPlacement(item.template.prefab, OnPlacementResult);
                player.UnequipGun();
                SlotManager.Instance.SetPlayerUI();
                break;
            case ItemType.Weapon:
                GunBase gunBase = SlotManager.Instance.GetWeaponTrans(item.template.name);
                if (gunBase != null)
                {                 
                    player.EquipGun(gunBase);
                    SlotManager.Instance.SetPlayerUI();
                }

                break;
        }
  
     
    }

    // 설치 결과 처리
    private void OnPlacementResult(bool success)
    {
        if (!success) return; // 설치 취소하면 아무 것도 안 함
        RemoveSlot(); // 슬롯 갯수차감

    }
    /// <summary>
    /// 슬롯 카운터 감소, 제거
    /// </summary>
    public void RemoveSlot(int count = 1)
    {

        if (item.currentCount >= count)
        {
            item.currentCount -= count;
            if (item.currentCount == 0)
            {
                InitSlot();
            }
            else
            {
                currnetCountText.text = item.currentCount.ToString();
            }

        }

    }

    // --- 드래그 앤 드롭 ---
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        orgTransform = transform;
        image.transform.SetParent(transform.parent.parent); // Canvas 최상단으로 빼기
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        image.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
  
        Slot fromSlot = eventData.pointerDrag?.GetComponent<Slot>();
        if (fromSlot != null && fromSlot != this)
        {
            if (slotType != fromSlot.item.template.type)
            {
                if(slotType != ItemType.All)
                {
                    return;
                }

            }
            if (fromSlot.item != null && item != null && fromSlot.item.template.id == item.template.id)
            {
                int total = fromSlot.item.currentCount + item.currentCount;
                if (total > fromSlot.item.template.maxStack)
                {
                    fromSlot.item.currentCount = fromSlot.item.template.maxStack;
                    item.currentCount = total - item.template.maxStack;
                    fromSlot.UpdateSlot();
                    UpdateSlot();
                }
                else
                {
                    item.currentCount = total;
                    UpdateSlot();
                    fromSlot.InitSlot();

                }
            }
            else
            {
                // 아이템 스왑
                Item temp = this.item;
                this.SetSlot(fromSlot.item);

                if (temp != null)
                    fromSlot.SetSlot(temp);
                else
                    fromSlot.InitSlot();

            }
            //위치 정보 초기화
            fromSlot.image.transform.SetParent(fromSlot.transform);
            image.transform.SetParent(transform);
            fromSlot.image.transform.localPosition = Vector3.zero;
            image.transform.localPosition = Vector3.zero;
            //위치 정보 초기화
            image.raycastTarget = true;
            SlotManager.Instance.SetPlayerUI();
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            image.transform.SetParent(transform);
            image.transform.localPosition = Vector3.zero;
        }
       
    }
}
