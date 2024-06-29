using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private int damage = 2;
    private GameManager _gameManager;
    private RagdollManager _ragdollManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        if (_ragdollManager == null)
        {
            _ragdollManager = FindObjectOfType<RagdollManager>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gameManager.DecreasePlayerHealth(damage);
            _ragdollManager.turnRagdoll();
        }
    }
}
