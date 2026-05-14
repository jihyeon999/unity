using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;        // 이동 속도
    public float mouseSensitivity = 2f; // 마우스 감도

    [Header("중력 설정")]
    public float gravity = -9.81f;      // 중력
    public float groundedGravity = -2f; // 땅에 붙어있게 하는 작은 중력값

    [Header("카메라")]
    public Camera playerCamera;         // 플레이어 카메라

    private CharacterController controller;
    private float xRotation = 0f;       // 카메라 상하 회전값
    private Vector3 velocity;           // 중력 속도 저장용

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // 마우스 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 마우스 클릭하면 커서 다시 잠금
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        LookAround();
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A / D
        float vertical = Input.GetAxisRaw("Vertical");     // W / S

        // 플레이어가 바라보는 방향 기준으로 이동
        Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;

        // 대각선 이동 속도 보정
        if (moveDir.magnitude > 1f)
        {
            moveDir.Normalize();
        }

        // 수평 이동
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 바닥에 닿아 있으면 아래로 살짝 눌러줌
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = groundedGravity;
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 수직 이동
        controller.Move(velocity * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 플레이어 몸통 좌우 회전
        transform.Rotate(Vector3.up * mouseX);

        // 카메라 상하 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}