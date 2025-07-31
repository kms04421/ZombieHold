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
        public IZombieState ChaseState { get; private set; }
        public IZombieState DeadState { get; private set; }

        public IZombieState AttackState { get; private set; }

        private IZombieState currentState;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Hp = maxHp;

            ChaseState = new ZombieChaseState(baseSpeed);
            DeadState = new ZombieDeadState();
            AttackState = new NomalZombieAttack();
        }
        private void OnEnable()
        {
            chaseTarget = GameManger.Instance.GetPlayer();
            Debug.Log(Hp);
        }
        private void Start()
        {
            ChangeState(ChaseState);
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
            Animator.SetTrigger("Die");
            Invoke("ReturnZombie", 2);
            // 사망 애니메이션, 콜라이더 비활성화 등
        }
        public void ReturnZombie()
        {
            Hp = maxHp;
            ChangeState(ChaseState);
            // 풀로 반환
            ZombiePoolManager.Instance.ReturnZombie(gameObject);
        }
    }
}