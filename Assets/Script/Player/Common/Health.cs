using UnityEngine;

public class Health : MonoBehaviour
{
    private float maxHp = 0;
    private float currentHealth;
    private PlayerController playerController;
    public delegate void OnPlayerDeath();
    public event OnPlayerDeath onDeath;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            maxHp = playerController.playerData.MaxHp;
            currentHealth = maxHp;
        }
        else
        {
            SetHP(100); // 임시
        }
       
    }

    public void IncreaseMaxHp(float amount)
    {
        maxHp += amount;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        playerUI.Instance.UpdateHealthUI(currentHealth/maxHp);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (playerController.ChkState(playerController.DeadState)) return;
        Debug.Log("플레이어 사망");
        onDeath?.Invoke(); // 죽음 알림
        playerController.ChangeState(playerController.DeadState);
        // 애니메이션이나 GameOver 처리도 여기서 가능
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHp);
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHp;
    }

    #region 구조물 
    public void SetHP(float value)
    {
        maxHp = value;
        currentHealth = value;
    }

    #endregion
}
