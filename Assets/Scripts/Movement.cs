using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Movement : MonoBehaviour
{
    
    PlayerInput playerInput;
    InputAction moveAction;
    
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private ParticleSystem flare;
    // Start is called before the first frame update
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc key was pressed");
            flare.Play();
        }
    }


    void MovePlayer()
    {
        Vector2 moveDirection = moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 finalMoveDirection = (forward * moveDirection.y + right * moveDirection.x).normalized;
        
        transform.position += finalMoveDirection * moveSpeed * Time.deltaTime;
    }
}
