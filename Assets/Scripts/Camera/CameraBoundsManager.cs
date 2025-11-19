using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class CameraBoundsManager : MonoBehaviour
{
    public CinemachineCamera virtualCamera;

    private string boundsTag = "CameraBound"; // Tag dùng để đánh dấu vùng giới hạn
    private CinemachineConfiner2D confiner;

    void Awake()
    {
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        UpdateCameraBounds();
    }

    public void UpdateCameraBounds()
    {
        // Tìm object có tag và đang active
        GameObject activeBounds = GameObject.FindGameObjectsWithTag(boundsTag)
            .FirstOrDefault(obj => obj.activeInHierarchy);

        if (activeBounds != null)
        {
            Collider2D col = activeBounds.GetComponent<Collider2D>();
            if (col != null)
            {
                confiner.BoundingShape2D = col;
                confiner.InvalidateBoundingShapeCache();
                Debug.Log("Camera bounds updated to: " + activeBounds.name);
            }
            else
            {
                Debug.LogWarning("Active bounds object has no Collider2D.");
            }
        }
        else
        {
            Debug.LogWarning("No active bounds object with tag '" + boundsTag + "' found.");
        }
    }
}