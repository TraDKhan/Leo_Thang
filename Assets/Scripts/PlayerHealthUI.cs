using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;

    [Header("Prefabs & Sprites")]
    public Image heartPrefab;       // Prefab của 1 hình trái tim (Image)
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Image[] heartImages;

    private void Start()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        BuildHearts();
        UpdateHearts();
    }
    void OnEnable()
    {

        // Đăng ký sự kiện
        playerHealth.onTakeDamage.AddListener(UpdateHearts);
        playerHealth.onHeal.AddListener(UpdateHearts);
        playerHealth.onDeath.AddListener(UpdateHearts);
    }

    void OnDisable()
    {
        if (playerHealth == null) return;
        playerHealth.onTakeDamage.RemoveListener(UpdateHearts);
        playerHealth.onHeal.RemoveListener(UpdateHearts);
        playerHealth.onDeath.RemoveListener(UpdateHearts);
    }

    private void BuildHearts()
    {
        // Xóa trái tim cũ
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // Tạo trái tim mới theo số máu tối đa
        heartImages = new Image[playerHealth.maxHealth];
        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            Image heart = Instantiate(heartPrefab, transform);
            heartImages[i] = heart;
        }
    }

    public void UpdateHearts()
    {
        if (heartImages == null || playerHealth == null) return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < playerHealth.currentHealth)
                heartImages[i].sprite = fullHeart;
            else
                heartImages[i].sprite = emptyHeart;      
        }
    }
}
