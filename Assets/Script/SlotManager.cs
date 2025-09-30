using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    [Header("slot 프리펩&위치")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private PlayerController playerController;
    //인벤토리
    [HideInInspector]public Inventory inventory;

    [Header("슬롯 정보 설정")]
    private int slotCount = 36;

    [SerializeField] private Slot[] uiSlots;

    //SwitchSlot용 인덱스 임시
    private int currentIndex = 0;
    
    private void Start()
    {
        inventory = new Inventory();
        int slotid = 0;
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);
            Slot slot = newSlot.GetComponent<Slot>();
            slotid = i;
            // Inventory에 Dictionary로 추가
            inventory.slots.Add(slotid.ToString(), slot);
        }
        for (int i = 0; i < uiSlots.Length; i++)
        {
            slotid = slotid + 1;
            inventory.slots.Add(slotid.ToString(), uiSlots[i]);
        }
    }
    /*    public void AssignItem(int index, Slot item)
        {
            if (index < 0 || index >= slots.Length) return;
            slots[index] = item;
            //InventoryUI.Instance.Refresh(slots);
        }

        public void UseSlot(int index)
        {
            if (slots[index] != null) return;
                slots[index].Use();
        }*/
    public void UseUiSlot(int index)
    {
        Debug.Log(uiSlots[index]);
        if (uiSlots[index] == null) return;
        uiSlots[index].Use();
    }

    /*    public void SwitchSlot(int index)
        {
            if (index < 0 || index >= slots.Length) return;
            currentIndex = index;
        //    QuickSlotUI.Instance.Highlight(index);
        }*/
    /// <summary>
    /// 플레이어 ui업데이트
    /// </summary>
    public void SetPlayerUI()
    {
        playerController.currentGun.SetGun();
    }
}
