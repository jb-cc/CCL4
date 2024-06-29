using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject unmuteButton;

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

    public void ToggleMute()
    {
        playToggle();
        _gameManager.muted = !_gameManager.muted;
        muteButton.SetActive(!_gameManager.muted);
        unmuteButton.SetActive(_gameManager.muted);
        Debug.Log("Muted: " + _gameManager.muted);
    }
    
    public void playToggle()
    {
        _gameManager.playToggle();
    }

    public void playClickSound()
    {
        _gameManager.playClickSound();
    }
}
