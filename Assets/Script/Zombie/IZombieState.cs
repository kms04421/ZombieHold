using UnityEngine;

namespace YourGame.AI
{
    public interface IZombieState
    {
        void Enter(Zombie context);
        void Execute(Zombie context);
        void Exit(Zombie context);
  
    }
}
