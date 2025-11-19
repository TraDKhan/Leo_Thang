using UnityEngine;
using TMPro;

public class LevelTimeUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Update()
    {
        float time = TimeManager.Instance.currentTime;

        int m = Mathf.FloorToInt(time / 60);
        int s = Mathf.FloorToInt(time % 60);

        timerText.text = $"{m:00}:{s:00}";
    }
}
