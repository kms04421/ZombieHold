[System.Serializable]
public class ZombieData
{
    public float maxHp;
    public float hpMultiplier;
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float attackDamage;
    public string atkType;
    public string name;

    public ZombieData()
    {
        maxHp = 100f;
        hpMultiplier = 20f;
        speed = 2f;
        minSpeed = 1.5f;
        maxSpeed = 4f;
        attackDamage = 10f;
    }
    public ZombieData(ZombieData _zombie)
    {
        maxHp = _zombie.maxHp;
        hpMultiplier = _zombie.hpMultiplier;
        speed = _zombie.speed;
        minSpeed = _zombie.minSpeed;
        maxSpeed = _zombie.maxSpeed;
        attackDamage = _zombie.attackDamage;
        atkType = _zombie.atkType;
        name = _zombie.name;
    }
}