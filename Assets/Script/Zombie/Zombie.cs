using UnityEngine;
using UnityEngine.AI;

namespace YourGame.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour
    {
        public enum HitType { Normal, Head, Leg }

        [SerializeField] private float maxHp = 100f;
        [SerializeField] private float baseSpeed = 0.1f; // speed 0 , 1 :걷기, 3 :뛰기
        [SerializeField][Range(0.1f, 1f)] private float slowFactor = 0.5f;

        [Header("Chase")]
        [SerializeField] private Transform chaseTarget; // 임시
        public Transform ChaseTarget => chaseTarget;

        public float Hp { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; } 
        public IZombieState NormalState { get; private set; }
        public IZombieState DeadState { get; private set; }

        private IZombieState currentState;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Hp = maxHp;

            NormalState = new ZombieChaseState(baseSpeed);
            DeadState = new ZombieDeadState();
        }

        private void Start()
        {
            ChangeState(NormalState);
        }

        private void Update()
        {
            currentState.Execute(this);
        }

        public void TakeDamage(float damage, HitType hitType)
        {
            currentState.OnHit(this, damage, hitType);
        }

        public void ChangeState(IZombieState nextState)
        {
            currentState?.Exit(this);
            currentState = nextState;
            currentState.Enter(this);
        }
      
        public void ApplyDamage(float damage)
        {
            Hp -= damage;
        }

        public void Die()
        {
            Debug.Log("Zombie died.");
            Agent.isStopped = true;
            Animator.SetBool("Die", true);
            // 사망 애니메이션, 콜라이더 비활성화 등
        }
    }
}