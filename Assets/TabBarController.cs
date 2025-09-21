using UnityEngine;

public class TabBarController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;
    private CraftingRecipe craftingRecipe;

    /// <summary>
    /// 텝바 활성화
    /// </summary>
    /// <param name="slotTransform"> 클릭한 슬롯의 RectTransform정보</param>
    /// <param name="_craftingRecipe">제작 스크립터블 오브젝트 정보</param>
    /// <param name="yOffset">텝바 호출시 y좌표 위치 </param>
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

    /// <summary>
    /// 텝바 비활성화
    /// </summary>
    public void HideTabBar()
    {
        tabBar.SetActive(false);
        craftingRecipe = null;
    }

    /// <summary>
    /// 제작 템바 버튼 클릭시
    /// </summary>
    public void CraftingOnClick()
    {
        if (craftingRecipe != null) 
        CraftingManager.Instance.Craft(craftingRecipe);
    }
}
