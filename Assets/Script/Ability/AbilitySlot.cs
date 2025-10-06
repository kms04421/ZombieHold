using System.Collections.Generic;
using UnityEngine;

public class AbilitySlot : MonoBehaviour
{
    [HideInInspector] public string Name { get; private set; }
    [HideInInspector] public string Description { get; private set; }

    private bool canActivateAbility = true;

    [Header("부여할 어빌리티 스텟")]
    [SerializeField] private List<AbilityData> abilityDatas;

    [SerializeField] private AbilityController controller;

    private PlayerUI playerUI;
    private void Start()
    {
        Name = "text";
        Description = "dasdasd";
    }
    #region 데이터 베이스 용
    public void SetName(string str)
    {
        Name = str;
    }

    public void SetDescription(string str)
    {
        Description = str;
    }
    public void DisableAbility()
    {
        canActivateAbility = false;
    }
    #endregion
    public void OnClick()
    {
        if (playerUI == null)
        {
            playerUI = PlayerUI.Instance;
        }

        playerUI.SetAbilityNameText(Name);
        playerUI.SetAbilityDescriptionText(Description);

        controller.Init();//초기화

        if (!canActivateAbility)
        {
            playerUI.SetAbilityBtnActive(false);
            return;
        }
        else
        {
            playerUI.SetAbilityBtnActive(true);
        }

        for (int i = 0; i < abilityDatas.Count; i++)
        {
            AbilityData abilityData = abilityDatas[i];
            controller.SetPlayerData(abilityData.stats, abilityData.value);
        }
        controller.SetAbilitySlot(this);
    }
}
