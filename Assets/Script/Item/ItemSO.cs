using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Itme/Item")]
public class ItemSO : ScriptableObject
{
    [Header("기본 정보")]
    public string id;          // 아이템 고유 ID 
    public string itemName;    // 아이템 이름 
    public Sprite icon;        // UI 아이콘
    public GameObject prefab;  // 월드에 떨어질 때 프리팹

    [Header("속성")]
    public bool stackable = true; // 겹쳐쓸 수 있는지 여부
    public int maxStack = 99;     // 최대 개수
    public int testCount = 0;  // 테스트용 개수

    [Header("드랍관련")]
    [Range(0,1)]
    public float dropChance = 0f;
    [Min(1)]
    public int minDropCount = 1;   // 최소 드랍 수량
    [Min(1)]
    public int maxDropCount = 1;   // 최대 드랍 수량

    [Header("랜덤 획득 아이템 수량")]
    [Range(0, 100)]
    public int minRandomItemCount = 0;

    [Range(0, 100)]
    public int maxRandomItemCount = 1;
}