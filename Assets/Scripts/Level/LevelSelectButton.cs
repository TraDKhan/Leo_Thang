using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex; // Số thứ tự level (0, 1, 2, ...)
    public Image lockIcon; // Sprite hình khóa
    public Button button;  // Nút button thực tế

    private void Start()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 6);
        PlayerPrefs.Save();
        // Kiểm tra xem level này đã được mở khóa chưa
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 0);

        bool isUnlocked = levelIndex <= unlockedLevel;

        lockIcon.gameObject.SetActive(!isUnlocked); // Hiện khóa nếu chưa mở
        button.interactable = isUnlocked;           // Tắt bấm nếu chưa mở
        if (isUnlocked)
        {
            button.onClick.AddListener(() => OnSelectLevel());
        }
    }

    public void OnSelectLevel()
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }
}
