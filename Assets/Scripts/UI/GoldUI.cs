using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI goldText;

    private int currentGold = -1; // để so sánh, tránh cập nhật liên tục

    void Start()
    {
        if (goldText == null)
            goldText = GetComponent<TextMeshProUGUI>();

        UpdateGoldUI(); // hiển thị ban đầu

        // Đăng ký callback tự cập nhật mỗi khi save (nếu muốn)
        InvokeRepeating(nameof(UpdateGoldUI), 0f, 0.2f); // cập nhật mỗi 0.2s
    }

    void UpdateGoldUI()
    {
        if (GameDataManager.Instance == null || GameDataManager.Instance.currentData == null)
            return;

        int gold = GameDataManager.Instance.currentData.totalGold;

        if (gold != currentGold) // chỉ cập nhật khi thay đổi
        {
            currentGold = gold;
            goldText.text = $"<sprite=0> {gold}";
        }
    }
}
