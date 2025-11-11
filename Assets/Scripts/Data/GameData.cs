using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int totalGold;
    public List<int> unlockedLevels;
    public Dictionary<int, int> highScores;

    public GameData()
    {
        totalGold = 0;
        unlockedLevels = new List<int> { 0, 1, 2, 3 };
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
