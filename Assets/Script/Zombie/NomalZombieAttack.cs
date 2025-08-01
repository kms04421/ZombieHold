using UnityEngine;
using YourGame.AI;

public class NomalZombieAttack : IZombieState
{
    public void Enter(Zombie z)
    {
        z.Animator.SetBool("Attack", true);
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
