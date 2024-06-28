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
        playClickSound();
        _gameManager.ResumeGame();
    }

    public void QuitGame()
    {
        playClickSound();
        Application.Quit();
        _gameManager.ResumeGame();
    }

    public void MainMenu()
    {
        playClickSound();
        _gameManager.ReturnToMainMenu();
    }


    public void playClickSound()
    {
        _gameManager.playClickSound();
    }
}
