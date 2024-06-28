using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RagdollManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ragdollGameObjects;
    [SerializeField] public GameObject hipObj;
    [SerializeField] private GameObject leftArmTarget;
    [SerializeField] private GameObject rightArmTarget;
    [SerializeField] private GameObject leftLowerArmEnd;
    [SerializeField] private GameObject _rig;

    private GameObject _rigRightHand;
    private GameObject lockedObject;

    private bool triesToGrab = false;
    public bool hasKey = false;
    private float[] _previousJointSprings;
    private bool _isActiveRagdoll;
    private Rigidbody _hipRigid;
    private int _hitsTaken = 0;
    private ThirdPersonMovement thirdPersonMovement;
    
    
    private bool isAttached = false;
    private float raycastDistance = 1.0f;
    public LayerMask touchObjectLayerMask;



    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        _isActiveRagdoll = true;
        _previousJointSprings = new float[ragdollGameObjects.Length];
        _hipRigid = hipObj.GetComponent<Rigidbody>();
        thirdPersonMovement = hipObj.GetComponent<ThirdPersonMovement>();
        _rigRightHand = _rig.transform.Find("RightArm").gameObject;


    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            triesToGrab = true;
            
        }
        else
        {
            triesToGrab = false;
        }

        if (Input.GetKey(KeyCode.G))
        {
            if (hasKey)
            {
                ReleaseFromHand();
            }
        }

    }

    public void turnRagdoll()
    {
        //Debug.Log(ragdollGameObjects[0].gameObject.GetComponent<ConfigurableJoint>().slerpDrive.positionSpring);
        
        ReleaseFromHand();
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
    public void AttachHands(GameObject otherObj)
    {

        
        
        if (!isAttached)
        {
            isAttached = true;
            _rig.GetComponent<Rig>().weight = 1f;
            //Debug.Log("Testing");


            //SetHandPosition(otherObj, rightArmTarget.transform);
            //SetHandPosition(otherObj, leftArmTarget.transform);

            //leftLowerArmEnd.transform.SetParent(otherObj.transform, true);
            //otherObj.transform.SetParent(leftLowerArmEnd.transform);
            //otherObj.GetComponent<Rigidbody>().isKinematic = true;

            //FixedJoint grabJoint = leftLowerArmEnd.AddComponent<FixedJoint>();
            //grabJoint.connectedBody = otherObj.GetComponent<Rigidbody>();
            //grabJoint.anchor = leftLowerArmEnd.transform.position;


        }
        
        
    }

    public void DetachHands(GameObject otherObj)
    {
        
        if (isAttached)
        {
            isAttached = false;
            _rig.GetComponent<Rig>().weight = 0f;
        }


    }

    public void LockToHand(GameObject otherObj)
    {
        if (!hasKey)
        {
            if (triesToGrab)
            {
                PlayKeyPickup();
                hasKey = true;

                lockedObject = otherObj;
                otherObj.transform.SetParent(leftLowerArmEnd.transform);
                otherObj.transform.localPosition = Vector3.zero;
                //otherObj.transform.rotation = Quaternion.Euler(Vector3.zero);
                otherObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //otherObj.transform.localScale = Vector3.one;



                otherObj.GetComponent<Rigidbody>().isKinematic = true;
                otherObj.GetComponent<BoxCollider>().isTrigger = true;
               // _rigRightHand.GetComponent<TwoBoneIKConstraint>().weight = 0f;
               // _rig.GetComponent<Rig>().weight = 1f;

            }
        }
        
    }

    public void ReleaseFromHand()
    {
        if(hasKey)
        {
            hasKey = false;
            lockedObject.transform.SetParent(null);

            lockedObject.GetComponent<Rigidbody>().isKinematic = false;
            lockedObject.GetComponent<BoxCollider>().isTrigger = false;

            lockedObject.GetComponent<Rigidbody>().velocity = _hipRigid.velocity/2;
            if(lockedObject.GetComponent<Rigidbody>().velocity.y > 0f)
            {
                lockedObject.GetComponent<Rigidbody>().AddForce(hipObj.transform.forward * 4, ForceMode.Impulse);
                lockedObject.GetComponent<Rigidbody>().AddForce(hipObj.transform.up * 6, ForceMode.Impulse);
            }
            else
            {
                lockedObject.GetComponent<Rigidbody>().AddForce(hipObj.transform.forward * 6, ForceMode.Impulse);
                lockedObject.GetComponent<Rigidbody>().AddForce(hipObj.transform.up * 7, ForceMode.Impulse);
            }
            

            float random = Random.Range(-1f, 1f);

            lockedObject.GetComponent<Rigidbody>().AddTorque(new Vector3(random,random, random));

           // _rigRightHand.GetComponent<TwoBoneIKConstraint>().weight = 1f;
           // _rig.GetComponent<Rig>().weight = 0f;

        }




    }
    /*
    public void SetHandPosition(GameObject otherObj, Transform handTransform)
    {
        Ray ray = new Ray(handTransform.position, handTransform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, raycastDistance, touchObjectLayerMask))
        {
            handTransform.position = new Vector3(hit.point.x, hit.point.y + 1.5f, hit.point.z);
            //handTransform.rotation = Quaternion.LookRotation(hit.normal);
        }
    }
    */

    void PlayKeyPickup()
    {
        // Play the sound of the key being picked up
        //Debug.Log("Key picked up");
        AkSoundEngine.PostEvent("Play_PickUp_Item", gameObject);
    }



}
