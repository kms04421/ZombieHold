using UnityEngine;

public class TabBarController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;
    private CraftingRecipe craftingRecipe;
    // 슬롯 클릭 시 호출
    public void ShowTabBar(RectTransform slotTransform, CraftingRecipe _craftingRecipe, float yOffset = -50f)
    {
        tabBar.SetActive(true);

        craftingRecipe = _craftingRecipe;

        // TabBar 위치를 슬롯 위치로 이동
        RectTransform tabRect = gameObject.GetComponent<RectTransform>();
        // X축은 오른쪽, Y축은 살짝 아래
        float offsetX = (slotTransform.rect.width / 2f) + (tabRect.rect.width / 2f);

        Vector3 newPos = slotTransform.position + new Vector3(offsetX, yOffset, 0);

        tabRect.position = newPos;
    }

    public void HideTabBar()
    {
        tabBar.SetActive(false);
        craftingRecipe = null;
    }

    public void CraftingOnClick()
    {
        if (craftingRecipe != null) 
        CraftingManager.Instance.Craft(craftingRecipe);
    }
}
