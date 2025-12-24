using UnityEngine;

public class PhisherProjectile : MonoBehaviour
{
    public float speed = 15f;

    void Start()
    {
        // Destroy automatically after 5 seconds if it hits nothing
        Destroy(gameObject, 5.0f);
    }

    void Update()
    {
        // Move Forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // DEBUG: Tell us what we hit!
        Debug.Log("Bullet hit: " + other.name + " (Tag: " + other.tag + ")");

        // 1. IGNORE THE SHOOTER
        // If we hit the Enemy, do nothing (pass through)
        if (other.CompareTag("Enemy")) 
        {
            return; 
        }

        // 2. HIT THE PLAYER
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER HIT! Launching Virus...");
            
            // Find the popup script
            VirusPopup virusUI = FindObjectOfType<VirusPopup>();
            
            if (virusUI != null)
            {
                virusUI.OpenPopup();
            }
            else
            {
                Debug.LogError("Could not find VirusPopup script in the scene!");
            }

            Destroy(gameObject); // Delete bullet
        }
        // 3. HIT WALLS/FLOOR
        else 
        {
            Destroy(gameObject); // Delete bullet
        }
    }
}