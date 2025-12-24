using UnityEngine;
using System.Collections; // Required for Coroutines

public class CharacterHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public float respawnDelay = 5.0f; // How long to wait before respawning
    private int currentHealth;

    [Header("References")]
    public Transform respawnPoint; // DRAG YOUR RESPAWN POINT HERE
    public Animator animator;
    public CharacterMovement movementScript;
    public CharacterShooting shootingScript;
    public CharacterController characterController;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (movementScript == null) movementScript = GetComponent<CharacterMovement>();
        if (shootingScript == null) shootingScript = GetComponent<CharacterShooting>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Test key to die
        if (Input.GetKeyDown(KeyCode.K) && !isDead)
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");

        // 1. Play Animation
        animator.SetTrigger("Die");
        
        // 2. Turn off Aiming (Layer 1) so arms don't float
        animator.SetLayerWeight(1, 0); 

        // 3. Disable Controls
        if (movementScript != null) movementScript.enabled = false;
        if (shootingScript != null) shootingScript.enabled = false;
        if (characterController != null) characterController.enabled = false;

        // 4. Start the Respawn Timer
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        // Wait for X seconds (Let the death animation finish)
        yield return new WaitForSeconds(respawnDelay);

        Debug.Log("Respawning...");

        // 1. Reset Stats
        isDead = false;
        currentHealth = maxHealth;

        // 2. Teleport to Spawn Point
        // (Must be done while CharacterController is disabled!)
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }
        else
        {
            Debug.LogError("Assign a RespawnPoint in the Inspector!");
        }

        // 3. Reset Animation (Rebind acts like a hard reset)
        animator.Rebind(); 
        animator.SetLayerWeight(1, 1); // Turn Aiming back ON

        // 4. Reset Camera Angle (Look forward again)
        movementScript.ResetCamera(); 

        // 5. Re-enable Controls
        if (characterController != null) characterController.enabled = true;
        if (movementScript != null) movementScript.enabled = true;
        if (shootingScript != null) shootingScript.enabled = true;
    }
}