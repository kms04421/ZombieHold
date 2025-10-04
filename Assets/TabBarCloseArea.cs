using UnityEngine;
using UnityEngine.EventSystems;

public class TabBarCloseArea : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TabBarController tabBarController;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabBarController.HideTabBar(); // 탭바 닫기
        gameObject.SetActive(false);  // 배경도 비활성화
    }
}