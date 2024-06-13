using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity = -9.81f;
    Vector3 downVelocity;


    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public Transform cam;
    private Rigidbody _player;

    [SerializeField] Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    [SerializeField] private ForceMode _forceMode;
    

    void Start()
    {
        _player = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0f, gravity, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (isGrounded && downVelocity.y<0)
        {
            //downVelocity.y = -2f;
        }

        Vector3 direction = new Vector3 (horizontal, 0f, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _player.AddForce(moveDirection.normalized * speed, _forceMode);
            //characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded) //&& isGrounded
        {
            downVelocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
            _player.AddForce(downVelocity * jumpSpeed, ForceMode.VelocityChange);
            //_player.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);

        }
        if (!isGrounded)
        {
            downVelocity.y += gravity * Time.deltaTime;
            _player.AddForce(downVelocity * Time.deltaTime, ForceMode.VelocityChange);
            
            //characterController.Move(downVelocity * Time.deltaTime);
        }


    }
}
