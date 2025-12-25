using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataCoreHealth : MonoBehaviour
{
    [Header("Core Stats")]
    public int maxHealth = 1000;
    public int currentHealth;
    
    [Header("UI References")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public GameObject defeatPanel;
    
    [Header("Visual Effects")]
    public Material coreMaterial;
    public Color fullHealthColor = Color.cyan;
    public Color lowHealthColor = Color.red;
    public ParticleSystem damageEffect;
    
    private WaveManager waveManager;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
        
        waveManager = FindObjectOfType<WaveManager>();
        
        if (coreMaterial != null)
        {
            coreMaterial.SetColor("_EmissionColor", fullHealthColor);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        UpdateUI();
        
        // Visual feedback
        if (damageEffect != null)
        {
            damageEffect.Play();
        }
        
        // Update emission color based on health
        if (coreMaterial != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            Color emissionColor = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
            coreMaterial.SetColor("_EmissionColor", emissionColor * 2f);
        }
        
        // Check for game over
        if (currentHealth <= 0)
        {
            OnCoreDestroyed();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = $"Core: {currentHealth}/{maxHealth}";
        }
    }

    void OnCoreDestroyed()
    {
        Debug.Log("Data Core Destroyed! Game Over!");
        
        // Show defeat screen
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
        
        // Notify wave manager
        if (waveManager != null)
        {
            waveManager.OnCoreDestroyed();
        }
        
        // Stop game
        Time.timeScale = 0f;
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }
}
