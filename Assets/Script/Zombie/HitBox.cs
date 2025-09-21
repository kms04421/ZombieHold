using UnityEngine;
using UnityEngine.AI;
using YourGame.AI;
using static YourGame.AI.Zombie;
public class HitBox : MonoBehaviour
{
  
    public Zombie.HitType part;

    public Zombie zombie; // 본체 참조
    /// <summary>
    /// 총기 명중 부위에 따라 데미지 적용함수
    /// </summary>
    /// <param name="baseDamage"></param>
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
