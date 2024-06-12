using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        continueButton.SetActive(false);

        if (IsLevelSaved())
        {
            continueButton.SetActive(true);
        }

        continueButton.GetComponent<Button>().onClick.AddListener(ContinueGame);
    }

    bool IsLevelSaved()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                Level level = JsonUtility.FromJson<Level>(json);
                if (!string.IsNullOrEmpty(level.level))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("IsContinuing", 0); // Set flag to indicate new game
        PlayerPrefs.Save();
        if (playerManager != null)
        {
            playerManager.ResetHealth();
            playerManager.SavePlayerData(null);
        }
        //Change to actual scene in game
        SceneManager.LoadScene("SceneHud");
        playerManager.SavePlayerData(null);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("IsContinuing", 1); // Set flag to indicate continuing game
        PlayerPrefs.Save();

        string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Level level = JsonUtility.FromJson<Level>(json);
            SceneManager.LoadScene(level.level);
        }
    }
}
