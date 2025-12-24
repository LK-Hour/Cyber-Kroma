using UnityEngine;

public class EnemyPhisher : MonoBehaviour
{
    [Header("Settings")]
    public Transform player;      // Drag Player here
    public GameObject projectile; // Drag 'FakeLinkProjectile' prefab here
    public Transform firePoint;   // Drag 'FirePos' here
    
    public float shootingRange = 20f;
    public float fireRate = 3.0f; // Shoot every 3 seconds

    private float nextFireTime = 0f;

    void Start()
    {
        // Auto-find player if empty
        if (player == null) 
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // 1. Check Distance
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootingRange)
        {
            // 2. Look at Player
            transform.LookAt(player);

            // 3. Shoot Loop
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        // Spawn the bullet
        Instantiate(projectile, firePoint.position, transform.rotation);
    }
}