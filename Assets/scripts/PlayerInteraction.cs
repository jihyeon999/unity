using UnityEngine;
using TMPro; // TextMeshPro 관련 기능을 쓰기 위해

public class PlayerInteraction : MonoBehaviour
{
    [Header("설정")]
    public float interactDistance = 2.5f;   // 상호작용 가능한 최대 거리
    public LayerMask interactLayer;         // 감지할 레이어 (Interactable)
    public TextMeshProUGUI promptText;      // 화면에 띄울 [E] 안내 텍스트 UI

    void Start()
    {
        // 게임이 시작할 때는 텍스트 숨김
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
        // 카메라의 정중앙 방향으로 Ray 생성
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 설정한 거리 안에서 Interactable 레이어에 해당하는 물체를 감지
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // 맞은 오브젝트 또는 부모에서 DrawerInteract 찾기
            DrawerInteract drawer = hit.collider.GetComponentInParent<DrawerInteract>();

            if (drawer != null)
            {
                promptText.gameObject.SetActive(true);

                // 서랍이 열려 있으면 닫기, 닫혀 있으면 열기 표시
                if (drawer.IsOpen)
                {
                    promptText.text = "[E] 닫기";
                }
                else
                {
                    promptText.text = "[E] 열기";
                }

                // E 키를 누르면 서랍 상호작용 실행
                if (Input.GetKeyDown(KeyCode.E))
                {
                    drawer.TryInteract();
                }

                return;
            }

            // 아이템인지 확인
            ItemPickup pickup = hit.collider.GetComponent<ItemPickup>();

            if (pickup != null)
            {
                promptText.gameObject.SetActive(true);
                promptText.text = "[E] 줍기: " + pickup.itemData.itemName;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickup.Pickup();
                }

                return;
            }
        }

        // 아무것도 감지하지 못했거나 상호작용 가능한 오브젝트가 아니면 텍스트 숨김
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
}