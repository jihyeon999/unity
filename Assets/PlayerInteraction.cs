using UnityEngine;
using TMPro; // TextMeshPro 관련 기능을 쓰기 위해 필수!

public class PlayerInteraction : MonoBehaviour
{
    [Header("설정")]
    public float interactDistance = 2.5f;   // 상호작용 가능한 최대 거리
    public LayerMask interactLayer;         // 감지할 레이어 (Interactable)
    public TextMeshProUGUI promptText;      // 화면에 띄울 [E] 열기 텍스트 UI

    void Start()
    {
        // 게임이 시작할 때는 안내 텍스트를 숨깁니다.
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        // 카메라의 정중앙(크로스헤어 위치)에서 앞방향으로 레이저(Ray)를 만듭니다.
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 레이저를 쏘아서 설정한 거리 내에 'Interactable' 레이어를 가진 물체가 부딪혔는지 검사합니다.
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // 부딪힌 물체에 'DrawerInteract' 스크립트가 붙어있는지 확인합니다.
            DrawerInteract drawer = hit.collider.GetComponent<DrawerInteract>();

            if (drawer != null)
            {
                // 1. 서랍을 조준 중이므로 텍스트 UI를 화면에 보여줍니다.
                promptText.gameObject.SetActive(true);

                // ★ 이 줄을 추가해 주세요! (서랍이 열려있으면 닫기, 닫혀있으면 열기로 텍스트 변경)
                promptText.text = drawer.IsOpen ? "[E] 닫기" : "[E] 열기";

                // 2. 이 상태에서 플레이어가 E 키를 누르면 서랍의 TryInteract() 함수를 실행합니다.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    drawer.TryInteract();
                }

                // 서랍을 찾아서 처리했으므로 아래에 있는 '텍스트 숨기기' 코드로 넘어가지 않도록 리턴합니다.
                return;
            }
            // ★ 여기부터 추가! (서랍이 아니면 아이템인지 확인)
            // 2. 아이템(ItemPickup)인지 확인
            ItemPickup pickup = hit.collider.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                // 아이템을 조준 중이므로 안내 텍스트 활성화
                promptText.gameObject.SetActive(true);

                // 데이터에 저장된 아이템 이름을 가져와서 띄워주면 더 친절하겠죠?
                promptText.text = "[E] 줍기: " + pickup.itemData.itemName;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickup.Pickup();
                }
                return; // 처리가 끝났으므로 함수 종료
            }
        }

        // 아무것도 맞지 않았거나, 상호작용 컴포넌트가 없는 경우 UI 숨김
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    

        // 레이저가 아무것도 안 맞았거나, 맞았더라도 DrawerInteract 가 없다면 텍스트를 숨깁니다.
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
}