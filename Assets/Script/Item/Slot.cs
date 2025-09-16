using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
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


    private void Start()
    {
        if (test != null)
        {
            SetSlot(test, 2);

        }
    }

    /// <summary>
    /// 아이템 정보 슬롯에 Setting
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(Item _item, int testCount = 0)
    {
        item = _item;
        image.gameObject.SetActive(true);
        image.sprite = item.icon;
        //tset
        if (testCount > 0)
        {
            item.currentCount = 2;
        
        }
        else
        {
            item.currentCount = _item.currentCount;
        }
        //test
        currnetCountText.text = item.currentCount.ToString();
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
    /// 슬롯 사용
    /// </summary>
    public void Use()
    {
        Debug.Log(item);
        Debug.Log(item.currentCount);
        if (item == null || item.currentCount <= 0) return;

        PlacementManager.Instance.StartPlacement(item.prefab, OnPlacementResult);
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
    public void RemoveSlot(int count =1)
    {
        Debug.Log(item.currentCount);
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
}
