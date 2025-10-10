using System.Collections;
using UnityEngine;
namespace YourGame.AI
{
    public class ZombieChaseState : IZombieState
    {
        public void Enter(Zombie z)
        {
            z.Agent.isStopped = false;    
        }

        public void Execute(Zombie z)
        {
           if(!z.ChaseTarget.isGrounded)
            {
                z.Agent.SetDestination(z.ChaseTarget.jumpPos);
                Debug.Log(z.ChaseTarget.isGrounded);
            }
            else
            {
                z.Agent.SetDestination(z.ChaseTarget.transform.localPosition);
            }             
            z.Animator.SetFloat("speed", z.data.speed);
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