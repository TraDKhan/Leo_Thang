using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;        // tốc độ di chuyển
    public float gridSize = 1f;         // kích thước mỗi ô
    private bool isMoving = false;      // đang di chuyển hay không
    private Vector3 targetPos;          // vị trí đích

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;

            // Chỉ nhận phím khi bấm xuống (1 lần)
            if (Input.GetKeyDown(KeyCode.UpArrow)) input = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.DownArrow)) input = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) input = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow)) input = Vector2.right;

            if (input != Vector2.zero)
            {
                targetPos = transform.position + new Vector3(input.x, input.y, 0) * gridSize;
                StartCoroutine(MoveTo(targetPos));
            }
        }
    }

    private System.Collections.IEnumerator MoveTo(Vector3 target)
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
}
