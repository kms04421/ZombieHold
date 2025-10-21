using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
namespace YourGame.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour
    {
        public enum HitType { Normal, Head, Leg }

        [Header("Chase")]
        [SerializeField] private PlayerController chaseTarget; // 추적 대상
        public PlayerController ChaseTarget => chaseTarget;

        [Header("ZombieData")]
        public ZombieData data;
        public float currentHealth { get; private set; }
        [Header("Component ")]
        private MonsterDrop monsterDrop;
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; }
        [SerializeField]private AttackBox attackBox;

        [Header("IZombieState")]
        public IZombieState ChaseState { get; private set; }
        public IZombieState DeadState { get; private set; }
        public IZombieState AttackState { get; private set; }

        private IZombieState currentState;

        private WaitForSeconds dlay = new WaitForSeconds(0.1f);
        private Coroutine HitDlayCoroutine;
        private PoolManager poolManager;
        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            monsterDrop = GetComponent<MonsterDrop>();
            poolManager = PoolManager.Instance;

            ChaseState = new ZombieChaseState();
            DeadState = new ZombieDeadState();
          
            switch (data.atkType)
            {
                case "Nomal":
                    AttackState = new NomalZombieAttack();
                    break;
                case "Explosion":
                    AttackState = new ExplosionZombieAttack();
                    Debug.Log("Explosion");
                    break;
                default:
                    Debug.Log(data.atkType);
                    Debug.Log("없음");
                    break;
            }
            Init();
        }
        private void OnEnable()
        {
            if (GameManager.Instance.PlayerList.Count > 0)
            {
                chaseTarget = GameManager.Instance.GetPlayer;
                ChangeState(ChaseState);
                Init();
            }
            else
            {
                StartCoroutine(WaitForPlayers()); // 뭔가 리스폰 방법이있을때
            }
        }
        public void Init()
        {
            int dayCount = GameManager.Instance.dayCount;
            data.maxHp = (dayCount * 100f) + Mathf.Pow(dayCount, 1.2f) * 20f;
            currentHealth = data.maxHp;
            data.speed = Random.Range(1.5f, 4);
            Agent.speed = data.speed;

        }
        public void Init(ZombieData data, int wave)
        {
            data.maxHp = (wave * data.maxHp) + Mathf.Pow(wave, 1.2f) * data.hpMultiplier;
            currentHealth = data.maxHp;
            data.speed = Random.Range(data.minSpeed, data.maxSpeed);
            Agent.speed = data.speed;
        }
        private IEnumerator WaitForPlayers()
        {
            yield return new WaitUntil(() => GameManager.Instance.PlayerList.Count > 0);
            chaseTarget = GameManager.Instance.GetPlayer;
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
            if (HitDlayCoroutine != null)
            {
                StopCoroutine(HitDlayCoroutine); // 기존 코루틴 종료
                HitDlayCoroutine = null;
            }

            HitDlayCoroutine = StartCoroutine(SlowDownOnHit());
        }

        public void Die()
        {
            monsterDrop.DropLoot();
            Invoke("ReturnZombie", 3);
            // 사망 애니메이션, 콜라이더 비활성화 등
        }
        public void ReturnZombie()
        {
            currentHealth = data.maxHp;
            ChangeState(ChaseState);
            PoolManager.Instance.GetPool<Zombie>().ReturnToPool(this);
        }
        public void TryHit()
        {
            if (attackBox.isInAttackRange && attackBox.targetPlayer != null)
            {
                // 데미지 처리
                attackBox.targetPlayer.GetComponent<Health>()?.TakeDamage(data.attackDamage);
            }
        }
        public IEnumerator SlowDownOnHit()
        {
            float currentSpeed = data.speed - 1;
            while (data.speed >= currentSpeed)
            {
                Agent.speed = currentSpeed;
                currentSpeed += 0.1f;
                yield return dlay;
            }
            Agent.speed = data.speed;
        }
    }
}