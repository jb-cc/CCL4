using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVBotHit : MonoBehaviour
{
    private GameManager _gameManager;
    private SphereCollider _collider;
    private RagdollManager _ragdollManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (_collider == null)
        {
            _collider = GetComponent<SphereCollider>();
        }
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        if (_ragdollManager == null)
        {
            _ragdollManager = FindObjectOfType<RagdollManager>();
        }
    }
    

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            _gameManager.DecreasePlayerHealth(1);
            _ragdollManager.turnRagdoll();
        }
    }
}
