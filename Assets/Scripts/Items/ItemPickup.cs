using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    public Item itemData;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;
        if (!collision.CompareTag("Player")) return;

        isCollected = true;

        HandlePickup(collision);

        // Hiệu ứng chung khi nhặt item
        PlayPickupEffect();

        Destroy(gameObject, 0.05f); // xóa vật phẩm sau khi nhặt
    }

    private void HandlePickup(Collider2D player)
    {
        switch (itemData.itemType)
        {
            case ItemType.Gold:
                CollectGold(player);
                break;

            case ItemType.Heart:
                CollectHeart(player);
                break;

            case ItemType.SpeedBoost:
                CollectSpeedBoost(player);
                break;

                // Mở rộng thêm tại đây
        }
    }

    // ==================================================
    // 🪙 GOLD
    // ==================================================
    private void CollectGold(Collider2D player)
    {
        //if (GameDataManager.Instance != null)
        //{
        //    GameDataManager.Instance.currentData.AddGold(itemData.value);
        //    GameDataManager.Instance.SaveGame();
        //}

        Debug.Log($"💰 Collected Gold +{itemData.value}");
    }

    // ==================================================
    // ❤️ HEART
    // ==================================================
    private void CollectHeart(Collider2D player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(itemData.value);
            Debug.Log($"❤️ Healed +{itemData.value}");
        }
    }

    // ==================================================
    // ⚡ SPEED BOOST
    // ==================================================
    private void CollectSpeedBoost(Collider2D player)
    {
        Debug.Log($"Speed boost");
    }

    // ==================================================
    // ✨ Hiệu ứng chung khi nhặt item
    // ==================================================
    private void PlayPickupEffect()
    {
        // TODO: thêm particle, âm thanh hoặc animation tùy bạn
        // Ví dụ:
        // Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
    }
}
