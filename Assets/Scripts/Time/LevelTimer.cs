using UnityEngine;

public enum LevelTimerMode
{
    CountUp,
    CountDown
}

public class LevelTimer : MonoBehaviour
{
    public LevelTimerMode mode;
    public float timeLimit = 60f;

    void Start()
    {
        // Khởi tạo timer ở TimeManager
        if (mode == LevelTimerMode.CountUp)
        {
            TimeManager.Instance.StartTimer(0);
        }
        else if (mode == LevelTimerMode.CountDown)
        {
            TimeManager.Instance.StartTimer(timeLimit);
        }
    }

    void Update()
    {
        if (!TimeManager.Instance.isRunning) return;

        // gửi dữ liệu cập nhật lên TimeManager
        if (mode == LevelTimerMode.CountUp)
        {
            TimeManager.Instance.AddTime(Time.deltaTime);
        }
        else if (mode == LevelTimerMode.CountDown)
        {
            TimeManager.Instance.ReduceTime(Time.deltaTime);

            // hết giờ → xử lý
            if (!TimeManager.Instance.isRunning)
                OnTimeOver();
        }
    }

    void OnTimeOver()
    {
        Debug.Log("Hết giờ! Màn thua!");
        PlayerHealth.Instance.Die();
    }
}
