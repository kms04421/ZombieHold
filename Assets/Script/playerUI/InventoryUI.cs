using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    // 인벤토리 오브젝트
    public GameObject inventory;

    /// <summary>
    /// 인벤토리 활성화,비활성화
    /// </summary>
    public void Show()
    {
        bool isActive = !inventory.activeSelf;
        inventory.SetActive(isActive);
    }
}
