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
            SetSlot(test);

        }
    }
    /// <summary>
    /// 아이템 정보 슬롯에 Setting
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(Item _item)
    {
        item = _item;
        image.gameObject.SetActive(true);
        image.sprite = item.icon;
        //tset
        item.currentCount = 2;
        //test
        currnetCountText.text = item.currentCount.ToString();
    }

    /// <summary>
    /// 슬롯정보 초기화
    /// </summary>
    private void InitSlot()
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
        if (item == null || item.currentCount <= 0) return;
        PlacementManager.Instance.StartPlacement(item.prefab, OnPlacementResult);
    }

    // 설치 결과 처리
    private void OnPlacementResult(bool success)
    {
        if (!success) return; // 설치 취소하면 아무 것도 안 함
        RemoveSlot(); // 슬롯 초기화

    }
    /// <summary>
    /// 슬롯 카운터 제거
    /// </summary>
    private void RemoveSlot()
    {
        if (item.currentCount > 1)
        {
            item.currentCount--;
            currnetCountText.text = item.currentCount.ToString();
        }
        else
        {
            InitSlot();
        }
    }
}
