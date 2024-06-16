using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceHipScript : MonoBehaviour
{
    
    public Transform body;
    void Start()
    {
        transform.position = body.position;
    }
}
