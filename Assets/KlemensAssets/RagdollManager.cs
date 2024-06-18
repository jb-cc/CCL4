using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ragdollGameObjects;
    [SerializeField] private Rigidbody hipObj;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void turnRagdoll()
    {
        Debug.Log(ragdollGameObjects[0].gameObject.GetComponent<ConfigurableJoint>().slerpDrive.positionSpring);
        
        foreach(var obj in ragdollGameObjects)
        {
            //speicher prev in array
            //bei anderer methode wieder einschalten
            //aus array auslesen und werde wieder richtig setzen
            ConfigurableJoint confJoint = obj.GetComponent<ConfigurableJoint>();

            JointDrive jDrive = confJoint.slerpDrive;
            jDrive.positionSpring = 0f;

            confJoint.slerpDrive = jDrive;

        }
        //vllt doch lieber GameObj als Rigidbody damit man rotation wieder richtig setzen könnte
        hipObj.freezeRotation = false;
    }
    


}
