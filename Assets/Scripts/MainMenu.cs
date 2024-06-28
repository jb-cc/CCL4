using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject unmuteButton;
    private GameManager _gameManager;

    void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        _gameManager.AdjustContinueButton();
    }


    public void ContinueGame()
    {
        playClickSound();
        _gameManager.ContinueGame();
    }
    
    public void StartGame()
    {
        playClickSound();
        Debug.Log("Starting game...");
        _gameManager.StartGame();
    }

    public void Quit()
    {
        playClickSound();
        Application.Quit();
    }

    public void ToggleMute()
    {
        playToggle();
        _gameManager.muted = !_gameManager.muted;
        muteButton.SetActive(!_gameManager.muted);
        unmuteButton.SetActive(_gameManager.muted);
        Debug.Log("Muted: " + _gameManager.muted);
    }
    
    public void playClickSound()
    {
        _gameManager.playClickSound();
    }

    public void playToggle()
    {
        _gameManager.playToggle();
    }

}
