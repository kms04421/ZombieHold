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

    
    
    }
}