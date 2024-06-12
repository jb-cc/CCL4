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
            SaveToJson();

            SceneManager.LoadScene(level.level);
        }
    }

    public void SaveToJson()
    {
        if (playerManager != null)
        {
            playerManager.SavePlayerData();
        }
    }
}
