using UnityEngine;
using UnityEngine.UI;

// TODO: Rewrite the Healthbar
public class HealthBar : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField]
    private Slider slider;

    void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        SetMaxHealth(_gameManager.maxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        slider.value = _gameManager.playerData.playerHealth;
    }
    
    private void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
