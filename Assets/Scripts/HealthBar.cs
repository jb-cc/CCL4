using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerManager playerManager;
    public Image[] healthImgs; // 0-2 0left 1middle 2right   

    void Start()
    {
        if (playerManager == null)
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        int health = playerManager.currentHealth;

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
