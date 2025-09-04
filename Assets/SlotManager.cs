using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    [SerializeField] private int slotCount = 4;
    private Slot[] slots;
    [SerializeField] private Slot[] uiSlots;
    private int currentIndex = 0;

    protected override void Awake()
    {      
        slots = new Slot[slotCount];
    }

    public void AssignItem(int index, Slot item)
    {
        if (index < 0 || index >= slots.Length) return;
        slots[index] = item;
        //InventoryUI.Instance.Refresh(slots);
    }

    public void UseSlot(int index)
    {
        if (slots[index] != null) return;
            slots[index].Use();
    }
    public void UseUiSlot(int index)
    {
        Debug.Log("UseUiSlot"+index);
        if (uiSlots[index] == null) return;
        uiSlots[index].Use();
    }

    public void SwitchSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        currentIndex = index;
    //    QuickSlotUI.Instance.Highlight(index);
    }
}
