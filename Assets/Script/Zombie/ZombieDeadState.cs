using UnityEngine;

namespace YourGame.AI
{
    public class ZombieDeadState : IZombieState
    {
        public void Enter(Zombie z)
        {
            z.Animator.SetTrigger("Die");
            z.Die();        
        }
        public void Execute(Zombie z) { }
        public void Exit(Zombie z) { }
        public void OnHit(Zombie z, float damage, Zombie.HitType hitType) { }
    }
}