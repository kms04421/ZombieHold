using UnityEngine;
using YourGame.AI;
using static YourGame.AI.Zombie;
public class HitBox : MonoBehaviour
{
  
    public Zombie.HitType part;

    public Zombie zombie; // 본체 참조

    public void OnHit(float baseDamage)
    {
        float finalDamage = baseDamage;

        switch (part)
        {
            case HitType.Head:
                finalDamage *= 2.0f; // 헤드샷 2배
                break;
            case HitType.Leg:
                finalDamage *= 0.5f;
              //  zombie.SlowDown();
                break;
        }

        zombie.TakeDamage(finalDamage, part);
    }
}
