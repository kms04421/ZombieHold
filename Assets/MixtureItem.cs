using UnityEngine;
using UnityEngine.EventSystems;

public class MixtureItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject Deactivate;
    [SerializeField] private TabBarController tabBarController;
    public bool Activate = false;
    public void ActivateMixtureItem()
    {
        if (Deactivate != null)
            Deactivate.SetActive(false);

        Activate = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        tabBarController.ShowTabBar(GetComponent<RectTransform>());
    }
}
