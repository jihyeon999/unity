using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    [Header("서랍 설정")]
    public Transform drawer;           // 열릴 서랍 오브젝트 (cabinet1 or cabinet2)
    public float openOffset = 0.5f;    // 서랍이 열릴 X 거리
    public float openSpeed = 3f;       // 열리는 속도
    public bool isLocked = false;      // 잠김 여부

    [Header("상호작용 설정")]
    public float interactDistance = 2.5f;  // 바라봐야 하는 최대 거리

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private bool isMoving = false;
    //추가
    public bool IsOpen => isOpen; // => : get { retrun inOpen; } 을 줄인 것

    void Start()
    {
        closedPos = drawer.localPosition;
        openPos = closedPos + new Vector3(openOffset, 0f, 0f);
    }

    void Update()
    {
        // 서랍 열기/닫기 애니메이션
        if (isMoving)
        {
            Vector3 target = isOpen ? openPos : closedPos;
            drawer.localPosition = Vector3.Lerp(drawer.localPosition, target, Time.deltaTime * openSpeed);

            if (Vector3.Distance(drawer.localPosition, target) < 0.001f)
            {
                drawer.localPosition = target;
                isMoving = false;
            }
        }
    }

    // PlayerInteraction.cs에서 호출
    public void TryInteract()
    {
        if (isLocked)
        {
            Debug.Log("잠겨있다.");
            // 나중에 여기에 "잠겼습니다" UI 표시 추가 가능
            return;
        }

        isOpen = !isOpen;
        isMoving = true;
    }

    // 열쇠 등으로 잠금 해제할 때 호출
    public void Unlock()
    {
        isLocked = false;
        Debug.Log("잠금을 풀었다.");
    }
}
