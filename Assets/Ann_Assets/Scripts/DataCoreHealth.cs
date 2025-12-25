using UnityEngine;
using UnityEngine.UI; // IMPORTANT: This allows the script to talk to the Slider

public class DataCoreHealth : MonoBehaviour
{
    [Header("Core Statistics")]
    public float maxHealth = 1000f;
    public float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider; // We will drag the UI Slider here

    void Start()
    {
        // 1. Initialize health
        currentHealth = maxHealth;

        // 2. Set up the Slider visuals
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    // This is the function the EnemyAI script calls
    public void TakeDamage(float damageAmount)
    {
        // 1. Reduce health
        currentHealth -= damageAmount;

        // 2. Update the visual bar
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // 3. Check for Death
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("SYSTEM CRITICAL: DATA CORE DESTROYED!");
        // Optional: You can freeze time or show a 'You Lose' screen here
        // Time.timeScale = 0; 
    }
}