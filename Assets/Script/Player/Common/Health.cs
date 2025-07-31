using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public delegate void OnPlayerDeath();
    public event OnPlayerDeath onDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");
        onDeath?.Invoke(); // 죽음 알림
        // 애니메이션이나 GameOver 처리도 여기서 가능
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }
}
