using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 50f;

    // This function gets called by your gun
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(transform.name + " took " + amount + " damage! HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add death animation or particle effects here later!
        Destroy(gameObject);
    }
}