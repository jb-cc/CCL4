using System.Collections;
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
    [SerializeField]
    private ParticleSystem upFlare;
    [SerializeField]
    private ParticleSystem downFlare;
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private float shootForce;
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
    private Vector3 _attackPoint;
    
    
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
        //Debug.Log("Searching for walk point...");
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            _isWalkPointSet = true;
            //Debug.Log("Walk point found: " + walkPoint);
        }
        else
        {
            //Debug.Log("No walk point found.");
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
        _animator.SetBool("isAttacking", true);
        
        
            
        // add a delay between attacks
        
        Invoke(nameof(ResetAttack), timeBetweenAttacks);

    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
    
    
    public void ShootUpFlare()
    {
        upFlare.Play();
        _attackPoint = player.transform.position;
        
    }
    
    public void ShootDownFlare()
    {
        Vector3 flareStart = _attackPoint + Vector3.up * 25f;
        Debug.Log("player position: " + player.position);
        Debug.Log("Attack point: " + _attackPoint);
        Debug.Log("Flare start: " + flareStart);
        Debug.Log("Down flare position: " + downFlare.transform.position);
        
        downFlare.transform.position = flareStart;
        downFlare.Play();
        StartCoroutine(ShootFireball(0.5f));
    }
    
    void ShootFireball()
    {
        
    }
    
    
    IEnumerator DestroyFireball(GameObject ball, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(ball);
    }
    
    IEnumerator ShootFireball(float time)
    {
        // Calculate the starting position 5 units above the player
        Vector3 shootPosition = player.position + Vector3.up * 20f;

        // Instantiate the ball at the shoot position
        GameObject ball = Instantiate(fireballPrefab, shootPosition, Quaternion.identity);

        // Get the Rigidbody component of the ball
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        // Apply a downward force to the ball
        rb.AddForce(Vector3.down * shootForce, ForceMode.Impulse);
        
        StartCoroutine(DestroyFireball(ball, 1f));
        yield return new WaitForSeconds(time);
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
