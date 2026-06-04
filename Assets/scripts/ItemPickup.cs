using UnityEngine;

public class ItemPickup : MonoBehaviour //월드에 놓인 아이템에 붙이는 스크립트
{
    public Item itemData; // 아까 만든 ScriptableObject 연결

    public void Pickup()
    {
        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject); // 월드에서 제거
    }
}