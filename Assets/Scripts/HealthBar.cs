using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerManager playerManager;
    public Image[] healthImgs; //0-2 0left 1middle 2right   
    public int health;

    private int playerHealth
    {
        get
        {
            return playerManager.currentHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       if (playerManager == null)
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        health = playerHealth;

        switch(health)
        {
            case 3:
                foreach (Image img in healthImgs)
                {
                    img.enabled = true;
                }
                break;
            case 2:
                healthImgs[2].enabled = false;
                break;
            case 1:
                healthImgs[1].enabled = false;
                break;
            case 0: 
                healthImgs[0].enabled = false;
                Debug.Log("Player died all Img Gone");
                break;
        }

    }
}
