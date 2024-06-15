using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using JetBrains.Annotations;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public int damageAmount = 1;
    private GameManager _gameManager;
    [SerializeField]
    private HealthBar healthBar;

    void Start()
    {   if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
        if (healthBar == null)
            healthBar = FindObjectOfType<HealthBar>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _gameManager.DecreasePlayerHealth(damageAmount);
            
            Debug.Log("Ouch!");
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            healthBar.UpdateHealthBar();
            _gameManager.DecreasePlayerHealth(10000);
        }
    }
    
}
