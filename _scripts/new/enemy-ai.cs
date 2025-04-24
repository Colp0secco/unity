using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;
    
    private Transform player;
    private Vector3 startingPosition;
    private Health enemyHealth;
    private float nextAttackTime = 0f;
    
    private enum State { Idle, Patrolling, Chasing, Attacking }
    private State currentState = State.Idle;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startingPosition = transform.position;
        enemyHealth = GetComponent<Health>();
        
        if (enemyHealth == null)
        {
            enemyHealth = gameObject.AddComponent<Health>();
        }
    }
    
    private void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // State management
        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attacking;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Patrolling;
        }
        
        // Handle state behavior
        switch (currentState)
        {
            case State.Idle:
                // Do nothing
                break;
                
            case State.Patrolling:
                // Simple patrolling - return to starting position
                if (Vector2.Distance(transform.position, startingPosition) > 0.1f)
                {
                    Vector2 direction = (startingPosition - transform.position).normalized;
                    transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
                }
                break;
                
            case State.Chasing:
                // Move toward player
                Vector2 chaseDirection = (player.position - transform.position).normalized;
                transform.position += (Vector3)chaseDirection * moveSpeed * Time.deltaTime;
                break;
                
            case State.Attacking:
                if (Time.time >= nextAttackTime)
                {
                    AttackPlayer();
                    nextAttackTime = Time.time + attackCooldown;
                }
                break;
        }
    }
    
    private void AttackPlayer()
    {
        // Simple implementation: direct damage to player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
