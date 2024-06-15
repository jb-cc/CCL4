using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // player data class, this is how the saved JSON data will be structured
    public class PlayerData
    {
        // level is the name of the scene
        public string level;
        public int playerHealth;
        // public Vector3 playerPosition;
    }

    // variables
    public PlayerData playerData { get; private set; }
    private int _playerHealth;
    // private Vector3 _playerPosition;
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private bool _gameWon = false;
    private bool _gameOver = false;
    public bool saveExists {get; private set;}
    public string firstLevel {get; private set;}
    public string gameOverScene {get; private set;}
    [SerializeField]
    private GameObject gameOverScreen;


    private void Awake()
    {
        
        playerData = LoadPlayerData();
        if (playerData != null)
        {
            saveExists = true;
            playerData.playerHealth = Math.Min(maxHealth, playerData.playerHealth);
        }
        else
        {
            saveExists = false;
            playerData = new PlayerData();
            playerData.playerHealth = maxHealth;
            playerData.level = firstLevel;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayerData(string lvl = null)
    {
        // if lvl is null, use the one stored in _level
        if (lvl != null)
        {
            playerData.level = lvl;
        };
        
        string json = JsonUtility.ToJson(playerData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
        saveExists = true;
    }
    
    private PlayerData LoadPlayerData()
    {
        string filePath = Application.persistentDataPath + "/playerData.json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return null;
    }
    
    public void DecreasePlayerHealth(int amount)
    {
        playerData.playerHealth -= amount;
        playerData.playerHealth = Math.Max(0, playerData.playerHealth);
        if (playerData.playerHealth <= 0)
        {
            GameOver();
        }
    }
    
    public void ContinueGame()
    {
        playerData = LoadPlayerData();
        SceneManager.LoadScene(playerData.level);
    }
    
    public void GameOver()
    {
        _gameOver = true;
        gameOverScreen.SetActive(true);
        // SceneManager.LoadScene(gameOverScene);
    }
}
