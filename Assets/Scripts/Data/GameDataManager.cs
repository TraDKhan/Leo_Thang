using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public GameData currentData;

    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/save.json";
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"💾 Game saved at {savePath}");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("📂 Game data loaded.");
        }
        else
        {
            Debug.Log("🆕 No save found, creating new data...");
            currentData = new GameData();
            SaveGame();
        }
    }
    [ContextMenu("Reset Game Data")] 
    public void ResetGame()
    {
        // Xóa file save nếu có
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Old save file deleted.");
        }

        // Tạo dữ liệu mới
        currentData = new GameData();
        SaveGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("✅ Game data reset complete!");
    }
}
