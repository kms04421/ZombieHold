using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private int skillPoint = 0;

    [SerializeField] private PlayerController playerController;

    private PlayerData playerData;
    private AbilitySlot slot;

    private void Start()
    {
        skillPoint = 1; //테스트용
        playerData = new PlayerData();
        playerData.ResetPlayData();
        PlayerUI.Instance.SetAbilityPoint(skillPoint);
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