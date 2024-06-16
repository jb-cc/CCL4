using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public int damageAmount = 1;
    private GameManager _gameManager;

    void Awake()
    {   
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _gameManager.DecreasePlayerHealth(damageAmount);
     
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            _gameManager.DecreasePlayerHealth(10000);
        }
    }
    
}
