using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public Ingredient[] ingredients;   // 필요한 재료
    public Item resultItem;      // 결과물
    public int resultCount = 1;  // 결과 개수
}
