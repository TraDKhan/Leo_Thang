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

    // 👇 Thêm phần dành cho cảm ứng
    private Vector2 startTouchPos;
    private bool isSwiping = false;
    private float minSwipeDistance = 50f; // pixel tối thiểu để nhận swipe

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

        // 🔹 Cắm cờ đầu tiên (Flag_Idle)
        Vector2 flagPosition = new Vector3(lastSafePos.x, lastSafePos.y + 0.1f);
        PlaceFlag(flagPosition, isStart: true);
    }

    void Update()
    {
        if (isMoving || groundTilemap == null) return;

        #if UNITY_EDITOR || UNITY_STANDALONE
        HandleKeyboardInput();
        #else
        HandleTouchInput();
        #endif
    }

    // ======================
    // 🔹 Xử lý cảm ứng vuốt
    // ======================
    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                startTouchPos = touch.position;
                isSwiping = true;
                break;

            case TouchPhase.Ended:
                if (!isSwiping) return;
                Vector2 delta = touch.position - startTouchPos;

                if (delta.magnitude > minSwipeDistance)
                {
                    Vector2 dir = Vector2.zero;
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                        dir = delta.x > 0 ? Vector2.right : Vector2.left;
                    else
                        dir = delta.y > 0 ? Vector2.up : Vector2.down;

                    TryMove(dir);
                }

                isSwiping = false;
                break;
        }
    }

    // ======================
    // 🔹 Xử lý bàn phím (test trong editor)
    // ======================
    private void HandleKeyboardInput()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            input = Vector2.up;
            AudioManager.Instance.PlayMove();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            input = Vector2.down;
            AudioManager.Instance.PlayMove();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            input = Vector2.left;
            AudioManager.Instance.PlayMove();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            input = Vector2.right;
            AudioManager.Instance.PlayMove();
        }

        if (input != Vector2.zero)
            TryMove(input);
    }

    // ======================
    // 🔹 Kiểm tra và di chuyển 1 ô
    // ======================
    private void TryMove(Vector2 input)
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

    // ======================
    // 🔹 Di chuyển mượt tới ô
    // ======================
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

    // ======================
    // 🔹 Khi rơi khỏi map
    // ======================
    private IEnumerator FallOutOfMap(Vector3 fallTarget)
    {
        isMoving = true;

        while ((fallTarget - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallTarget, moveSpeed * Time.deltaTime);
            yield return null;
        }

        AudioManager.Instance.PlayFall();
        PlayerHealth hp = GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(1);

        if (hp == null || !hp.IsDead())
            yield return StartCoroutine(RespawnEffect());

        isMoving = false;
    }

    // ======================
    // 🔹 Hồi sinh + flag
    // ======================
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

        transform.position = lastSafePos;

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

    // ======================
    // 🔹 Cắm / cập nhật flag
    // ======================
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

    // ======================
    // 🔹 Kiểm tra Tile và refresh
    // ======================
    private bool HasTileAt(Vector3 worldPos)
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        return groundTilemap.HasTile(cellPos);
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
