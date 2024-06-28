using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.ScrollRect;

public class ThirdPersonMovement : MonoBehaviour
{
    //[SerializeField] private CharacterController characterController;

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed = 5f;
    //[SerializeField] private float jumpMultiplier 
    [SerializeField] private float gravity = -9.81f;
    Vector3 downVelocity;

    //[SerializeField] private float jumpingMaxHeight;
    //[SerializeField] private float fallFactor;
    
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    //private bool isOnGround = false;
    //private bool isJumping = false;

    private bool _lockMovement = false;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public Animator animator;
    public Transform cam;
    public Transform balanceObj;
    private Rigidbody _player;

    [SerializeField] Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    internal enum MovementType
    {
        TransformBased,
        PhysicsBased
    }

    [SerializeField] private ForceMode _forceMode;
    [SerializeField] private MovementType movementType;

    //Footsteps
     [SerializeField] private float footstepInterval = 0.5f; // Intervall zwischen Fußschritten
    private float footstepTimer;

    void Start()
    {
        _player = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        //Physics.gravity = new Vector3(0f, gravity, 0f);
        //StartCoroutine(FallControlFlow());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_lockMovement)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
                balanceObj.rotation = Quaternion.Euler(0, angle, 0);
                

                // Move the character using Rigidbody
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (movementType == MovementType.PhysicsBased)
                {
                    _player.AddForce(moveDirection.normalized * speed * Time.deltaTime, _forceMode);
                }
                //Physic based is way better for us... so basically artefact
                else if (movementType == MovementType.TransformBased)
                {
                    float strength = Vector3.Magnitude(moveDirection);
                    transform.Translate(new Vector3(0, 0, 1f) * speed * strength * Time.deltaTime);
                }

                animator.SetBool("isWalking", true);
                PlayFootstep();
            }
            else
            {
                
                animator.SetBool("isWalking", false);
            }

            if(Input.GetMouseButtonDown(0))
            {
                
                downVelocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
                animator.SetTrigger("punchTrigger");
            }


            if (Input.GetButtonDown("Jump") && isGrounded) 
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isWalking", false);

                Jump();

                //downVelocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
                //_player.AddForce(downVelocity * jumpSpeed, ForceMode.Impulse);
                //wrong _player.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange); wrong                
                //StartCoroutine(JumpControlFlow());



            }
            else if (isGrounded && !Input.GetButtonDown("Jump"))
            {
                animator.SetBool("isJumping", false);

            }
            if (!isGrounded)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isWalking", false);

            }

            /*
            if (!isGrounded)
            {
                downVelocity.y += gravity * Time.deltaTime;
                //_player.AddForce(downVelocity * Time.deltaTime, ForceMode.VelocityChange);
                animator.SetBool("isJumping", true);

                //characterController.Move(downVelocity * Time.deltaTime);
            }
            */

        }
        HandleGravity();

    }

    private void HandleGravity()
    {
        if (_player.velocity.y <= 0)
        {
            _player.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(_player.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _player.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }
    }
    private void Jump()
    {
        PlayJump();
        _player.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    /*
    private IEnumerator JumpControlFlow()
    {
        isJumping = true;
        float jumpHeight = transform.position.y + jumpingMaxHeight;
        //Debug.Log(jumpHeight);
        _player.AddForce((Vector3.up * jumpSpeed * jumpMultiplier) * Time.deltaTime, _forceMode);
        while (transform.position.y < jumpHeight)
        {
            //Debug.Log("Jonas " + transform.position.y);


            yield return null;


        }
        _player.AddForce((Vector3.up * jumpSpeed * jumpMultiplier * fallFactor * -1) * Time.deltaTime, _forceMode);
        isJumping = false;
    }
    private IEnumerator FallControlFlow()
    {
        //RaycastHit hit;
        //float prevY;
        float currentY = transform.position.y;
        while (true)
        {
            //bool raycastSuccess = Physics.Raycast(transform.position, transform.up * -1, out hit);
            //if (raycastSuccess && hit.collider.gameObject.CompareTag("Ground") && hit.distance <= 0.50001f)
            if(isGrounded)
            {
                isJumping = false;
                StopCoroutine(JumpControlFlow());
                isOnGround = true;
            }
            else
            {
                isJumping = true;

                isOnGround = false;
            }
            yield return null;

        }
    }
    */
    public void LockMovement(bool locking)
    {
        _lockMovement = locking;
    }

    void PlayJump()
    {
        AkSoundEngine.PostEvent("Play_Jump", gameObject);
    }

    void PlayFootstep()
    {
        if (footstepTimer <= 0f && isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance))
            {
                string surfaceType = "Default";
                if (hit.collider.CompareTag("Grass"))
                {
                    surfaceType = "Grass";
                }
                else if (hit.collider.CompareTag("Wood"))
                {
                    surfaceType = "Wood";
                }

                AkSoundEngine.SetSwitch("SurfaceType", surfaceType, gameObject);
                AkSoundEngine.PostEvent("Play_Footstep", gameObject);
            }

            footstepTimer = footstepInterval; // Timer zurücksetzen
        }
        else
        {
            footstepTimer -= Time.deltaTime;
        }
    }

    void PlayDeath()
    {
        Debug.Log("Death");
       // AkSoundEngine.PostEvent("Play_Death", gameObject);
    }

}
