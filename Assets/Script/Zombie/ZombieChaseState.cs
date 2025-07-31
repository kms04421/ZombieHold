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
            if (z.ChaseTarget != null)
            {
                z.Agent.SetDestination(z.ChaseTarget.position);
            }
               
            z.Animator.SetFloat("speed", speed);
        }

        public void Exit(Zombie z)
        {
            // 상태 종료 시 필요한 정리 작업
        }

        public void OnHit(Zombie z, float damage, Zombie.HitType hitType)
        {
            Debug.Log("hit");
            z.ApplyDamage(damage);
            if (z.Hp <= 0f)
            {
                z.ChangeState(z.DeadState);
                return;
            }
        }
    }
}