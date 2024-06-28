using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;
    private GameManager _gameManager;
    private RagdollManager _ragdollManager;
    
    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
        
        _ragdollManager = FindObjectOfType<RagdollManager>();
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            
            if (_ragdollManager.hasKey)
            {
                AkSoundEngine.PostEvent("Play_PortalEnter", gameObject);
                // Save player data with new level as current level, then load scene
                _gameManager.SavePlayerData(sceneToLoad);
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
