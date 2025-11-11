using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonUIManager: MonoBehaviour
{
    public static ButtonUIManager Instance;

    [Header("Buttons UI")]
    public Button pauseButton;
    public Button settingButton;

    [Header("Buttons Game Control")]
    public Button restartButton;
    public Button resumeButton;
    public Button quitButton;

    [Header("Win Buttons")]
    public Button restart;
    public Button next;
    public Button menu;

    [Header("Panels UI")]
    public GameObject pausePanel;
    public GameObject settingPanel;

    private void Start()
    {
        Instance = this;
        SetActivePanel();

        ToggleButtons();
        onButtons();
    }

    private void SetActivePanel() 
    { 
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    private void ToggleButtons()
    {
        pauseButton.onClick.AddListener (() => TogglePausePanel(pauseButton, pausePanel));
        settingButton.onClick.AddListener(() => ToggleSettingPanel(settingButton, settingPanel));
    }

    private void onButtons()
    {
        restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
        resumeButton.onClick.AddListener(() => onResume());
        quitButton.onClick.AddListener(() => GameManager.Instance.Quit());

        //win button
        restart.onClick.AddListener(() => GameManager.Instance.Restart());
        next.onClick.AddListener(() => GameManager.Instance.onNextLevel());
        menu.onClick.AddListener(() => GameManager.Instance.Quit());
    }
    private void TogglePausePanel(Button button, GameObject panel)
    {
        if (button != null && panel != null)
        {
            bool isActive = !panel.activeSelf;
            panel.SetActive(isActive);

            if (isActive)
            {
                Time.timeScale = 0f;
                settingPanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    private void ToggleSettingPanel(Button button, GameObject panel)
    {
        if (button != null && panel != null)
        {
            bool isActive = !panel.activeSelf;
            panel.SetActive(isActive);

            if (isActive)
            {
                Time.timeScale = 0f;
                pausePanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    private void onResume()
    {
        Time.timeScale = 1f;
        SetActivePanel();
    }
}