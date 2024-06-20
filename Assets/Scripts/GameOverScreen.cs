using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private GameManager _gameManager;
    
    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void RestartGame(){
        playClickSound();
        Debug.Log("Restarting game...");
        _gameManager.StartGame();
    }

    public void MainMenu(){
        playClickSound();
        Debug.Log("Returning to main menu...");
        _gameManager.ReturnToMainMenu();
    }
    
    public void QuitGame(){
        playClickSound();
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void playClickSound()
    {
        _gameManager.playClickSound();
    }
}
