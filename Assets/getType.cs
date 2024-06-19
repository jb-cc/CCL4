using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getType : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Type:");
        Debug.Log(this.transform.parent.GetType().ToString());
        Debug.Log(this.transform.parent.GetType().ToString());
        Debug.Log(Type.GetType(this.transform.parent.GetType().ToString()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
