using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Level
{
    internal class LevelManager
    {
        void OnLevelComplete(int levelId, int score)
        {
            var data = GameDataManager.Instance.currentData;
            data.UnlockLevel(levelId + 1);     // Mở khóa màn kế tiếp
            data.UpdateHighScore(levelId, score); // Lưu điểm cao nhất
            GameDataManager.Instance.SaveGame();
        }

    }
}
