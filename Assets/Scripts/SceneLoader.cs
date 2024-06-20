using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;
    private GameManager _gameManager;
    
    private void Start()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AkSoundEngine.PostEvent("Play_PortalEnter", gameObject);
            // Save player data with new level as current level, then load scene
            _gameManager.SavePlayerData(sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
