using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private RagdollManager ragdollManager;
    private GameObject _player;
    private float distance;
    // Start is called before the first frame update
    void Awake()
    {
        ragdollManager = FindObjectOfType<RagdollManager>();
        _player = ragdollManager.hipObj;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(_player.transform.position, transform.position);
        if (distance <= 3)
        {
            ragdollManager.LockToHand(gameObject);
        }
    }
}
