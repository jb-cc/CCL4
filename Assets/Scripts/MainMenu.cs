using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private GameManager _gameManager;

    void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();

        continueButton.SetActive(false);
        _gameManager.AdjustContinueButton();

        continueButton.GetComponent<Button>().onClick.AddListener(ContinueGame);
    }


    
    
    public void PlayGame()
    {
        _gameManager.StartGame();
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
