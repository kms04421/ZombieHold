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
    
    
}
