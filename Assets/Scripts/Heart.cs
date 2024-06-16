using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private GameManager _gameManager;
    
    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ik habe dein hearz gebobben!");
            _gameManager.IncreasePlayerHealth(1);
            Destroy(gameObject);
        }
    }
}
