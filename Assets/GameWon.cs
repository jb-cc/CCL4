using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWon : MonoBehaviour
{
    private GameManager _gameManager;
    
    public float speed = 1.0f;
    // Height of the oscillation
    public float height = 1.0f;
    public float rotationSpeed = 100.0f;


    // Original position of the GameObject
    private Vector3 originalPosition;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        originalPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * height;

        // Set the new position
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gameManager.gameWon = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("WinScene");
            Cursor.lockState = CursorLockMode.None;

        }
    }
}
