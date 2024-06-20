using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Components
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField] private Transform player;

    
    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;
    private Animator _animator;
    public bool endOfAttackAnimation = true;
    
    // Visuals
    private GameObject _angryFace;
    private GameObject _neutralFace;
    private GameObject _deadFace;
    private int _chosenAttackAnimation = 1;

    // Patroling
    [SerializeField]
    private Vector3 walkPoint;
    [SerializeField]
    private float walkPointRange;
    private bool _isWalkPointSet;

    // Attacking
    [SerializeField]
    private float timeBetweenAttacks = 0f;
    private bool _alreadyAttacked;
    [SerializeField] private GameObject leftHand, leftForeArm, rightHand, rightForeArm;
    private CapsuleCollider _leftHandCollider, _leftForeArmCollider, _rightHandCollider, _rightForeArmCollider;
    
    // States
    [SerializeField]
    private float sightRange, attackRange;
    private bool _isPlayerInSightRange, _isPlayerInAttackRange;
    private float _standardSpeed;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        player = player.transform.Find("Armature");
        player = player.transform.Find("Main");
        player = player.transform.Find("Hip");
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _standardSpeed = agent.speed;
        _angryFace = gameObject.transform.Find("FaceAngry").gameObject;
        _neutralFace = gameObject.transform.Find("FaceNeutral").gameObject;
        _deadFace = gameObject.transform.Find("FaceDead").gameObject;
        _leftHandCollider = leftHand.GetComponent<CapsuleCollider>();
        _leftForeArmCollider = leftForeArm.GetComponent<CapsuleCollider>();
        _rightHandCollider = rightHand.GetComponent<CapsuleCollider>();
        _rightForeArmCollider = rightForeArm.GetComponent<CapsuleCollider>();
        DeactivateWeaponColliders();
        StartCoroutine(ForgetWalkPointLoop());
        StartCoroutine(RandomizeAttackAnimation());
    }

    private void Update()
    {
        // Check for sight and attack range
        _isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        bool _isPlayerInSmallAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        bool _isPlayerInBigAttackRange = Physics.CheckSphere(transform.position, attackRange+ 0.2f, whatIsPlayer);
        _isPlayerInAttackRange = _isPlayerInSmallAttackRange | _isPlayerInBigAttackRange;
        
        if (!_isPlayerInSightRange && !_isPlayerInAttackRange)
            if (endOfAttackAnimation)
                Patroling();
        
        if (_isPlayerInSightRange && !_isPlayerInAttackRange)
            if (endOfAttackAnimation)
                ChasePlayer();
        
        if (_isPlayerInSightRange && _isPlayerInAttackRange)
                AttackPlayer();
            
    }

    private void Patroling()
    {
        // Visuals
        _animator.SetBool("isChasing", false);
        _animator.SetBool("isStanding", false);
        _animator.SetBool("isWalking", true);
        _angryFace.SetActive(false);
        _neutralFace.SetActive(true);
        _deadFace.SetActive(false);
        
        
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
            _animator.SetBool("isStanding", true);
            _animator.SetInteger("chosenStandingAnimation", Random.Range(1, 3));            
            
            // Idle for a while, then search for a new walk point
            StartCoroutine(ForgetWalkPoint(10));
        }
    }

    private void SearchWalkPoint()
    {
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
    
    private IEnumerator ForgetWalkPoint(float time)
    {
        yield return new WaitForSeconds(time);
        _isWalkPointSet = false;
    }

    private IEnumerator ForgetWalkPointLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            _isWalkPointSet = false;
        }
    }

    private void ChasePlayer()
    {
        // Visuals
        _animator.SetBool("isStanding", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isChasing", true);
        _angryFace.SetActive(true);
        _neutralFace.SetActive(false);
        _deadFace.SetActive(false);
        
        // When chasing, the enemy should walk at double speed
        agent.speed = 2.5f * _standardSpeed;
        
        
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (!endOfAttackAnimation) return;
        // Attack only once
        _alreadyAttacked = true;

        // Visuals
        _animator.SetBool("isStanding", false);
        _animator.SetBool("isChasing", false);
        _animator.SetBool("isWalking", false);
        _animator.SetInteger("chosenAttackAnimation", _chosenAttackAnimation);
        _animator.SetBool("isAttacking", true);
        _angryFace.SetActive(true);
        _neutralFace.SetActive(false);
        _deadFace.SetActive(false);
        
        
        // Make the enemy stop moving and reset speed in case the player dies
        agent.SetDestination(transform.position);
        agent.speed = _standardSpeed;
        
        
        // Calculate direction to look at and look at the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep the y component zero to avoid tilting up/down
        transform.rotation = Quaternion.LookRotation(direction);
        
            
        // Visuals again
        // randomize punch animation
        
        
            
        // add a delay between attacks
        StartCoroutine(ResetAttack());
    }
    
    public void EndOfAttackAnimation()
    {
        endOfAttackAnimation = true;
    }
    
    public void StartOfAttackAnimation()
    {
        endOfAttackAnimation = false;
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        _alreadyAttacked = false;
        _animator.SetBool("isAttacking", false);
    }
    
    private IEnumerator RandomizeAttackAnimation()
    {
        yield return new WaitForSeconds(2f);
        _chosenAttackAnimation = Random.Range(1, 3);
    }

    public void ActivateWeaponCollidersLeft()
    {
        _leftHandCollider.enabled = true;
        _leftForeArmCollider.enabled = true;
        _rightHandCollider.enabled = false;
        _rightForeArmCollider.enabled = false;
    }
    
    public void ActivateWeaponCollidersRight()
    {
        _leftHandCollider.enabled = false;
        _leftForeArmCollider.enabled = false;
        _rightHandCollider.enabled = true;
        _rightForeArmCollider.enabled = true;
    }
    
    public void DeactivateWeaponColliders()
    {
        _leftHandCollider.enabled = false;
        _leftForeArmCollider.enabled = false;
        _rightHandCollider.enabled = false;
        _rightForeArmCollider.enabled = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
}
