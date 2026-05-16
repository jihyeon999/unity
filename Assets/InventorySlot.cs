using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    // 슬롯에 아이템을 표시하는 함수
    public void AddItem(Item newItem)
    {
        icon.sprite = newItem.icon;
        icon.enabled = true; // 아이콘 활성화
    }

    // 슬롯을 비우는 함수
    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false; // 아이콘 비활성화
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
