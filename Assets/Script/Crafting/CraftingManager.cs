using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public Inventory inventory;

    public bool Craft(CraftingRecipe recipe)
    {
        // 재료 체크
        foreach (var item in recipe.ingredients)
        {
            if (!inventory.HasItem(item.id, 1)) // 필요한 수량은 item에 정의
            {
                Debug.Log("재료 부족!");
                return false;
            }
        }

        // 재료 차감
        foreach (var item in recipe.ingredients)
        {
            inventory.RemoveItem(item.id, 1);
        }

        // 결과물 지급
        inventory.AddItem(recipe.resultItem.id, recipe.resultCount);
        Debug.Log(recipe.resultItem.id + " 제작 완료!");

        return true;
    }
}