using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // 어디서든 접근 가능하게 싱글톤 설정
    public List<Item> items = new List<Item>(); // 플레이어가 획득한 아이템 저장

    [Header("UI 설정")]
    public GameObject inventoryUI; // 에디터에서 InventoryPanel을 드래그해서 넣어줄 칸
    public Transform slotParent;      // 인벤토리 패널 (슬롯들의 부모 오브젝트) = 화면에 보이는 슬롯
    private InventorySlot[] slots;    // 모든 슬롯 리스트

    private bool isInventoryOpen = false; //시작 시 인벤토리 창 끄기

    void Awake() { Instance = this; }

    void Start()
    {
        // 부모 밑에 있는 모든 InventorySlot을 배열로 가져옴, 아이템 개수에 맞게 슬롯을 채움
        slots = slotParent.GetComponentsInChildren<InventorySlot>(true);
        UpdateUI();
    }

    void Update()
    {
        // 'I' 키를 누르면 인벤토리 열기
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // 아이템 추가 함수
    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        Debug.Log(newItem.itemName + " 획득!");
        if (GameMessageUI.Instance != null)
        {
            GameMessageUI.Instance.ShowMessage(newItem.itemName + "을(를) 획득했다.");
        }
        UpdateUI(); // 아이템을 먹을 때마다 화면 갱신
    }

    // 화면 갱신 핵심 로직
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count) // 가지고 있는 아이템 개수 안쪽이라면
            {
                slots[i].AddItem(items[i]); // 해당 아이템 표시
            }
            else
            {
                slots[i].ClearSlot(); // 나머지는 빈 칸 처리
            }
        }
    }

    public void RemoveItem(string targetName)
    {
        foreach (InventorySlot slot in slots) // 모든 슬롯을 하나씩 확인
        {
            // 슬롯의 이름이 내가 지우려는 아이템 이름과 같다면
            if (slot.itemName == targetName)
            {
                slot.ClearSlot(); 
                return; // 찾아서 지웠으니 종료
            }
        }
    }

    // 특정 아이템을 가지고 있는지 확인하는 함수
    public bool HasItem(string targetName)
    {
        return items.Exists(x => x.itemName == targetName);
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen; //토글: 입력을 받을 때마다 상태를 반대로 바꿈
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            // 인벤토리가 열리면 마우스 커서를 자유롭게 함
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 인벤토리가 닫히면 다시 화면 중앙에 가둠
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
