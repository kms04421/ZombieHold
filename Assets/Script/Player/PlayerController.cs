using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float backSpeed = 2f;
    private float jumpForce = 1f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;

    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        Move();
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        // ���� �� �ʱ�ȭ
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        moveDir.Normalize();

        // �ӵ� ����
        float currentSpeed = 0f;
        bool isRunning = false;

        if (moveZ < 0)
        {
            currentSpeed = backSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && moveDir.magnitude > 0.1f)
        {
            currentSpeed = runSpeed;
            isRunning = true;
        }
        else if (moveDir.magnitude > 0.1f)
        {
            currentSpeed = walkSpeed;
        }

        // �̵�
        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        // ����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool("isJump", true);  // ���� ����
            isGrounded = false;
            Debug.Log("jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // �ִϸ��̼� �Ķ����
        animator.SetBool("isMove", moveDir.magnitude > 0.1f);
        animator.SetBool("isRun", isRunning);
        animator.SetFloat("xDir", moveX);
        animator.SetFloat("yDir", -moveZ);     

        // ���� �Ǻ�
        if (isGrounded && animator.GetBool("isJump"))
        {
            animator.SetBool("isJump", false);  // ���� �� ���� ����
        }
    }
    }