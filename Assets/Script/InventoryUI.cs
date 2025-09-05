using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    public GameObject inventory;

    public void Show()
    {
        bool isActive = !inventory.activeSelf;
        inventory.SetActive(isActive);
    }
}
