using UnityEngine;

public class WinPoint : MonoBehaviour
{
    public float rayLength = 5f;
    public LayerMask collisionLayer;

    private bool hasWon = false; // Biến cờ để kiểm tra đã thắng hay chưa

    void Update()
    {
        if (hasWon) return;

        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, collisionLayer);

        if (hit.collider != null)
        {
            Debug.Log("Va chạm với: " + hit.collider.name);

            int indexLevel = PlayerPrefs.GetInt("SelectedLevel");
            int currentPoint = GameManager.Instance.getCurrentPoint();

            Debug.Log($"{indexLevel} + {currentPoint}");

            AudioManager.Instance.PlayCompleteLevel();
            UIManager.Instance.onResultLevel("You Win");
            ButtonUIManager.Instance.UnLockLevelButton();
            LevelManager.Instance.OnLevelComplete(indexLevel, currentPoint);

            hasWon = true;
        }
    }

    public void ResetWinState()
    {
        hasWon = false;
    }

    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, collisionLayer);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, direction * rayLength);
    }
}