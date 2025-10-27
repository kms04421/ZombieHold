using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private int skillPoint = 0;
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerController playerController; 
    
    [Header("DB메니져")]
    [SerializeField] private DBManager dbManager; // db메니져

    private Dictionary<string, AbilityDTO> abilitys; // 어빌리티 정보 저장용
    private PlayerData playerData; //적용할 능력치 저장용
    private AbilitySlot slot; // 현재 클릭한 슬롯정보 저장용

    public event Action OnDBLoaded; //DB 데이터가 성공적으로 로드된 시점에 데이터 세팅

    private void Awake()
    {
        abilitys = new Dictionary<string, AbilityDTO>();
        dbManager.DBAbilityRequest((server) =>
        {
            if(server == null || server.Count == 0)
            {
                return;
            }

            for(int i =0; i < server.Count; i++)
            {
                AbilityDTO abilityDTO = server[i];
                abilitys.Add(abilityDTO.Name, abilityDTO);
            }
            OnDBLoaded?.Invoke();//슬롯에 데이터 세팅
        });
    }
    private void Start()
    {
        skillPoint = 10; //테스트용
        playerData = new PlayerData();
        playerData.ResetPlayData();
        PlayerUI.Instance.SetAbilityPoint(skillPoint);
    }

    public AbilityDTO GetAbilitys(string name)
    {
        return abilitys[name];
    }

    /// <summary>
    /// Player에 능력치 적용
    /// </summary>
    public void PlayerStatsUp()
    {
        if (skillPoint > 0)
        {
            skillPoint--;
            playerController.SetStats(playerData);
            playerData.ResetPlayData();
            slot.DisableAbility();
            PlayerUI.Instance.SetAbilityBtnActive(false);
            PlayerUI.Instance.SetAbilityPoint(skillPoint);
        }
        else
        {
            Debug.Log("스킬포인트가 없습니다");
        }
    }
    /// <summary>
    /// 어블리티 능력치 AbilityController에 있는 playerData에 적용
    /// </summary>
    /// <param name="playerStats">적용할 스텟</param>
    /// <param name="amount">적용할수치</param>
    public void SetPlayerData(PlayerStats playerStats, float amount)
    {
        switch (playerStats)
        {
            case PlayerStats.MaxHp:
                playerData.SetMaxHp(amount);
                break;
            case PlayerStats.AttackPower:
                playerData.SetAttackPower(amount);
                break;
            case PlayerStats.Defense:
                playerData.SetDefense(amount);
                break;


        }
    }
    public void SetAbilitySlot(AbilitySlot _slot)
    {
        slot = _slot;
    }
    /// <summary>
    /// 추가할 어빌리티 초기화
    /// </summary>
    public void Init()
    {
        playerData.ResetPlayData();
    }
}
public enum PlayerStats
{
    PlayerName,
    MaxHp,
    Level,
    AttackPower,
    Defense
}