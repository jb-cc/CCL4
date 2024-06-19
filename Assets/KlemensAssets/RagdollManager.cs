using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ragdollGameObjects;
    [SerializeField] private GameObject hipObj;

    private float[] _previousJointSprings;
    private bool _isActiveRagdoll;
    private Rigidbody _hipRigid;
    private int _hitsTaken = 0;
    private ThirdPersonMovement thirdPersonMovement;
    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        _isActiveRagdoll = true;
        _previousJointSprings = new float[ragdollGameObjects.Length];
        _hipRigid = hipObj.GetComponent<Rigidbody>();
        thirdPersonMovement = hipObj.GetComponent<ThirdPersonMovement>();

    }

    public void turnRagdoll()
    {
        //Debug.Log(ragdollGameObjects[0].gameObject.GetComponent<ConfigurableJoint>().slerpDrive.positionSpring);
        _hitsTaken++;
        if (_hitsTaken > 2)
        {
            if (_isActiveRagdoll)
            {
                for (int i = 0; i < ragdollGameObjects.Length; i++)
                {
                    //speicher prev in array
                    //bei anderer methode wieder einschalten
                    //aus array auslesen und werde wieder richtig setzen
                    ConfigurableJoint confJoint = ragdollGameObjects[i].GetComponent<ConfigurableJoint>();
                    _previousJointSprings[i] = confJoint.slerpDrive.positionSpring;

                    JointDrive jDrive = confJoint.slerpDrive;
                    jDrive.positionSpring = 0f;

                    confJoint.slerpDrive = jDrive;

                }
                //vllt doch lieber GameObj als Rigidbody damit man rotation wieder richtig setzen könnte
                _hipRigid.freezeRotation = false;
                _hitsTaken = 0;
                _isActiveRagdoll = false;
                thirdPersonMovement.LockMovement(true);
                StartCoroutine(waitFunction(3));
            }
        }
        
    }

    public void activateRagdoll()
    {
        for (int i=0;i<ragdollGameObjects.Length;i++)
        {
            ConfigurableJoint confJoint = ragdollGameObjects[i].GetComponent<ConfigurableJoint>();

            JointDrive jDrive = confJoint.slerpDrive;
            jDrive.positionSpring = _previousJointSprings[i];

            confJoint.slerpDrive = jDrive;

        }

        
        _isActiveRagdoll = true;
        thirdPersonMovement.LockMovement(false);
        _hitsTaken = 0;
        StartCoroutine(waitFreezeFunction(2));
    } 

    IEnumerator waitFunction(int secs)
    {
        yield return new WaitForSeconds(secs);
        activateRagdoll();
    }

    IEnumerator waitFreezeFunction(int secs)
    {
        yield return new WaitForSeconds(secs);
        hipObj.transform.rotation = Quaternion.Euler(12.136f, hipObj.transform.eulerAngles.y, 0f);
        _hipRigid.freezeRotation = true;
    }




}
