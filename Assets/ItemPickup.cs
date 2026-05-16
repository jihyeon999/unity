using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; // 아까 만든 ScriptableObject 연결

    public void Pickup()
    {
        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject); // 월드에서 제거
    }
}