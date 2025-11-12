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
        if (currentData != null)
        {
            // 🔁 Đồng bộ Dictionary → List trước khi lưu
            currentData.SyncFromDictionary();

            string json = JsonUtility.ToJson(currentData, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"💾 Game saved at {savePath}");
        }
        else
        {
            Debug.LogWarning("⚠️ No game data to save!");
        }
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentData = JsonUtility.FromJson<GameData>(json);

            // 🔁 Đồng bộ List → Dictionary sau khi tải
            currentData.SyncToDictionary();

            Debug.Log("📂 Game data loaded.");
        }
        else
        {
            Debug.Log("🆕 No save found, creating new data...");
            currentData = new GameData();

            // 🔁 Đồng bộ trước khi lưu
            currentData.SyncFromDictionary();
            SaveGame();
        }
    }

    [ContextMenu("Reset Game Data")]
    public void ResetGame()
    {
        // 🗑️ Xóa file save nếu có
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Old save file deleted.");
        }

        // 🔄 Tạo dữ liệu mới
        currentData = new GameData();

        // 🔁 Đồng bộ trước khi lưu
        currentData.SyncFromDictionary();
        SaveGame();

        // 🔃 Tải lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("✅ Game data reset complete!");
    }
}
