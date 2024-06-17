using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// The GameManager is a Script that manages the game state, and main game logic
// There only exists one GameManager in the game, and it persists between scenes
// There needs to be a GameManager Object in the Main Menu Scene, with this script attached, nothing else
// Then add All the required things in the SerializeFields in the Unity Editor

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

    // ================== VARIABLES ==================
    
    // the player data object with all the player data. Can be read from other scripts, but only set from this script
    public PlayerData playerData { get; private set; }
    
    // the maximum health the player can have
    [SerializeField]
    public int maxHealth { get; private set; } = 10;
    
    // a flag to check if the game has been won, not used yet
    private bool _gameWon = false;
    
    // a flag to check if a save exists
    public bool saveExists {get; private set;}
    
    // the name of first level to be loaded, accessible from other scripts and in the Unity Editor
    public string firstLevel = "Game Manager Rewrite";

    // a flag to check if the game is paused
    private bool _pauseGame = false;
    
    // the game over scene to be loaded, not used yet
    // public string gameOverScene {get; private set;}
    
    // ================== UI ELEMENTS ==================
    // the UI elements that will be updated by the GameManager
    // have to be dragged in the Unity Editor
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField] 
    private HealthBar healthBar;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject continueButton;

    
    // ================== METHODS ==================
    private void Awake()
    {
        // Make sure the GameManager and UI Canvas persist between scenes
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(canvas.gameObject);
        
        // Load the player data into the playerData variable
        playerData = LoadPlayerData();
        
        // Check if the player data exists, if it does, set the saveExists variable to true
        // also make sure the player health is not greater than the max health (no cheating!)
        if (playerData != null)
        {
            saveExists = true;
            playerData.playerHealth = Math.Min(maxHealth, playerData.playerHealth);
        }
        else
        {
            // if there was no PlayerData to be loaded, create a new one with the default values (max health and first level)
            saveExists = false;
            playerData = new PlayerData();
            playerData.playerHealth = maxHealth;
            playerData.level = firstLevel;
        }
    }

    void Update()
    {
        if (!mainMenu.gameObject.activeSelf){
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Esc key was pressed");
                if (_pauseGame)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    // This method saves the player data to a JSON file
    // It is called on StartGame and when a new Scene is loaded (see SceneLoader.cs)
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
    
    
    // This method loads the player data from a JSON file
    // It is called on Awake (to check if a save exists) and when the game is continued
    private PlayerData LoadPlayerData()
    {
        string filePath = Application.persistentDataPath + "/playerData.json";
        Debug.Log(filePath);
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
        healthBar.UpdateHealthBar();
        if (playerData.playerHealth <= 0)
        {
            GameOver();
        }
    }
    
    public void IncreasePlayerHealth(int amount)
    {
        playerData.playerHealth += amount;
        playerData.playerHealth = Math.Min(maxHealth, playerData.playerHealth);
        healthBar.UpdateHealthBar();
    }

    public void StartGame()
    {
        // Set the player data to the first level and max health, then save it
        playerData = new PlayerData();
        playerData.level = firstLevel;
        playerData.playerHealth = maxHealth;
        SavePlayerData();
        
        // Update the active UI Elements
        healthBar.gameObject.SetActive(true);
        gameOverScreen.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        healthBar.UpdateHealthBar();
        
        // Load the first level
        Time.timeScale = 1;
        SceneManager.LoadScene(firstLevel);
    }
    
    public void ContinueGame()
    {
        // Load the level and health stored in the player data
        playerData = LoadPlayerData();
        
        // Update the active UI Elements
        healthBar.gameObject.SetActive(true);
        gameOverScreen.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        healthBar.UpdateHealthBar();
        
        // Load the level
        Time.timeScale = 1;
        SceneManager.LoadScene(playerData.level);
    }
    
    // called when the playerHealth reaches 0
    public void GameOver()
    {
        // Update the active UI Elements
        Time.timeScale = 0;
        gameOverScreen.gameObject.SetActive(true);
        healthBar.gameObject.SetActive(false);
        
        // in case we have a game over scene, load it
        // SceneManager.LoadScene(gameOverScene);
    }

    
    // called on clicking the main menu button in the game over screen
    public void ReturnToMainMenu()
    {
        // Update the active UI Elements
        gameOverScreen.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        AdjustContinueButton();
    }
    
    // called every time the main menu is activated
    public void AdjustContinueButton()
    {
        if (saveExists)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        _pauseGame = true;
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _pauseGame = false;
    }
}
