using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Automatically configures enemy prefabs with correct AI settings
/// Phisher: Ranged attacker, medium health
/// GhostAccount: Fast melee attacker, low health, stealth
/// DeepFake: Tank, high health, slow, powerful attacks
/// </summary>
public class EnemyPrefabConfigurator : MonoBehaviour
{
    void Start()
    {
        ConfigureAllEnemies();
    }
    
    void ConfigureAllEnemies()
    {
        Debug.Log("🤖 Configuring enemy AI...");
        
        ConfigurePhisher();
        ConfigureGhostAccount();
        ConfigureDeepFake();
        
        Debug.Log("✅ All enemy AI configured!");
        
        // Disable after running once
        this.enabled = false;
    }
    
    #region Phisher Configuration
    void ConfigurePhisher()
    {
        GameObject phisher = GameObject.Find("Phisher");
        if (phisher == null)
        {
            Debug.LogWarning("Phisher enemy not found!");
            return;
        }
        
        // Configure NavMeshAgent
        NavMeshAgent agent = phisher.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = 3.5f;              // Medium speed
            agent.acceleration = 8f;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 8f;     // Stop at range for ranged attack
            agent.autoBraking = true;
            agent.radius = 0.5f;
            agent.height = 2f;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        
        // Configure EnemyAI
        EnemyAI enemyAI = phisher.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enemyType = EnemyAI.EnemyType.Phisher;
            enemyAI.maxHealth = 100;
            enemyAI.moveSpeed = 3.5f;
            enemyAI.attackDamage = 15;
            enemyAI.shootingRange = 10f;     // Phisher uses shootingRange for ranged attacks
            enemyAI.attackRange = 2f;        // Fallback melee range
            enemyAI.fireRate = 2.0f;
        }
        
        Debug.Log("✅ Phisher configured: HP=100, ShootingRange=10, Damage=15");
    }
    #endregion
    
    #region Ghost Account Configuration
    void ConfigureGhostAccount()
    {
        GameObject ghost = GameObject.Find("GhostAccount");
        if (ghost == null)
        {
            Debug.LogWarning("GhostAccount enemy not found!");
            return;
        }
        
        // Configure NavMeshAgent
        NavMeshAgent agent = ghost.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = 5.0f;              // Fast speed
            agent.acceleration = 10f;
            agent.angularSpeed = 180f;
            agent.stoppingDistance = 1.5f;   // Get close for melee
            agent.autoBraking = true;
            agent.radius = 0.4f;
            agent.height = 1.8f;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }
        
        // Configure EnemyAI
        EnemyAI enemyAI = ghost.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enemyType = EnemyAI.EnemyType.GhostAccount;
            enemyAI.maxHealth = 80;
            enemyAI.moveSpeed = 5.0f;
            enemyAI.attackDamage = 25;       // High damage
            enemyAI.attackRange = 2f;        // Melee range
            enemyAI.attackCooldown = 1.5f;
            enemyAI.detectionRange = 20f;    // Ghost Account uses detectionRange for stealth
        }
        
        Debug.Log("✅ GhostAccount configured: HP=80, Speed=5.0, Damage=25, Melee");
    }
    #endregion
    
    #region DeepFake Configuration
    void ConfigureDeepFake()
    {
        GameObject deepFake = GameObject.Find("DeepFake");
        if (deepFake == null)
        {
            Debug.LogWarning("DeepFake enemy not found!");
            return;
        }
        
        // Configure NavMeshAgent
        NavMeshAgent agent = deepFake.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = 2.5f;              // Slow but powerful
            agent.acceleration = 6f;
            agent.angularSpeed = 90f;
            agent.stoppingDistance = 12f;    // Long range tank
            agent.autoBraking = true;
            agent.radius = 0.7f;
            agent.height = 3f;               // Larger enemy
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        
        // Configure EnemyAI
        EnemyAI enemyAI = deepFake.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enemyType = EnemyAI.EnemyType.DeepFake;
            enemyAI.maxHealth = 300;         // Tank health
            enemyAI.moveSpeed = 2.5f;
            enemyAI.attackDamage = 30;       // Very high damage
            enemyAI.attackRange = 15f;       // Very long range
            enemyAI.attackCooldown = 3.0f;   // Slower attacks
        }
        
        Debug.Log("✅ DeepFake configured: HP=300, Range=15, Damage=30, Tank");
    }
    #endregion
}
