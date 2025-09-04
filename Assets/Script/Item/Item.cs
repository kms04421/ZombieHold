using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("기본 정보")]
    public string id;          // 아이템 고유 ID 
    public string itemName;    // 아이템 이름 
    public Sprite icon;        // UI 아이콘
    public GameObject prefab;  // 월드에 떨어질 때 프리팹

    [Header("속성")]
    public bool stackable = true; // 겹쳐쓸 수 있는지 여부
    public int maxStack = 99;     // 최대 개수
    public int currentCount = 0;  // 현재 개수
}