using UnityEngine;
using System.Collections; 

public class CharacterShooting : MonoBehaviour
{
    [Header("Setup")]
    public Camera playerCamera;
    public Animator animator;
    public Transform firePoint; 
    public LineRenderer bulletTrailPrefab; // We are back to using LineRenderer!

    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.15f;
    public int maxAmmo = 30;

    private int currentAmmo;
    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        if (animator == null) animator = GetComponent<Animator>();
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            Reload();
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    void Reload()
    {
        animator.SetTrigger("Reload");
        currentAmmo = maxAmmo;
    }

    void Shoot()
    {
        currentAmmo--;
        animator.SetTrigger("Shoot");

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            targetPoint = hit.point;

            // --- NEW DAMAGE LOGIC ---
            // 1. Try to find the Enemy script on the object we hit
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            
            // 2. If it exists, deal damage
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            // ------------------------
        }
        else
        {
            targetPoint = playerCamera.transform.position + playerCamera.transform.forward * range;
        }

        SpawnBulletTrail(targetPoint);
    }

    void SpawnBulletTrail(Vector3 hitPos)
    {
        GameObject trailObj = Instantiate(bulletTrailPrefab.gameObject, firePoint.position, Quaternion.identity);
        LineRenderer line = trailObj.GetComponent<LineRenderer>();

        line.SetPosition(0, firePoint.position);
        line.SetPosition(1, hitPos);

        // Destroy the laser line after 0.05 seconds so it flashes like a gunshot
        Destroy(trailObj, 0.05f);
    }
}