using UnityEngine;
using TMPro;
public class UIManager: MonoBehaviour
{
    public static UIManager Instance;

    [Header("Result")]
    public GameObject resultPanel;
    public TextMeshProUGUI levelResultText;
    public TextMeshProUGUI levelTimeText;
    public TextMeshProUGUI levelScoreText;
    public TextMeshProUGUI levelBestScoreText;

    [Header("Level UI")]
    public TextMeshProUGUI levelSelected_Text;
    public TextMeshProUGUI level_CurrentScore;
    public TextMeshProUGUI levelSelected_HighScore;

    private string highScore = "";

    private void Start()
    {
        if (Instance == null) Instance = this;
        setActivePanel();

        int levelSelect = PlayerPrefs.GetInt("SelectedLevel") + 1;
        levelSelected_Text.text = $"LEVEL { levelSelect.ToString()}";

        ////
        highScore = GameDataManager.Instance.currentData.GetHighScore(PlayerPrefs.GetInt("SelectedLevel")).ToString();
        level_CurrentScore.text = "Score: " + GameManager.Instance.getCurrentPoint();
        levelSelected_HighScore.text ="Best: " + highScore;
        updateScoreUI();
    }
    private void setActivePanel()
    {
        resultPanel.SetActive(false);
    }
    public void onResultLevel(string result)
    {
        float finalTime = TimeManager.Instance.currentTime;
        string formattedTime = FormatTime(finalTime);

        levelResultText.text = result;
        levelTimeText.text = "Time: " + formattedTime;
        levelScoreText.text = "Score: " + GameManager.Instance.getCurrentPoint().ToString();
        levelBestScoreText.text = "Best score: " + highScore;

        resultPanel.SetActive(true);
    }

    public void updateScoreUI()
    {
        level_CurrentScore.text ="Score: " + GameManager.Instance.getCurrentPoint().ToString();
    }
    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}