using UnityEngine;
using UnityEngine.UI; // Only if you are using UI to display health

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField]
    public int damageAmount = 10; // Amount of health to decrease on collision
    public Text healthText; // Only if you are using UI to display health

    public GameOverScreen GameOverScreen;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount);
            Debug.Log("Ouch!");
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0 or above max
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    void Die()
    {
        // Handle player death (e.g., restart level, show game over screen, etc.)
        Debug.Log("Player died");
        GameOverScreen.Setup();
    }
}
