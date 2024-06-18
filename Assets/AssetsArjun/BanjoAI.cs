using UnityEngine;
using UnityEngine.AI;

public class BanjoAI : MonoBehaviour
{
    // Components
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;
    private Animator _animator;
    
    [SerializeField]
    private Vector3 walkPoint;
    [SerializeField]
    private float walkPointRange;
    private bool _isWalkPointSet;
    
    // Attacking
    [SerializeField]
    private float timeBetweenAttacks = 0.5f;
    private bool _alreadyAttacked;
    
    
    // States
    [SerializeField]
    private float sightRange;
    private bool _isPlayerInSightRange;
    private float _standardSpeed;

    
    
    
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _standardSpeed = agent.speed;
    }

    private void Update()
    {
        // Check for sight and attack range
        _isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        
        if (!_isPlayerInSightRange)
            Patroling();
        
        if (_isPlayerInSightRange)
            if (!_alreadyAttacked)
            {
                AttackPlayer();
            }
        
        
    }
    
    
    private void Patroling()
    {
        // Visuals
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
   
        
        
        // When patrolling, the enemy should walk at standard speed
        agent.speed = _standardSpeed;
        
        
        // Enemy selects a random point to walk to (patroling behaviour)
        if (!_isWalkPointSet) SearchWalkPoint();
        
        if (_isWalkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        
        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            // Visuals
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isIdle", true);
            
            // Idle for a while, then search for a new walk point
            Invoke(nameof(ForgetWalkPoint), 20f);
        }
    }
    
    private void SearchWalkPoint()
    {
        Debug.Log("Searching for walk point...");
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            _isWalkPointSet = true;
            Debug.Log("Walk point found: " + walkPoint);
        }
        else
        {
            Debug.Log("No walk point found.");
        }
    }
    
    private void ForgetWalkPoint()
    {
        _isWalkPointSet = false;
    }
    
    private void AttackPlayer()
    {
        // Attack only once
        _alreadyAttacked = true;

        // Visuals
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isAttacking", true);
        _animator.SetBool("isWalking", false);
        
        
        // Make the enemy stop moving and reset speed in case the player dies
        agent.SetDestination(transform.position);
        agent.speed = _standardSpeed;
        
        
        // Calculate direction to look at and look at the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep the y component zero to avoid tilting up/down
        transform.rotation = Quaternion.LookRotation(direction);
        
            
        // Visuals again
        // randomize punch animation
        _animator.SetBool("isAttacking", true);
        
            
        // add a delay between attacks
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
