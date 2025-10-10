using System.Collections;
using System.Xml;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [Header("PlayeData")]
    public PlayerData playerData;
   
    [Header("GunData")]
    public GameObject weapon;

    [Header("Movement")]
    private const float jumpForce = 1f;
    public float gravity = -9.81f;
    private float xRecoilOffset = 0f; // Recoil이 줄 값
    private float yRecoilOffset = 0f; // 좌우 반동
    [Header("Mouse Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    [HideInInspector] public Animator animator;
    private FrInverseKinematic frInverseKinematic;
    private CharacterController controller;
    private DebuffHandler debuffHandler;
    private Vector3 velocity;

    public bool isGrounded;

    [Header("무기 슬롯")]
    public GunBase currentGun;

    public Vector3 jumpPos; // 점프시 임시 저장위치 좀비추격에 필요

    private IPlayerState currentState;

    ///상태패턴
    public IPlayerState NormalState { get; private set; }
    public IPlayerState InventoryState { get; private set; }
    public IPlayerState MenuState { get; private set; }
    public IPlayerState DeadState { get; private set; }
    ///상태패턴
    ///
    void Awake()
    {
        playerData = new PlayerData();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        frInverseKinematic = GetComponent<FrInverseKinematic>();
    }

    void Start()
    {
        debuffHandler = GetComponent<DebuffHandler>();
        DeadState = new DeadState(this);
        NormalState = new NormalState(this);
        InventoryState = new InventoryState(this);
        MenuState = new MenuState(this);
        SetWeapon();
        ChangeState(NormalState);
    }

    void Update()
    {
        currentState.HandleInput();
        currentState.UpdateState();
    }
    /// <summary>
    /// 상태 변경
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    #region Movement & Look
    public void Move()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        moveDir.Normalize();

        float currentSpeed = 0f;
        bool isRunning = false;

        if (moveZ < 0)
            currentSpeed = playerData.BackSpeed;
        else if (Input.GetKey(KeyCode.LeftShift) && moveDir.magnitude > 0.1f)
        {
            currentSpeed = playerData.RunSpeed;
            isRunning = true;
        }
        else if (moveDir.magnitude > 0.1f)
            currentSpeed = playerData.WalkSpeed;

        // 디버프
        if (debuffHandler != null)
        {
            if (debuffHandler.IsStunned())
            {
                // 행동 불가 처리
                currentSpeed = 0f;
            }
            else
            {
                // 속도 적용
                currentSpeed = currentSpeed * debuffHandler.GetSpeedModifier();
            }
        }
        // 디버프
        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -1.5f * gravity);
            animator.SetBool("isJump", true);
            isGrounded = false;
            jumpPos = GetGroundPoint();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetBool("isMove", moveDir.magnitude > 0.1f);
        animator.SetBool("isRun", isRunning);
        animator.SetFloat("xDir", moveX);
        animator.SetFloat("yDir", -moveZ);

        if (isGrounded && animator.GetBool("isJump"))
            animator.SetBool("isJump", false);
    }
    public void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // 바닥에 닿으면 속도 초기화
        }

        velocity.y += gravity * Time.deltaTime;      // 중력 누적
        controller.Move(velocity * Time.deltaTime);  // 이동 적용
    }
    private Vector3 GetGroundPoint()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            return hit.point; // 레이가 땅에 닿은 위치 반환
        }

        // 맞은 게 없을 경우, 자기 위치 그대로 반환
        return transform.position;
    }
    public void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 마우스 Y + 반동 가산
        PlayerStateData.xRotation -= (mouseY - xRecoilOffset);
        xRecoilOffset = 0f; // 한 번 적용했으니 초기화

        PlayerStateData.xRotation = Mathf.Clamp(PlayerStateData.xRotation, -90f, 90f);

        // 좌우 회전 적용 (마우스 + 반동)
        float totalYaw = mouseX + yRecoilOffset;
        transform.Rotate(Vector3.up * totalYaw);
        yRecoilOffset = 0f;

        cameraTransform.localRotation = Quaternion.Euler(PlayerStateData.xRotation, 0f, 0f);
    }

    // 외부에서 반동을 주입
    public void ApplyRecoil(float verticalAmount, float horizontalAmount)
    {
        xRecoilOffset -= verticalAmount;
        yRecoilOffset += horizontalAmount;
    }
    /// <summary>
    /// 캐릭터 컨트롤로 활성화 비활성화 여부
    /// </summary>
    /// <param name="enable"></param>
    public void EnableMovement(bool enable)
    {
        controller.enabled = enable;
    }
    #endregion

    /// <summary>
    /// 무기 장착 시
    /// </summary>
    /// <param name="go"></param>
    public void SetWeapon()
    {
        currentGun.SetPlayerController(this);
    }

    /// <summary>
    /// 재장전 애니메이션 실행
    /// </summary>
    public void ReloadAnimaion()
    {
        // IK 끄기
        frInverseKinematic.SetIKActive(false);
        // 리로드 애니메이션 실행
        animator.SetTrigger("Reload");
  
        // 코루틴으로 일정 시간 후 다시 IK 켜기 (애니메이션 길이에 맞춰 조절)
        StartCoroutine(RestoreIKAfterDelay(3.1f));
    }
    /// <summary>
    /// 일정시간후 ik활성화
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator RestoreIKAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        frInverseKinematic.SetOrgPos();
        frInverseKinematic.SetIKActive(true);
        isReload = false;
    }

    public void DetachMagazine()
    {
        currentGun.Magazine();
    }
    public void AttachMagazine()
    {
        currentGun.ReturnMagazine();
    }
    /// <summary>
    /// 캐릭터가 총기 발사 ,중지
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(bool isShoot)
    {
        if (currentState != NormalState) return;
        if (isReload) return;

        if (isShoot)
            currentGun?.StartFiring();
        else
            currentGun?.StopFiring();

    }
    private bool isReload = false; // 장전중 액션 정지용
    /// <summary>
    /// 재장전 인풋시스템 사용
    /// </summary>
    /// <param name="context"></param>
    public void OnReload()
    {
        if (currentState != NormalState) return;
        if (currentGun == null) return;
        if (!isReload)
        {
            isReload = true;
            bool isbool = currentGun.Reload();

            if (isbool)
                ReloadAnimaion();
            else
                isReload = false;

        }

    }
    /// <summary>
    /// 총기 변경
    /// </summary>
    /// <param name="newGun"></param>
    public void EquipGun(GunBase newGun)
    {
        if (currentGun != null)
            currentGun.gameObject.SetActive(false);

        currentGun = newGun;
        currentGun.gameObject.SetActive(true);
    }
  /// <summary>
  /// 현재 상태 체크
  /// </summary>
    public bool ChkState(IPlayerState playerState)
    {
        return currentState == playerState;
    }
    /// <summary>
    /// 어빌리티 능력 playerData 적용
    /// </summary>
    /// <param name="_playerData"></param>
    public void SetStats(PlayerData _playerData)
    {
        playerData.AddPlayData(_playerData);
    }
    
}
