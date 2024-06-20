using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BanjoAI : MonoBehaviour
{
    // Components
    private NavMeshAgent agent;
    [SerializeField]
    private Transform playerHip;
    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField]
    private ParticleSystem upFlare;
    [SerializeField]
    private ParticleSystem downFlare;
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private float shootForce = 200f;
    private Animator _animator;
    
    [SerializeField]
    private Vector3 walkPoint;
    [SerializeField]
    private float walkPointRange = 20f;
    private bool _isWalkPointSet;
    
    // Attacking
    [SerializeField]
    private float timeBetweenAttacks = 5f;
    private bool _alreadyAttacked;
    private Vector3 _attackPoint;
    
    
    // States
    [SerializeField]
    private float sightRange = 15f;
    private bool _isPlayerInSightRange;
    private float _standardSpeed;

    
    
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _standardSpeed = agent.speed;
        playerHip = GameObject.Find("Player").transform;
        playerHip = playerHip.transform.Find("Armature");
        playerHip = playerHip.transform.Find("Main");
        playerHip = playerHip.transform.Find("Hip");
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
            else
            {
                _animator.SetBool("isAttacking", false);
                _animator.SetBool("isIdle", true);
            }
        
        
    }
    
    
    private void Patroling()
    {
        // Visuals
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
   
        //Sound 
        PlayWalk();
        
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
        PlayAttack();

        // Visuals
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isAttacking", true);
        _animator.SetBool("isWalking", false);
        
        // Make the enemy stop moving and reset speed in case the player dies
        agent.SetDestination(transform.position);
        agent.speed = _standardSpeed;
        
        
        // Calculate direction to look at and look at the player
        Vector3 direction = (playerHip.position - transform.position).normalized;
        direction.y = 0; // Keep the y component zero to avoid tilting up/down
        transform.rotation = Quaternion.LookRotation(direction);
        
        
            
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
        _attackPoint = playerHip.transform.position;
        _alreadyAttacked = true;
        StartCoroutine(ShootDownFlare(_attackPoint, 0.5f));
        StartCoroutine(ShootFireball(_attackPoint, 1.5f));
    }
    
    
    IEnumerator ShootDownFlare(Vector3 attackPoint, float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 flareStart = attackPoint + Vector3.up * 20f;
        downFlare.transform.position = flareStart;
        downFlare.Play();
    }
    
    IEnumerator ShootFireball(Vector3 shootPosition, float time)
    {
        yield return new WaitForSeconds(time);
        shootPosition = shootPosition + Vector3.up * 20f;

        GameObject ball = Instantiate(fireballPrefab, shootPosition, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.down * shootForce, ForceMode.Impulse);
        
        StartCoroutine(DestroyFireball(ball, 1f));
        
        
    }
    
    IEnumerator DestroyFireball(GameObject ball, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(ball);
    }
    
   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void PlayWalk(){
        //AkSoundEngine.PostEvent("Play_Banjo_Walk", gameObject);
        Debug.Log("Playing Banjo walk sound");
    }

    void PlayAttack(){
        AkSoundEngine.PostEvent("Play_BanjoAttack", gameObject);
        Debug.Log("Playing Banjo attack sound");
    }

    void PlayDamage(){
        //AkSoundEngine.PostEvent("Play_Banjo_Death", gameObject);
        Debug.Log("Playing Banjo death sound");
    }

}
