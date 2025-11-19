using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    public Item itemData;

    private Tilemap groundTilemap;
    private bool isCollected = false;

    private void Start()
    {
        if (groundTilemap == null)
            groundTilemap = FindActiveGroundTilemap();

        Vector3Int cell = groundTilemap.WorldToCell(transform.position);
        transform.position = groundTilemap.GetCellCenterWorld(cell);
    }
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
            case ItemType.Point:
                CollectPoint(player); 
                break;

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
    public void CollectPoint(Collider2D player)
    {
        Debug.Log("+ " + itemData.value);
        GameManager.Instance.setCurrentPoint(itemData.value);
        AudioManager.Instance.PlayPickUpItem();
        UIManager.Instance.updateScoreUI();
    }
    private void CollectGold(Collider2D player)
    {
        AudioManager.Instance.PlayPickUpCoin();
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.currentData.AddGold(itemData.value);
            GameDataManager.Instance.SaveGame();
        }
        Debug.Log($"💰 Collected Gold +{itemData.value}");
    }
    private void CollectHeart(Collider2D player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(itemData.value);
            Debug.Log($"❤️ Healed +{itemData.value}");
        }
    }
    private void CollectSpeedBoost(Collider2D player)
    {
        Debug.Log($"Speed boost");
    }
    private void PlayPickupEffect()
    {
        // TODO: thêm particle, âm thanh hoặc animation tùy bạn
        // Ví dụ:
        // Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
    }
    private Tilemap FindActiveGroundTilemap()
    {
        var allTilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.InstanceID);
        foreach (var map in allTilemaps)
        {
            if (map.gameObject.activeInHierarchy && map.gameObject.CompareTag("Ground"))
                return map;
        }
        return null;
    }
}
