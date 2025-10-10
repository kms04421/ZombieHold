[System.Serializable]
public class ZombieData
{
    public float maxHp;
    public float hpMultiplier;
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float attackDamage;

    public ZombieData()
    {
        maxHp = 100f;
        hpMultiplier = 20f;
        speed = 2f;
        minSpeed = 1.5f;
        maxSpeed = 4f;
        attackDamage = 10f;
    }
}