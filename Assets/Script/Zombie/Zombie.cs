using UnityEngine;
using UnityEngine.AI;
using System.Collections;
namespace YourGame.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour
    {
        public enum HitType { Normal, Head, Leg }

        [Header("Chase")]
        [SerializeField] private Transform chaseTarget; // 추적 대상
        public Transform ChaseTarget => chaseTarget;

        [Header("ZombieData")]
        public float currentHealth { get; private set; }
        [SerializeField] private float maxHp = 100f; // 임시
        [SerializeField] private float baseSpeed = 1f; // speed 0 , 1 :걷기, 3 :뛰기
        private float zombieDamege = 10f;

        [Header("Component ")]
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; }
        [SerializeField]private AttackBox attackBox;

        [Header("IZombieState")]
        public IZombieState ChaseState { get; private set; }
        public IZombieState DeadState { get; private set; }
        public IZombieState AttackState { get; private set; }

        private IZombieState currentState;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            currentHealth = maxHp;

            ChaseState = new ZombieChaseState(Random.Range(1,4));
            DeadState = new ZombieDeadState();
            AttackState = new NomalZombieAttack();
        }
        private void OnEnable()
        {
            if (GameManager.Instance.PlayerList.Count > 0)
            {
                chaseTarget = GameManager.Instance.GetPlayer();
                ChangeState(ChaseState);
            }
            else
            {
                StartCoroutine(WaitForPlayers());
            }
        }

        private IEnumerator WaitForPlayers()
        {
            yield return new WaitUntil(() => GameManager.Instance.PlayerList.Count > 0);
            chaseTarget = GameManager.Instance.GetPlayer();
            ChangeState(ChaseState);
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
            currentHealth -= damage;
        }

        public void Die()
        {
            Animator.SetTrigger("Die");
            Invoke("ReturnZombie", 2);
            // 사망 애니메이션, 콜라이더 비활성화 등
        }
        public void ReturnZombie()
        {
            currentHealth = maxHp;
            ChangeState(ChaseState);
            // 풀로 반환
            ZombiePoolManager.Instance.ReturnZombie(gameObject);
        }
        public void TryHit()
        {
            if (attackBox.isInAttackRange && attackBox.targetPlayer != null)
            {
                // 데미지 처리
                attackBox.targetPlayer.GetComponent<Health>()?.TakeDamage(zombieDamege);
            }
        }

    }
}