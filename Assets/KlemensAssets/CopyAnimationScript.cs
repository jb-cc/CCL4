using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyAnimationScript : MonoBehaviour
{
    public Transform target;
    private ConfigurableJoint confJoint;
    private Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        confJoint = GetComponent<ConfigurableJoint>();
        startingRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //confJoint.targetRotation = target.rotation;
        confJoint.SetTargetRotationLocal(target.localRotation, startingRotation);
    }
}
