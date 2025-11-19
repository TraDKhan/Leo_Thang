using UnityEngine;

public enum DeathZoneType
{
    Static,
    Rising,
    FollowPlayer,
    LaserPulse
}

[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
    [Header("Cấu hình chung")]
    public DeathZoneType zoneType = DeathZoneType.FollowPlayer;
    public Transform player;
    public float gridSize = 1f;         // Kích thước mỗi ô lưới
    public float safeDistance = 5f;     // Khoảng cách an toàn tính theo trục Y

    [Header("Tùy chọn Rising / Pulse")]
    public float moveSpeed = 2f;
    public float laserDuration = 1.5f;
    public float laserCooldown = 1f;
    public SpriteRenderer laserSprite;

    private bool laserActive = true;
    private float laserTimer;
    private float nextSafeHeight;       // Ngưỡng mà DeathZone sẽ nhảy lên
    private float currentGridY;         // Vị trí hiện tại theo grid

    void Start()
    {
        // Làm tròn vị trí ban đầu theo grid
        currentGridY = Mathf.Round(transform.position.y / gridSize) * gridSize;
        nextSafeHeight = currentGridY;
    }

    void Update()
    {
        switch (zoneType)
        {
            case DeathZoneType.Static:
                break;

            case DeathZoneType.Rising:
                HandleRising();
                break;

            case DeathZoneType.FollowPlayer:
                HandleFollowPlayerGrid();
                break;

            case DeathZoneType.LaserPulse:
                HandleLaserPulse();
                break;
        }
    }

    // ========== FOLLOW PLAYER GRID MODE ==========
    private void HandleFollowPlayerGrid()
    {
        if (player == null) return;

        float playerY = player.position.y;
        float currentY = transform.position.y;

        // Nếu player cao hơn vùng chết quá khoảng an toàn -> vùng chết nhảy lên 1 grid
        if (playerY - currentY > safeDistance)
        {
            nextSafeHeight += gridSize;
            MoveToNextGrid();
        }
    }

    private void MoveToNextGrid()
    {
        // Nhảy lên đúng 1 ô grid
        currentGridY += gridSize;
        transform.position = new Vector3(transform.position.x, currentGridY, transform.position.z);
        // (Có thể thêm hiệu ứng rung hoặc âm thanh tại đây)
    }

    // ========== RISING ==========
    private void HandleRising()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    // ========== LASER PULSE ==========
    private void HandleLaserPulse()
    {
        if (laserSprite == null) return;

        laserTimer += Time.deltaTime;

        if (laserActive && laserTimer > laserDuration)
        {
            SetLaserActive(false);
        }
        else if (!laserActive && laserTimer > laserCooldown)
        {
            SetLaserActive(true);
        }
    }

    private void SetLaserActive(bool active)
    {
        laserActive = active;
        laserTimer = 0;
        laserSprite.enabled = active;
        GetComponent<Collider2D>().enabled = active;
    }

    // ========== VA CHẠM ==========
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log($"Player chết bởi {zoneType}");
        KillPlayer(collision.gameObject);
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        // Gọi GameManager nếu có
        // GameManager.Instance.RestartLevel();
        PlayerHealth.Instance.Die();
    }
}
