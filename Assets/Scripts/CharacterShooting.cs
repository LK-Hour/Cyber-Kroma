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
    public float fireRate = 0.15f; // Speed of the machine gun
    public int maxAmmo = 30;

    private int currentAmmo;
    private float nextTimeToFire = 0f;

    // This is the "Switch" for the button
    private bool isHoldingButton = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        if (animator == null) animator = GetComponent<Animator>();
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // LOGIC: If holding the Mobile Button OR holding Left Mouse
        if (isHoldingButton || Input.GetButton("Fire1"))
        {
            // Only shoot if enough time has passed (Fire Rate)
            if (Time.time >= nextTimeToFire && currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    // --- CONNECT THESE TO THE EVENT TRIGGER ---
    public void StartFiring()
    {
        isHoldingButton = true; // Finger is DOWN
    }

    public void StopFiring()
    {
        isHoldingButton = false; // Finger is UP
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
        currentAmmo = maxAmmo;
    }
    // ------------------------------------------

    void Shoot()
    {
        currentAmmo--;
        animator.SetTrigger("Shoot"); // Make sure this animation has NO LOOP time!

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            targetPoint = hit.point;
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            if (enemy != null) enemy.TakeDamage(damage);
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