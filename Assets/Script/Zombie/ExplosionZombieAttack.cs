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
    
}
