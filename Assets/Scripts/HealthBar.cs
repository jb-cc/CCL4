using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField]
    private Slider slider;
    public Gradient gradient;
    public Image fill;

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
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
    private void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }
}
