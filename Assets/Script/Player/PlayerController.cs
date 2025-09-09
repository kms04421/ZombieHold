using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("PlayeData")]
    public PlayerData playerData;
    [Header("GunData")]
    public GameObject weapon;
    public RecoilController weaponRecoil;

    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float backSpeed = 2f;
    private float jumpForce = 1f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    [HideInInspector] public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("무기 슬롯")]
    public GunBase currentGun;
    
    private IPlayerState currentState;

    void Awake()
    {
        playerData = new PlayerData();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        ChangeState(new NormalState(this));
    }

    void Update()
    {
        currentState.HandleInput();
        currentState.UpdateState();
    }

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
            currentSpeed = backSpeed;
        else if (Input.GetKey(KeyCode.LeftShift) && moveDir.magnitude > 0.1f)
        {
            currentSpeed = runSpeed;
            isRunning = true;
        }
        else if (moveDir.magnitude > 0.1f)
            currentSpeed = walkSpeed;

        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool("isJump", true);
            isGrounded = false;
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

    public void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        PlayerStateData.xRotation -= mouseY;
        PlayerStateData.xRotation = Mathf.Clamp(PlayerStateData.xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(PlayerStateData.xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void EnableMovement(bool enable)
    {
        controller.enabled = enable;
    }
    #endregion
    public void SetWeapon(GameObject go)
    {
        if(go != null)
        {
            weapon = go;
            RecoilController recoilController = go.GetComponent<RecoilController>();
            if(recoilController != null)
            {
                weaponRecoil = recoilController;
            }
            else
            {
                weaponRecoil = null;
            }
        }
    }


    public void Reload()
    {
        // IK 끄기
        GetComponent<FrInverseKinematic>().SetIKActive(false);

        // 리로드 애니메이션 실행
        animator.SetTrigger("Reload");

        // 코루틴으로 일정 시간 후 다시 IK 켜기 (애니메이션 길이에 맞춰 조절)
        StartCoroutine(RestoreIKAfterDelay(3.5f));
    }

    private IEnumerator RestoreIKAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<FrInverseKinematic>().SetIKActive(true);
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            currentGun?.StartFiring();
        else if (context.canceled)
            currentGun?.StopFiring();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
            currentGun?.Reload();
    }

    public void EquipGun(GunBase newGun)
    {
        if (currentGun != null)
            currentGun.gameObject.SetActive(false);

        currentGun = newGun;
        currentGun.gameObject.SetActive(true);
    }
}
