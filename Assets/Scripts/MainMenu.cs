using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        continueButton.SetActive(false);

        if (_gameManager.saveExists)
        {
            continueButton.SetActive(true);
        }

        continueButton.GetComponent<Button>().onClick.AddListener(ContinueGame);
    }


    public void PlayGame()
    {
        //Change to actual scene in game
        _gameManager.SavePlayerData(_gameManager.firstLevel);
        SceneManager.LoadScene(_gameManager.firstLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        _gameManager.ContinueGame();
    }
}
