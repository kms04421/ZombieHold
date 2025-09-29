using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    public Item test;

    [Header("아이템 스크리터블 오브젝트")]
    public Item item;

    [Header("이미지 연결")]
    public Image image;

    [Header("아이템 쿨타임")]
    public Image coolTime;

    [Header("현재 개수")]
    public TextMeshProUGUI currnetCountText;

    private Transform orgTransform;
    private void Start()
    {
        if (test != null)
        {
            SetSlot(test);

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
        image.sprite = item.icon;
       
        AddSlot(_item.currentCount);
        
        //test
        currnetCountText.text = item.currentCount.ToString();
    }
    /// <summary>
    /// 아이템 수량추가
    /// </summary>
    /// <param name="i"></param>
    public void AddSlot(int i)
    {
        int total = i + item.currentCount;
        if(total > item.maxStack)
        {
            item.currentCount = item.maxStack;
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
        if(item != null)
        {
   
            currnetCountText.text = item.currentCount.ToString();

        }
    }
    /// <summary>
    /// 슬롯 사용
    /// </summary>
    public void Use()
    {
        if (item == null || item.currentCount <= 0) return;

        PlacementManager.Instance.StartPlacement(item.prefab, OnPlacementResult);
    }

    // 설치 결과 처리
    private void OnPlacementResult(bool success)
    {
        Debug.Log("OnPlacementResult");
        if (!success) return; // 설치 취소하면 아무 것도 안 함
        RemoveSlot(); // 슬롯 갯수차감

    }
    /// <summary>
    /// 슬롯 카운터 감소, 제거
    /// </summary>
    public void RemoveSlot(int count =1)
    {
        item.currentCount -= count;
        if (item.currentCount > 0)
        {       
            currnetCountText.text = item.currentCount.ToString();
        }
        else
        {
            InitSlot();
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
            if(fromSlot.item != null && item != null  && fromSlot.item.id == item.id)
            {
                int total = fromSlot.item.currentCount + item.currentCount;
                if(total > fromSlot.item.maxStack)
                {
                    fromSlot.item.currentCount = fromSlot.item.maxStack;
                    item.currentCount = total  - item.maxStack;
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
        }
       
    }
}
