using UnityEngine;
using YourGame.AI;

public class ExplosionZombieAttack : IZombieState
{
    public void Enter(Zombie z)
    {
       z.gameObject.SetActive(false);
    }
    public void Execute(Zombie z) { }
    public void Exit(Zombie z) { }
    public void OnHit(Zombie z, float damage, Zombie.HitType hitType)
    {
        z.ApplyDamage(damage);
        if (z.currentHealth <= 0f)
        {
            z.ChangeState(z.DeadState);
            return;
        }
    }
}
