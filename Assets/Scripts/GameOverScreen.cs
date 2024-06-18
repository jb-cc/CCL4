using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private GameManager _gameManager;
    
    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void RestartGame(){
        Debug.Log("Restarting game...");
        _gameManager.StartGame();
    }

    public void MainMenu(){
        Debug.Log("Returning to main menu...");
        _gameManager.ReturnToMainMenu();
    }
    
    public void QuitGame(){
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
