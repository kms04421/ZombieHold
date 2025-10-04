using UnityEngine;

namespace YourGame.AI
{
    public class ZombieChaseState : IZombieState
    {
        private readonly float speed;

        public ZombieChaseState(float speed) => this.speed = speed;

        public void Enter(Zombie z)
        {
            z.Agent.isStopped = false;
            z.Agent.speed = speed;
        
        }

        public void Execute(Zombie z)
        {
           if(!z.ChaseTarget.isGrounded)
            {
                z.Agent.SetDestination(z.ChaseTarget.jumpPos);
            }
            else
            {
                z.Agent.SetDestination(z.ChaseTarget.transform.position);
            }             
            z.Animator.SetFloat("speed", speed);
        }

        public void Exit(Zombie z)
        {
            z.Agent.isStopped = true;
          
        }

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
}