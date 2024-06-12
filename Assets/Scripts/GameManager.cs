using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
    public string level;
    public int lifePoints;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Level level = new Level();
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            

            

            SceneManager.LoadScene(level.level);

            if (playerManager != null)
            {
                playerManager.SavePlayerData(level.level);
            }
        }
    }

    
}
