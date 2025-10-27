using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [Header("PlayeData")]
    public PlayerData playerData;

    [Header("PlayerHand")]
    [SerializeField] private Transform LeftHend;
    [SerializeField] private Transform RightHend;
   
    [Header("GunData")]
    public GameObject weapon;

    [Header("Movement")]
    private const float jumpForce = 1f;
    private float gravity = -9.81f;

    [Header("References")]
    public CinemachineCamera vCam; // 최신 Cinemachine 3
    private CinemachineBasicMultiChannelPerlin noiseComponent;

    [Header("Mouse Look")]
    private float mouseSensitivity = 2f; // 마우스 감도
    private float xRotation = 0f;
    
    //컴퍼넌트용
    [HideInInspector] public Animator animator;
    private FrInverseKinematic frInverseKinematic;
    private CharacterController controller;
    private buffHandler buffHandler;
    private Vector3 velocity;

    [HideInInspector]public bool isGrounded;

    [Header("무기 슬롯")]
    public GunBase currentGun;

    [Header("Zoom Settings")]
    public float zoomFOV = 30f;
    private float defaultFOV;
    public float zoomSpeed = 10f;
    private bool isZooming = false;

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
        noiseComponent = vCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        if (noiseComponent == null)
            Debug.LogWarning("Noise component not found!");

        defaultFOV = vCam.Lens.FieldOfView; // Lens에서 직접 FOV


        buffHandler = GetComponent<buffHandler>();
        DeadState = new DeadState(this);
        NormalState = new NormalState(this);
        InventoryState = new InventoryState(this);
        MenuState = new MenuState(this);
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
        if (buffHandler != null)
        {
            if (buffHandler.IsStunned())
            {
                // 행동 불가 처리
                currentSpeed = 0f;
            }
            else
            {
                // 속도 적용
                currentSpeed = currentSpeed * buffHandler.GetSpeedModifier();
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
        // 마우스 입력
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 카메라 위/아래 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        vCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
     
        // 플레이어 몸체 좌/우 회전
        transform.Rotate(Vector3.up * mouseX);
    }
    //추가 start

   /// <summary>
   /// 화면 흔들림 
   /// </summary>
   /// <param name="amplitude"></param>
   /// <param name="frequency"></param>
   /// <param name="duration"></param>
    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        StartCoroutine(DoShake(amplitude, frequency, duration));
    }

    private IEnumerator DoShake(float amplitude, float frequency, float duration)
    {

        // 최신 버전에서는 m_AmplitudeGain 대신 AmplitudeGain 사용
        noiseComponent.AmplitudeGain = amplitude;
        noiseComponent.FrequencyGain = frequency;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        noiseComponent.AmplitudeGain = 0f;
        noiseComponent.FrequencyGain = 0f;
    }
    #region Zoom
    public void SetZoom(bool zoom)
    {
        isZooming = zoom;
    }

    private void HandleZoom()
    {
        float targetFOV = isZooming ? zoomFOV : defaultFOV;
        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
    #endregion
    //추가 end

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
    /// 재장전 애니메이션 실행
    /// </summary>
    public void ReloadAnimaion()
    {
        // IK 끄기
        frInverseKinematic.SetIKReloadActive(false);
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
        frInverseKinematic.SetIKReloadActive(true);
        isReload = false;
    }
    /// <summary>
    /// 장전시 탄창 위치 벗어낫다가 돌아옴
    /// </summary>
    public void DetachMagazine()
    {
        currentGun.Magazine(LeftHend);
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
        {
            ShakeCamera(0.7f, 0.7f, 0.1f);
            currentGun?.StartFiring();
        }    
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
    /// 총기 장착
    /// </summary>
    /// <param name="newGun"></param>
    public void EquipGun(GunBase newGun)
    {
        currentGun = newGun;
        frInverseKinematic.SetIK(true);
        animator.SetBool("IsEquipped", true);
    }
    /// <summary>
    /// 총기 장착해제
    /// </summary>
    /// <param name="newGun"></param>
    public void UnequipGun()
    {
        if (currentGun == null) return;
        currentGun.gameObject.SetActive(false);
        currentGun = null;
        frInverseKinematic.SetIK(false);
        animator.SetBool("IsEquipped", false);
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
