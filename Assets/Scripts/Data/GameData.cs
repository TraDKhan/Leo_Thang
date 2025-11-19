using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int totalGold;
    public List<int> unlockedLevels;

    // Hai danh sách song song thay thế Dictionary
    public List<int> highScoreKeys;
    public List<int> highScoreValues;

    // Không serialize Dictionary, chỉ dùng nội bộ
    [NonSerialized]
    public Dictionary<int, int> highScores = new Dictionary<int, int>();

    public GameData()
    {
        totalGold = 111;
        unlockedLevels = new List<int> { 0, 1, 2, 3 };

        highScores = new Dictionary<int, int>
        {
            { 0, 100 },
            { 1, 200 },
            { 2, 300 }
        };

        SyncFromDictionary();
    }

    // --- Đồng bộ hóa khi lưu ---
    public void SyncFromDictionary()
    {
        highScoreKeys = new List<int>(highScores.Keys);
        highScoreValues = new List<int>(highScores.Values);
    }

    // --- Đồng bộ hóa khi tải ---
    public void SyncToDictionary()
    {
        highScores = new Dictionary<int, int>();
        for (int i = 0; i < highScoreKeys.Count; i++)
        {
            highScores[highScoreKeys[i]] = highScoreValues[i];
        }
    }

    public void AddGold(int amount)
    {
        totalGold = Mathf.Max(0, totalGold + amount);
    }

    public void UnlockLevel(int levelId)
    {
        if (!unlockedLevels.Contains(levelId))
            unlockedLevels.Add(levelId);
    }
    public bool IsLevelUnlocked(int levelId)
    {
        return unlockedLevels.Contains(levelId);
    }
    public void UpdateHighScore(int levelId, int newScore)
    {
        if (!highScores.ContainsKey(levelId) || newScore > highScores[levelId])
            highScores[levelId] = newScore;
    }

    public int GetHighScore(int levelId)
    {
        if (highScores.TryGetValue(levelId, out int score))
            return score;
        return 0;
    }
}
