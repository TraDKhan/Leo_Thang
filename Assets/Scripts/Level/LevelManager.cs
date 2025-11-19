using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public GameObject[] levels;
    private int currentLevel;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("SelectedLevel", 0);

        // Tắt tất cả level
        foreach (GameObject lvl in levels)
            lvl.SetActive(false);

        // Bật level được chọn
        if (currentLevel >= 0 && currentLevel < levels.Length)
            levels[currentLevel].SetActive(true);
        else
            Debug.LogWarning("⚠️ SelectedLevel out of range!");

        // 🔥 Cập nhật lại Tilemap cho nhân vật
        var player = FindAnyObjectByType<GridMovementTilemap>();
        if (player != null)
            player.RefreshTilemap();
    }

    public void OnLevelComplete(int levelId, int score)
    {
        var data = GameDataManager.Instance.currentData;
        data.UnlockLevel(levelId + 1);     // Mở khóa màn kế tiếp
        data.UpdateHighScore(levelId, score); // Lưu điểm cao nhất
        GameDataManager.Instance.SaveGame();
    }

    public void OnNextLevel()
    {
        if (currentLevel + 1 < levels.Length)
        {
            PlayerPrefs.SetInt("SelectedLevel", currentLevel + 1);
            PlayerPrefs.Save();

            // Load lại scene hiện tại để bật level mới
            GameManager.Instance.setCurrentPoint(0);
            UIManager.Instance.updateScoreUI();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Nếu đã hết level → quay lại menu
            SceneManager.LoadScene("LevelSelectScene");
        }
    }
}
