using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    public Image lockIcon;
    public Button button;

    private void Start()
    {
        if (GameDataManager.Instance?.currentData == null) return;

        var data = GameDataManager.Instance.currentData;
        bool isUnlocked = data.unlockedLevels.Contains(levelIndex);

        lockIcon.gameObject.SetActive(!isUnlocked);
        button.interactable = isUnlocked;

        if (isUnlocked)
            button.onClick.AddListener(() => OnSelectLevel());
    }

    public void OnSelectLevel()
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }
}
