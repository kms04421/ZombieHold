using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    [Header("slot 프리펩&위치")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private PlayerController playerController;

    //인벤토리
    [HideInInspector] public Inventory inventory;

    [Header("슬롯 정보 설정")]
    private int slotCount = 39;

    [SerializeField] private Slot[] uiSlots;


    protected override void Awake()
    {
        base.Awake();
        inventory = new Inventory();
    }
    private void Start()
    {
        GameManager.Instance.OnPlayerIDAssigned += SetPlayerController;

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
    private void SetPlayerController()
    {
        playerController = GameManager.Instance.GetPlayerID;
        SetPlayerUI();
    }
    public void UseUiSlot(int index)
    {
        if (uiSlots[index] == null) return;
        uiSlots[index].Use(playerController);
    }
    /// <summary>
    /// 무기중찾기
    /// </summary>
    /// <returns></returns>
    public GunBase GetWeaponTrans(string name)
    {
        Transform foundChild = playerController.weaponTransform.Find(name);

        if (foundChild != null)
        {
            Debug.Log("자식 찾음: " + foundChild.name);
            GunBase gunBase = foundChild.GetComponent<GunBase>();
            if (gunBase != null)
            {
                foundChild.gameObject.SetActive(true);
                return gunBase;
            }
            else
            {
                return null;
            }

        }
        else
        {
            Debug.Log("자식 없음");
            return null;
        }


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
        if (playerController.currentGun != null)
        {
            //총알 관련 ui
            int total = inventory.HasItemCount(playerController.currentGun.GetAmmoId()); // 총알이 충분한지 검사
            PlayerUI.Instance.SetCurrentAmmo(playerController.currentGun.CurrentAmmo); // 현재 총알 ui 적용
            PlayerUI.Instance.SetAllAmmo(total); // 남은 전체총알 ui 출력
                                                 //총알 관련 ui
        }
        else
        {
            PlayerUI.Instance.SetCurrentAmmo(0); // 현재 총알 ui 적용
            PlayerUI.Instance.SetAllAmmo(0); // 남은 전체총알 ui 출력
        }

    }
}
