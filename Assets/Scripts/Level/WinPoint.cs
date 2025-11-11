using UnityEngine;

public class WinPoint : MonoBehaviour
{
    public float rayLength = 5f;
    public LayerMask collisionLayer;

    private bool hasWon = false; // Biến cờ để kiểm tra đã thắng hay chưa

    void Update()
    {
        if (hasWon) return; // Nếu đã thắng thì không kiểm tra nữa

        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, collisionLayer);

        Debug.DrawRay(origin, direction * rayLength, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Va chạm với: " + hit.collider.name);

            int indexLevel = PlayerPrefs.GetInt("SelectedLevel");
            int currentPoint = GameManager.Instance.getCurrentPoint();

            Debug.Log($"{indexLevel} + {currentPoint}");

            UIManager.Instance.onWinLevel();
            LevelManager.Instance.OnLevelComplete(indexLevel, 100);

            hasWon = true; // Đánh dấu đã thắng để không gọi lại
        }
    }

    // Nếu cần reset khi bắt đầu level mới, bạn có thể thêm hàm này:
    public void ResetWinState()
    {
        hasWon = false;
    }
}