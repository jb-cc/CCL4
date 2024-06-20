using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxScript : MonoBehaviour
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
    private void Update()
    {
        distance = Vector3.Distance(_player.transform.position, transform.position);
        if (distance <= 3.5)
        {
            ragdollManager.AttachHands(gameObject);
        }
        else
        {
            ragdollManager.DetachHands(gameObject);

        }
    }

}
