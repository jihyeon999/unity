using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    [Header("서랍 설정")]
    public Transform drawer; //열릴 서랍 오브젝트(cabinet1 or cabinet2)
    public float openOffset = 0.5f; //서랍이 열릴 x 거리
    public float openSpeed = 3f; //열리는 속도
    public bool isLocked = false; //잠김 여부

    // 추가: 이 서랍을 여는 데 필요한 열쇠 이름
    [Header("잠금 설정")]
    public string requiredKeyName = "서랍 열쇠";

    [Header("상호작용 설정")]
    public float interactDistance = 2.5f; //바라봐야 하는 최대 거리

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private bool isMoving = false;

    public bool IsOpen => isOpen; // => : get { retrun inOpen; } 을 줄인 것

    void Start()
    {
        closedPos = drawer.localPosition;
        // 현재 X축으로 이동하게 되어 있는데, 
        // 만약 옆으로 열린다면 new Vector3(0f, 0f, openOffset) 등으로 수정하세요.
        openPos = closedPos + new Vector3(openOffset, 0f, 0f);
    }

    void Update()
    {
        if (isMoving) //서랍 열기/닫기 애니메이션
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
        // 1. 잠겨있는 경우
        if (isLocked)
        {
            // 인벤토리에 열쇠가 있는지 확인 (싱글톤 매니저 호출)
            if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredKeyName))
            {
                Unlock(); // 열쇠가 있으면 잠금 해제
            }
            else
            {
                Debug.Log(requiredKeyName + "이(가) 필요하다.");
                // 여기에 "열쇠가 필요합니다" 같은 UI 연출을 넣으면 좋습니다.
                return;
            }
        }

        // 2. 잠겨있지 않거나 방금 해제했다면 열기/닫기 실행
        isOpen = !isOpen;
        isMoving = true;
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log("잠금을 풀었다.");
    }
}