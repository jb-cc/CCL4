using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{
    private RagdollManager _ragdollManager;
    
    void Awake()
    {
        _ragdollManager = FindObjectOfType<RagdollManager>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.name == "Head")
        {
            
            //RagdollManager
            
            _ragdollManager.turnRagdoll();
            
            
        }
    }
}
