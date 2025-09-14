using UnityEngine;

public class TabBarController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;

    // 슬롯 클릭 시 호출
    public void ShowTabBar(RectTransform slotTransform, float yOffset = -50f)
    {
        tabBar.SetActive(true);

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
    }
}
