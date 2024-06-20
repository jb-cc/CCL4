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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ich habe dein Herz gebobben!");
            

            // Wwise Event auslösen
            
            Debug.Log("ik habe dein hearz gebobben!");
            _gameManager.IncreasePlayerHealth(15);
            Destroy(gameObject);
            AkSoundEngine.PostEvent("Play_Heal", gameObject);
            //_gameManager.IncreasePlayerHealth(1);
            
        }
    }
}
