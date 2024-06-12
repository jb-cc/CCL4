using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton; // Assign the Continue button in the Inspector

    void Start()
    {
        // Initially hide the Continue button
        continueButton.SetActive(false);

        // Check if there is saved data
        if (IsLevelSaved())
        {
            // Show the Continue button
            continueButton.SetActive(true);
        }

        // Add onClick listener for the Continue button
        continueButton.GetComponent<Button>().onClick.AddListener(ContinueGame);
    }

    bool IsLevelSaved()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        return File.Exists(filePath);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SceneHud");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ContinueGame()
    {
        // Load the saved level or handle the continue logic
        string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Level level = JsonUtility.FromJson<Level>(json);
            // Load the level using the saved data
            SceneManager.LoadScene(level.level);
        }
    }
}


