using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class GridMovementTilemap : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    [Header("References")]
    public Tilemap groundTilemap; // Kéo Tilemap Ground vào đây trong Inspector

    private bool isMoving = false;
    private Vector3 lastSafePos; // vị trí tile hợp lệ cuối cùng

    void Start()
    {
        // Căn nhân vật chính giữa ô
        Vector3Int cell = groundTilemap.WorldToCell(transform.position);
        transform.position = groundTilemap.GetCellCenterWorld(cell);
        lastSafePos = transform.position;
    }

    void Update()
    {
        if (isMoving) return;

        Vector2 input = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow)) input = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) input = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) input = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) input = Vector2.right;

        if (input != Vector2.zero)
        {
            Vector3 nextPos = transform.position + new Vector3(input.x, input.y, 0) * gridSize;

            // Nếu ô kế tiếp có tile => di chuyển bình thường
            if (HasTileAt(nextPos))
            {
                lastSafePos = groundTilemap.GetCellCenterWorld(groundTilemap.WorldToCell(nextPos));
                StartCoroutine(MoveTo(nextPos));
            }
            else
            {
                // Nếu không có tile => bước ra thêm 1 ô rồi thực hiện hiệu ứng rơi
                Debug.Log("❌ Game Over: Ra khỏi Tilemap!");
                Vector3 fallPos = transform.position + new Vector3(input.x, input.y, 0) * gridSize; // bước ra 1 ô
                StartCoroutine(FallOutOfMap(fallPos));
            }
        }
    }

    private bool HasTileAt(Vector3 worldPos)
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        return groundTilemap.HasTile(cellPos);
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;

        while ((target - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    private IEnumerator FallOutOfMap(Vector3 fallTarget)
    {
        isMoving = true;

        // Bước ra khỏi map
        while ((fallTarget - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallTarget, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Trừ máu sau khi rơi
        PlayerHealth hp = GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(1);
        }

        // Nếu còn sống -> thực hiện hiệu ứng hồi sinh
        if (hp == null || !hp.IsDead())
        {
            yield return StartCoroutine(RespawnEffect());
        }

        isMoving = false;
    }

    private IEnumerator RespawnEffect()
    {
        // Thu nhỏ
        float shrinkTime = 0.3f;
        Vector3 startScale = transform.localScale;
        float t = 0f;

        while (t < shrinkTime)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / shrinkTime);
            yield return null;
        }

        // Hồi sinh lại vị trí an toàn
        transform.position = lastSafePos;

        // Phóng to lại
        float growTime = 0.3f;
        t = 0f;
        while (t < growTime)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, startScale, t / growTime);
            yield return null;
        }

        transform.localScale = startScale;
    }

}
