using UnityEngine;

public class Item 
{
    public ItemSO template;   // 원본 ScriptableObject 참조
    public int currentCount; // 개별 아이템의 상태 (개수, 내구도 등)

    /// <summary>
    /// ItemSO정보 복제 해성 생성
    /// </summary>
    /// <param name="template">ItemSO 정보</param>
    /// <param name="count"></param>
    public Item(ItemSO template, int count = 1)
    {
        this.template = template;
        this.currentCount = count;
    }
}
