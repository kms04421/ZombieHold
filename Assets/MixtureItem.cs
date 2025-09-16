using UnityEngine;
using UnityEngine.EventSystems;

public class MixtureItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject Deactivate;
    [SerializeField] private TabBarController tabBarController;
    [SerializeField] private CraftingRecipe craftingRecipe;
    public bool Activate = false;
    private RectTransform RectTransform;
    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }
    public void ActivateMixtureItem()
    {
        if (Deactivate != null)
            Deactivate.SetActive(false);

        Activate = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        tabBarController.ShowTabBar(RectTransform , craftingRecipe);
    }
}
