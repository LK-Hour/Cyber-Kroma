using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.Phisher;
    
    [Header("Target References")]
    public Transform dataCore;    // The objective enemies attack
    public Transform player;       // Current player target (for certain types)
    
    [Header("Stats")]
    public int maxHealth = 50;
    public float moveSpeed = 3.5f;
    public float attackDamage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    
    [Header("Phisher Specific")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootingRange = 15f;
    public float fireRate = 2f;
    
    [Header("Ghost Account Specific")]
    public Material invisibleMaterial;
    public float detectionRange = 5f; // Scanner class can detect within this range
    
    [Header("DeepFake Specific")]
    public GameObject playerMimicPrefab;
    
    private NavMeshAgent agent;
    private Animator animator;
    private int currentHealth;
    private float nextAttackTime;
    private float nextFireTime;
    private bool isAttacking;
    private Transform currentTarget;
    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    
    public enum EnemyType
    {
        Phisher,        // Ranged attacker
        GhostAccount,   // Stealth, invisible
        DeepFake        // Boss that mimics player
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        
        currentHealth = maxHealth;
        agent.speed = moveSpeed;
        
        // Auto-find Data Core if not assigned
        if (dataCore == null)
        {
            GameObject core = GameObject.FindGameObjectWithTag("DataCore");
            if (core != null) dataCore = core.transform;
        }
        
        // Set initial target based on enemy type
        switch (enemyType)
        {
            case EnemyType.GhostAccount:
                // Ghost accounts ignore player, go straight to core
                currentTarget = dataCore;
                MakeInvisible();
                break;
            case EnemyType.Phisher:
            case EnemyType.DeepFake:
            default:
                // Attack player if close, otherwise go to core
                currentTarget = dataCore;
                break;
        }
        
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return;
        
        // Update target based on enemy type
        UpdateTarget();
        
        // Move to target
        if (currentTarget != null && agent.isOnNavMesh)
        {
            agent.SetDestination(currentTarget.position);
            
            // Update animator
            if (animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }
        
        // Attack logic
        HandleAttack();
    }

    void UpdateTarget()
    {
        switch (enemyType)
        {
            case EnemyType.Phisher:
                // Find closest player or stick to core
                GameObject closestPlayer = FindClosestPlayer();
                if (closestPlayer != null)
                {
                    float distToPlayer = Vector3.Distance(transform.position, closestPlayer.transform.position);
                    if (distToPlayer < shootingRange)
                    {
                        currentTarget = closestPlayer.transform;
                    }
                    else
                    {
                        currentTarget = dataCore;
                    }
                }
                break;
                
            case EnemyType.GhostAccount:
                // Always target data core, ignore players
                currentTarget = dataCore;
                break;
                
            case EnemyType.DeepFake:
                // Target closest player
                GameObject targetPlayer = FindClosestPlayer();
                currentTarget = targetPlayer != null ? targetPlayer.transform : dataCore;
                break;
        }
    }

    void HandleAttack()
    {
        if (currentTarget == null) return;
        
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        
        switch (enemyType)
        {
            case EnemyType.Phisher:
                // Ranged attack
                if (distanceToTarget <= shootingRange && Time.time >= nextFireTime)
                {
                    ShootProjectile();
                    nextFireTime = Time.time + fireRate;
                }
                break;
                
            case EnemyType.GhostAccount:
            case EnemyType.DeepFake:
                // Melee attack
                if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
                {
                    PerformMeleeAttack();
                    nextAttackTime = Time.time + attackCooldown;
                }
                break;
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;
        
        // Look at target
        transform.LookAt(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z));
        
        // Spawn projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        
        // Play animation
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }

    void PerformMeleeAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Deal damage to target
        if (currentTarget.CompareTag("Player"))
        {
            CharacterHealth playerHealth = currentTarget.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)attackDamage);
            }
        }
        else if (currentTarget.CompareTag("DataCore"))
        {
            DataCoreHealth coreHealth = currentTarget.GetComponent<DataCoreHealth>();
            if (coreHealth != null)
            {
                coreHealth.TakeDamage((int)attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Notify wave manager
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.OnEnemyDeath(this);
        }
        
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        // Disable components
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        
        // Destroy after animation
        Destroy(gameObject, 2f);
    }

    void MakeInvisible()
    {
        if (invisibleMaterial != null && meshRenderer != null)
        {
            meshRenderer.material = invisibleMaterial;
        }
        else if (meshRenderer != null)
        {
            // Make semi-transparent
            Color color = meshRenderer.material.color;
            color.a = 0.2f;
            meshRenderer.material.color = color;
        }
    }

    GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }
        
        return closest;
    }

    // Called by Scanner class to reveal ghost accounts
    public void Reveal()
    {
        if (enemyType == EnemyType.GhostAccount && meshRenderer != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize attack ranges
        Gizmos.color = Color.red;
        if (enemyType == EnemyType.Phisher)
        {
            Gizmos.DrawWireSphere(transform.position, shootingRange);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
