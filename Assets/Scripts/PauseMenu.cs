using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private GameManager _gameManager;

    void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
    }
    
    public void Continue()
    {
        _gameManager.ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        _gameManager.ResumeGame();
    }

    public void MainMenu()
    {
        _gameManager.ReturnToMainMenu();
    }
}
