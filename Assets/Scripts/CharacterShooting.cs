using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [Header("Setup")]
    public Camera playerCamera;
    public Animator animator;
    public Transform firePoint; 
    public LineRenderer bulletTrailPrefab;

    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.15f;
    public int maxAmmo = 30;

    public int currentAmmo; // Made public for shop access
    private float nextTimeToFire = 0f;

    // The Switch
    private bool isHoldingButton = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        if (animator == null) animator = GetComponent<Animator>();
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // 1. RELOAD (Keep 'R' key for PC testing if you want)
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            Reload();
            return;
        }

        // 2. SHOOTING LOGIC (The Fix)
        // We REMOVED "|| Input.GetButton("Fire1")"
        // Now it ONLY shoots if the UI Button is explicitly held down.
        if (isHoldingButton)
        {
            if (Time.time >= nextTimeToFire && currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    // --- BUTTON CONNECTORS ---
    public void StartFiring()
    {
        isHoldingButton = true;
    }

    public void StopFiring()
    {
        isHoldingButton = false;
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
        currentAmmo = maxAmmo;
    }
    // ------------------------

    void Shoot()
    {
        currentAmmo--;
        animator.SetTrigger("Shoot");

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            targetPoint = hit.point;
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage((int)damage);
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
        Destroy(trailObj, 0.05f);
    }
}