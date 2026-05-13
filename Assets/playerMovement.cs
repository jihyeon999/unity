using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;        // 이동 속도
    public float mouseSensitivity = 2f; // 마우스 감도

    [Header("카메라")]
    public Camera playerCamera;         // 플레이어 카메라 (First Person)

    private Rigidbody rb;
    private float xRotation = 0f;       // 카메라 상하 회전값

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 리지드바디 설정 - 플레이어가 넘어지지 않도록
        rb.freezeRotation = true;

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

        // 마우스로 시점 회전
        LookAround();
    }

    void FixedUpdate()
    {
        // WASD 이동 (물리 기반이라 FixedUpdate에서 처리)
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A / D
        float vertical = Input.GetAxisRaw("Vertical");   // W / S

        // 카메라가 바라보는 방향 기준으로 이동
        Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;

        // 정규화 (대각선 이동 시 속도 일정하게)
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        Vector3 targetVelocity = moveDir * moveSpeed;

        // Y축 속도(중력)는 건드리지 않음
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 플레이어 몸통: 좌우 회전
        transform.Rotate(Vector3.up * mouseX);

        // 카메라: 상하 회전 (위아래 각도 제한 -90 ~ 90)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
