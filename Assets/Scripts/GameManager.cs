using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentPoint = 0;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        //GameDataManager.Instance.SaveGame();
        SceneManager.LoadScene("MenuScene");   
    }

    public void onNextLevel()
    {
        LevelManager.Instance.OnNextLevel();
    }

    public void setCurrentPoint(int point)
    {
        currentPoint += point;
        Debug.Log(currentPoint);
    }

    public int getCurrentPoint()
    {
        return currentPoint;
    }
}