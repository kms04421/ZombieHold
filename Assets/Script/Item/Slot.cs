using Unity.VisualScripting;
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

    private void Start()
    {
        item = test;
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
    }

    /// <summary>
    /// 슬롯정보 초기화
    /// </summary>
    public void InitSlot()
    {
        item = null;
        image.sprite =null;
        image.gameObject.SetActive(false);
    }
}
