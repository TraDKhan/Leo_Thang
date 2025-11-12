using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onHeal;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        onTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return; // không hồi máu nếu đã chết

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}! HP: {currentHealth}/{maxHealth}");
        onHeal?.Invoke();
    }

    private void Die()
    {
        Debug.Log("💀 Player is dead! GAME OVER!");

        UIManager.Instance.onResultLevel("You Lost");
        ButtonUIManager.Instance.UnLockLevelButton();
        onDeath?.Invoke();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
