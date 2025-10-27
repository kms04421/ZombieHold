using System.Collections.Generic;
using UnityEngine;

public class AbilitySlot : MonoBehaviour
{
    public string Name;
    [HideInInspector] public string Description { get; private set; }

    private bool canActivateAbility = true;

    [Header("부여할 어빌리티 스텟")]
    [SerializeField] private List<AbilityData> abilityDatas;
    [Header("선행 필요 어빌리티 리스트")]
    [SerializeField] private List<AbilitySlot> CheckAbility;
    [Header("어빌리티 컨트롤러")]
    [SerializeField] private AbilityController controller;

    private PlayerUI playerUI;

    #region 데이터 베이스 용

    private void Start()
    {
        controller.OnDBLoaded += GetAbilityDB; 
    }
    /// <summary>
    /// 어빌리티 설명 세팅
    /// </summary>
    /// <param name="str"></param>
    private void SetDescription(string str)
    {
        Description = str;
    }
    /// <summary>
    /// DB에서 받아온 정보 어빌리테에 세팅
    /// </summary>
    private void GetAbilityDB()
    {
        AbilityDTO ability = controller.GetAbilitys(Name);
        SetDescription(ability.Description);
    }

    public void DisableAbility()
    {
        canActivateAbility = false;
    }
    #endregion
    /// <summary>
    /// 클릭시 어빌리티 정보 Ui에 세팅
    /// </summary>
    public void OnClick()
    {
        if (playerUI == null)
        {
            playerUI = PlayerUI.Instance;
        }

        playerUI.SetAbilityNameText(Name);
        playerUI.SetAbilityDescriptionText(Description);

        controller.Init();//초기화

        if (!canActivateAbility || !HasPrerequisite())
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

    private bool HasPrerequisite()
    {
        for (int i = 0; i < CheckAbility.Count; i++)
        {
            if (CheckAbility[i].canActivateAbility)
            {
                return false;
            }
        }
        return true;
    }
}
