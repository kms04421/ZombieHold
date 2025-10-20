using System.Collections.Generic;

[System.Serializable]
public class ItemDTO
{
    public int id;
    public string itemName;
    public int stackable;
    public int maxStack;
    public int currentCount;
}

[System.Serializable]
public class ItemListWrapper
{
    public List<ItemDTO> items;
}