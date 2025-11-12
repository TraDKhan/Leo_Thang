using UnityEngine;
using TMPro;
public class UIManager: MonoBehaviour
{
    public static UIManager Instance;

    [Header("WIN")]
    public GameObject resultPanel;
    public TextMeshProUGUI levelResultText;
    public TextMeshProUGUI levelTimeText;
    public TextMeshProUGUI levelScoreText;
    public TextMeshProUGUI levelBestScoreText;

    [Header("Level UI")]
    public TextMeshProUGUI levelSelected_Text;
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
        levelSelected_HighScore.text ="Best: " + highScore;
    }
    private void setActivePanel()
    {
        resultPanel.SetActive(false);
    }
    public void onResultLevel(string result)
    {
        levelResultText.text = result;
        levelTimeText.text ="Time: " + "00 : 00";
        levelScoreText.text ="Score: " + GameManager.Instance.getCurrentPoint().ToString();
        levelBestScoreText.text ="Best score: " + highScore;

        ////
        resultPanel.SetActive(true);
    }
}