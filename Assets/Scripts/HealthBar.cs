using UnityEngine;
using UnityEngine.UI;

// TODO: Rewrite the Healthbar
public class HealthBar : MonoBehaviour
{
    public Image[] healthImgs; // 0-2 0left 1middle 2right   
    private GameManager _gameManager;

    void Start()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        UpdateHealthBar();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
    }

    public void UpdateHealthBar()
    {
        int health = _gameManager.playerData.playerHealth;
        for (int i = 0; i < healthImgs.Length; i++)
        {
            healthImgs[i].enabled = i < health;
        }

        if (health == 0)
        {
            Debug.Log("Player died all Img Gone");
        }
    }
}
