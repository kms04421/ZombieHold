using UnityEngine;

namespace YourGame.AI
{
    public class ZombieNormalState : IZombieState
    {
        private readonly float speed;

        public ZombieNormalState(float speed) => this.speed = speed;

        public void Enter(Zombie z)
        {
            z.Agent.speed = speed;
            // z.Animator.Play("Walk");
        }

        public void Execute(Zombie z)
        {
            // 추적/순찰 로직
        }

        public void Exit(Zombie z)
        {
            // 상태 종료 시 필요한 정리 작업
        }

        public void OnHit(Zombie z, float damage, Zombie.HitType hitType)
        {
            z.ApplyDamage(damage);
            if (z.Hp <= 0f)
            {
                z.ChangeState(z.DeadState);
                return;
            }
        }
    }
}