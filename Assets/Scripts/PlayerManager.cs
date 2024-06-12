using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 3;
    public int currentHealth;
    [SerializeField]
    public int damageAmount = 1;
    public Text healthText;
    public GameOverScreen GameOverScreen;

    void Start()
    {
        // Initialize the health correctly based on whether it's a new game or a continuation
        if (PlayerPrefs.GetInt("IsContinuing", 0) == 1)
        {
            LoadPlayerData(); // Load health if continuing the game
        }
        else
        {
            ResetHealth(); // Reset health for a new game
            SavePlayerData(); // Initial save for a new game
        }
        UpdateHealthUI();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount);
            Debug.Log("Ouch!");
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Die();
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            SavePlayerData();
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
        Debug.Log("Player died");
        GameOverScreen.Setup();
        ResetHealth(); // Reset health upon death
        SavePlayerData(); // Save the reset health
    }

    public void SavePlayerData()
    {
        Level level = new Level
        {
            level = SceneManager.GetActiveScene().name,
            lifePoints = currentHealth
        };

        string data = JsonUtility.ToJson(level);
        string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        Debug.Log("Saving to: " + filePath);
        File.WriteAllText(filePath, data);
        Debug.Log("Saved json data: " + data);
    }

    public void LoadPlayerData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Level loadedLevel = JsonUtility.FromJson<Level>(json);
            currentHealth = loadedLevel.lifePoints;
            Debug.Log("Loaded health: " + currentHealth);
        }
        else
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
