using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gameManager.DecreasePlayerHealth(10000);
        }
    }
}
