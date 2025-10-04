using UnityEngine;

public class CraftingManager : Singleton<CraftingManager>
{
  /// <summary>
  /// 아이템 제작 함수
  /// </summary>
  /// <param name="recipe"></param>
    public void Craft(CraftingRecipe recipe)
    {
        bool isCraftable = true;
        // 재료 체크
        foreach (var ingredients in recipe.ingredients)
        {
            if (!SlotManager.Instance.inventory.HasItem(ingredients.item.id, ingredients.count)) // 필요한 수량은 item에 정의
            {
                isCraftable = false;
                Debug.Log("재료 부족!");
            
            }
        }

        // 재료 차감
        foreach (var ingredients in recipe.ingredients)
        {
            SlotManager.Instance.inventory.RemoveItem(ingredients.item.id, ingredients.count);
        }

        if (isCraftable)
        {
            // 결과물 지급
            SlotManager.Instance.inventory.AddItem(new Item(recipe.resultItem), recipe.resultCount);
        }
   
    }
}