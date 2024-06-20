using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Random = UnityEngine.Random;

public class SmallBoiAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Transform playerHip;
    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;
    private Vector3 walkPoint;
    [SerializeField]
    private float walkPointRange = 20f;
    private bool _isWalkPointSet;
    [SerializeField]
    private float sightRange = 15f;
    private bool _isPlayerInSightRange;
    private float _standardSpeed;
    private float _standardAnimationSpeed = 2f;
    private Animator _animator;
    private GameManager _gameManager;
    private bool _alreadyAttacked = false;
    private bool _hasPlayedAttack = false;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _standardSpeed = agent.speed;
        _animator.speed = _standardAnimationSpeed;
        playerHip = GameObject.Find("Player").transform;
        playerHip = playerHip.transform.Find("Armature");
        playerHip = playerHip.transform.Find("Main");
        playerHip = playerHip.transform.Find("Hip");
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for sight range
        _isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        
        if (!_isPlayerInSightRange)
            Patroling();
        
        if (_isPlayerInSightRange)
        {
            if (!_alreadyAttacked)
            {
                ChasePlayer();
                if (!_hasPlayedAttack)
                {
                    PlayAttack();
                    _hasPlayedAttack = true;
                }
            }
            else
            {
                Patroling();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_alreadyAttacked) return;
            _gameManager.DecreasePlayerHealth(1);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 4f);
        }
    }

    private void Patroling()
    {
        agent.speed = _standardSpeed;
        _animator.speed = _standardAnimationSpeed;

        if (!_isWalkPointSet)
            SearchWalkPoint();
        
        if (_isWalkPointSet)
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            ForgetWalkPoint();

    }
    
    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            _isWalkPointSet = true;
    }
    
    private void ForgetWalkPoint()
    {
        _isWalkPointSet = false;
    }
    
    private void ChasePlayer()
    {
        // When chasing, the enemy should walk at double speed
        agent.speed = 2f * _standardSpeed;
        _animator.speed = 2f * _standardAnimationSpeed;
        agent.SetDestination(playerHip.position);
    }
    
    private void ResetAttack()
    {
        _alreadyAttacked = false;
        _hasPlayedAttack = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
    void PlayAttack()
    {
        // Play attack sound
        Debug.Log("Attack sound played");
        AkSoundEngine.PostEvent("Play_LittleAttack", gameObject);
    }
}