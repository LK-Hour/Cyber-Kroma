using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    public float attackRange = 2.0f;
    public float attackCooldown = 1.5f;
    
    private NavMeshAgent agent;
    private Animator anim;
    private Transform target;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        // Find the DataCore
        GameObject core = GameObject.FindGameObjectWithTag("Core");
        if (core != null) target = core.transform;

        // Set NavMesh Agent stopping distance to match attack range
        agent.stoppingDistance = attackRange;
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            // Close enough to Smash
            AttackLogic();
        }
        else
        {
            // Too far, keep Walking
            MoveToTarget();
        }

        // Update the "Speed" parameter for your Idle <-> Walking transitions
        // We use agent.velocity.magnitude to see if the NavMesh is actually moving
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void MoveToTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    void AttackLogic()
    {
        // Stop moving to perform the Smash
        agent.isStopped = true;

        // Look at the core while smashing
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);

        // Check if cooldown is ready
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // This triggers your "Any State -> Smash" transition
            anim.SetTrigger("Smash"); 
            lastAttackTime = Time.time;
            
            // Logic for damaging the core would go here
            Debug.Log("Enemy Smashed the Core!");
        }
    }
}