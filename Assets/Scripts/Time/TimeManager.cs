using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float currentTime = 0f;
    public bool isRunning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartTimer(float startValue = 0f)
    {
        currentTime = startValue;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void AddTime(float delta)
    {
        if (!isRunning) return;
        currentTime += delta;
    }

    public void ReduceTime(float delta)
    {
        if (!isRunning) return;
        currentTime -= delta;
        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
        }
    }
}
