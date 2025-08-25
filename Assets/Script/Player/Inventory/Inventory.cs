using System.Collections.Generic;

public class Inventory
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    public bool HasItem(string itemId, int count)
    {
        return items.ContainsKey(itemId) && items[itemId] >= count;
    }

    public void AddItem(string itemId, int count = 1)
    {
        if (!items.ContainsKey(itemId)) items[itemId] = 0;
        items[itemId] += count;
    }

    public void RemoveItem(string itemId, int count = 1)
    {
        if (items.ContainsKey(itemId))
        {
            items[itemId] -= count;
            if (items[itemId] <= 0) items.Remove(itemId);
        }
    }
}
