using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels; // Gán các level trong Inspector

    private void Start()
    {
        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);

        // Vô hiệu hóa tất cả level
        foreach (GameObject lvl in levels)
            lvl.SetActive(false);

        // Bật level được chọn
        if (selectedLevel >= 0 && selectedLevel < levels.Length)
            levels[selectedLevel].SetActive(true);
        else
            Debug.LogWarning("SelectedLevel out of range!");
    }

    void OnLevelComplete(int levelId, int score)
    {
        var data = GameDataManager.Instance.currentData;
        data.UnlockLevel(levelId + 1);     // Mở khóa màn kế tiếp
        data.UpdateHighScore(levelId, score); // Lưu điểm cao nhất
        GameDataManager.Instance.SaveGame();
    }
}
