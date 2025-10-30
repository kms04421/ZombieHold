using System.Collections;
using Unity.VisualScripting;
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
            data.speed = Random.Range(1.5f, 4);
            Agent.speed = data.speed;

        }
        /// <summary>
        /// 추격할 플레이어 설정
        /// </summary>
        /// <returns></returns>
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
        public void ChangeState(IZombieState nextState)
        {
           // Debug.Log(currentState);
            currentState?.Exit(this);
            currentState = nextState;
            currentState.Enter(this);
           
        }
        /// <summary>
        /// 데미지 피격시 발동 함수
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="hitType"></param>
        public void TakeDamage(float damage, HitType hitType)
        {
            if (currentState == DeadState) return;
            MultiClient.Instance.SendHitZombieToServer(data.id, damage);
       
        }
        public void OnHit( float hp, bool dead)
        {  
            if (dead)
            {
                Debug.Log("OnHit(전)" + hp);
                ChangeState(DeadState);
                Debug.Log("OnHit(후)");
                return;
            }
            else
            {
                if (HitDlayCoroutine != null)
                {
                    StopCoroutine(HitDlayCoroutine); // 기존 코루틴 종료
                    HitDlayCoroutine = null;
                }

                HitDlayCoroutine = StartCoroutine(SlowDownOnHit());
            }
         
        } 
        /// <summary>
        /// 사망시
        /// </summary>
        public void Die()
        {
            Debug.Log("Die@@@@@@@@@@@");
            monsterDrop.DropLoot();
            Invoke("ReturnZombie", 3);
            // 사망 애니메이션, 콜라이더 비활성화 등
        }
        /// <summary>
        /// 좀비 풀로 반환
        /// </summary>
        public void ReturnZombie()
        {
            ChangeState(ChaseState);
            PoolManager.Instance.GetPool<Zombie>().ReturnToPool(this);
        }
        /// <summary>
        /// 좀비가 플레이어 공격시
        /// </summary>
        public void TryHit()
        {
            if (attackBox.isInAttackRange && attackBox.targetPlayer != null)
            {
                // 데미지 처리
                attackBox.targetPlayer.GetComponent<Health>()?.TakeDamage(data.attackDamage);
            }
        }
        /// <summary>
        /// 피격시 슬로우 적용
        /// </summary>
        /// <returns></returns>
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
        public bool DieChk()
        {
            return currentState == DeadState;
        }
    }
}

