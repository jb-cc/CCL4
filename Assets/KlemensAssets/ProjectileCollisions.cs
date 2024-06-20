using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{
    private RagdollManager _ragdollManager;
    private GameManager _gameManager;
    
    void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();
        _ragdollManager = FindObjectOfType<RagdollManager>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _gameManager.DecreasePlayerHealth(1);
        }
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.name == "Head")
        {
            
            //RagdollManager
            
            _ragdollManager.turnRagdoll();
            
            
        }
    }
}
