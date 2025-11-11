using UnityEngine;
using TMPro;
public class UIManager: MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject winPanel;

    [Header("Level UI")]
    public TextMeshProUGUI levelSelected_Text;
    public TextMeshProUGUI levelSelected_HighScore;

    private void Start()
    {
        if (Instance == null) Instance = this;

        int levelSelect = PlayerPrefs.GetInt("SelectedLevel") + 1;
        levelSelected_Text.text = $"LEVEL { levelSelect.ToString()}";
        levelSelected_HighScore.text = GameDataManager.Instance.currentData.GetHighScore(PlayerPrefs.GetInt("SelectedLevel")).ToString();

        setActivePanel();
    }
    private void setActivePanel()
    {
        winPanel.SetActive(false);
    }
    public void onWinLevel()
    {
        winPanel.SetActive(true);
    }
}