using UnityEngine;

namespace YourGame.AI
{
    public class ZombieDeadState : IZombieState
    {
        public void Enter(Zombie z)
        {
            Debug.Log("¡¯¿‘");
            z.Animator.SetTrigger("Die");
            z.Die();        
        }
        public void Execute(Zombie z) { }
        public void Exit(Zombie z) { }
      
    }
}