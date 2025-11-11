using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class GridMovementTilemap : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    [Header("Auto-detect Tilemap")]
    public Tilemap groundTilemap;

    [Header("Checkpoint Prefab")]
    public GameObject flagPrefab;

    private bool isMoving = false;
    private Vector3 lastSafePos;
    private GameObject currentFlag;

    void Start()
    {
        if (groundTilemap == null)
            groundTilemap = FindActiveGroundTilemap();

        if (groundTilemap == null)
        {
            Debug.LogWarning("⚠️ Không tìm thấy Tilemap cho nhân vật!");
            return;
        }

        Vector3Int cell = groundTilemap.WorldToCell(transform.position);
        transform.position = groundTilemap.GetCellCenterWorld(cell);
        lastSafePos = transform.position;

        // 🔹 Cắm cờ đầu tiên (Flag_Idle) với Y tăng thêm 0.1f
        Vector2 flagPosition = new Vector3(lastSafePos.x, lastSafePos.y + 0.1f);
        PlaceFlag(flagPosition, isStart: true);

    }

    void Update()
    {
        if (isMoving || groundTilemap == null) return;

        Vector2 input = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow)) input = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) input = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) input = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) input = Vector2.right;

        if (input != Vector2.zero)
        {
            Vector3 nextPos = transform.position + new Vector3(input.x, input.y, 0) * gridSize;

            if (HasTileAt(nextPos))
            {
                lastSafePos = groundTilemap.GetCellCenterWorld(groundTilemap.WorldToCell(nextPos));
                StartCoroutine(MoveTo(nextPos));
            }
            else
            {
                Debug.Log("❌ Game Over: Ra khỏi Tilemap!");
                Vector3 fallPos = transform.position + new Vector3(input.x, input.y, 0) * gridSize;
                StartCoroutine(FallOutOfMap(fallPos));
            }
        }
    }

    private Tilemap FindActiveGroundTilemap()
    {
        var allTilemaps = FindObjectsOfType<Tilemap>(true);
        foreach (var map in allTilemaps)
        {
            if (map.gameObject.activeInHierarchy && map.gameObject.CompareTag("Ground"))
                return map;
        }
        return null;
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

        while ((fallTarget - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallTarget, moveSpeed * Time.deltaTime);
            yield return null;
        }

        PlayerHealth hp = GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(1);

        if (hp == null || !hp.IsDead())
            yield return StartCoroutine(RespawnEffect());

        isMoving = false;
    }

    private IEnumerator RespawnEffect()
    {
        float shrinkTime = 0.3f;
        Vector3 startScale = transform.localScale;
        float t = 0f;

        while (t < shrinkTime)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / shrinkTime);
            yield return null;
        }

        // 🔹 Hồi sinh tại vị trí an toàn
        transform.position = lastSafePos;


        // 🔹 Cắm flag mới (Flag_Out)
        Vector2 flagPosition = new Vector3(lastSafePos.x, lastSafePos.y + 0.1f);
        PlaceFlag(flagPosition, isStart: false);

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
    // 🏳️ Spawn hoặc thay flag
    private void PlaceFlag(Vector3 position, bool isStart)
    {
        if (flagPrefab == null)
        {
            Debug.LogWarning("⚠️ Chưa gán Prefab Flag!");
            return;
        }

        if (currentFlag != null)
            Destroy(currentFlag);

        currentFlag = Instantiate(flagPrefab, position, Quaternion.identity);

        CheckPoint cp = currentFlag.GetComponent<CheckPoint>();
        if (cp != null)
        {
            if (isStart)
                cp.PlayIdle();
            else
                cp.PlayOut();
        }
    }
    public void RefreshTilemap()
    {
        groundTilemap = FindActiveGroundTilemap();
        if (groundTilemap != null)
        {
            Vector3Int cell = groundTilemap.WorldToCell(transform.position);
            transform.position = groundTilemap.GetCellCenterWorld(cell);
            lastSafePos = transform.position;
        }
    }
}
