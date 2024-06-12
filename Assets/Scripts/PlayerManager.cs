using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using JetBrains.Annotations;

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
    
        if (PlayerPrefs.GetInt("IsContinuing", 0) == 1)
        {
            LoadPlayerData(); // Load health if continuing the game or changing levels
        }
        else
        {
            ResetHealth(); // Reset health for a new game
            SavePlayerData(); // Initial save for a new game
        }
        PlayerPrefs.SetInt("IsContinuing", 1); // Ensure that further level changes are considered as continuing
        PlayerPrefs.Save();
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
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            SavePlayerData();
        }
    }


    void Die()
    {
        Debug.Log("Player died");
        GameOverScreen.Setup();
        ResetHealth(); // Reset health upon death
        SavePlayerData(); // Save the reset health
    }

    /// <summary>
    /// Saves the current level and PlayerHealth. If you call this directly after a Levelchange [SceneManager.LoadScene()], you need to pass the next level as a parameter.
    /// </summary>
    /// <param name="lvl">The name of the Unity Scene (Level) that should be loaded</param>
    public void SavePlayerData(string lvl = null)
    {
        
        Debug.Log("Saving player data");

        if (lvl == null)
        {
            Debug.Log("lvl is null, getting active scene name");
            lvl = SceneManager.GetActiveScene().name;
        }
        else
        {
            Debug.Log("lvl is not null, using: " + lvl);
        }

        Level level = new Level
        {
            level = lvl,
            lifePoints = currentHealth
        };
        //Debug.Log("Saving health: " + level.lifePoints);
        //Debug.Log("Saving level: " + level.level);
        //Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
        //Debug.Log("----------------------------------------------------");
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
            Debug.Log("Loaded level: " + loadedLevel.level);
        }
        else
        {
            currentHealth = maxHealth;
        }
        
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        
    }
}
