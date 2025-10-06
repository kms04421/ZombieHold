using UnityEngine;
using UnityEngine.EventSystems;

public class MixtureItem : MonoBehaviour, IPointerClickHandler
{
    [Header("비활성화 배경")]
    [SerializeField] private GameObject Deactivate;
    [Header("텝바 컨트롤러")]
    [SerializeField] private TabBarController tabBarController;
    [Header("제작 조건메뉴 SO")]
    [SerializeField] private CraftingRecipe craftingRecipe;
    public bool Activate = false;
    private RectTransform RectTransform;
    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }
    /// <summary>
    /// 제작아이템 활성화 함수
    /// </summary>
    public void ActivateMixtureItem()
    {
        if (Deactivate != null)
            Deactivate.SetActive(false);

        Activate = true;
    }
    /// <summary>
    /// 제작 슬롯 클릭시 작동 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        tabBarController.ShowTabBar(RectTransform , craftingRecipe); // 텝바 호풀
    }


}
