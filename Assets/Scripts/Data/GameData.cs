using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int totalGold; // Tổng số vàng người chơi có
    public List<int> unlockedLevels; // Danh sách ID hoặc index của các màn đã mở khóa
    public Dictionary<int, int> highScores; // Lưu điểm cao nhất theo từng màn (key = levelId)

    public GameData()
    {
        totalGold = 0;
        unlockedLevels = new List<int> { 1 }; // Mặc định mở khóa màn 1
        highScores = new Dictionary<int, int>();
    }

    public void AddGold(int amount)
    {
        totalGold += amount;
        if (totalGold < 0) totalGold = 0;
    }

    public void UnlockLevel(int levelId)
    {
        if (!unlockedLevels.Contains(levelId))
            unlockedLevels.Add(levelId);
    }

    public void UpdateHighScore(int levelId, int newScore)
    {
        if (!highScores.ContainsKey(levelId) || newScore > highScores[levelId])
            highScores[levelId] = newScore;
    }

    public int GetHighScore(int levelId)
    {
        if (highScores.ContainsKey(levelId))
            return highScores[levelId];
        return 0;
    }
}
