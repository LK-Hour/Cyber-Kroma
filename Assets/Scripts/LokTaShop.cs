using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LokTaShop : MonoBehaviour
{
    [Header("Shop Items")]
    public int healthPotionCost = 50;
    public int shieldCost = 75;
    public int ammoCost = 30;
    
    public int healthPotionAmount = 50;
    public int ammoAmount = 30;
    
    [Header("UI References")]
    public Button healthButton;
    public Button shieldButton;
    public Button ammoButton;
    public TextMeshProUGUI recommendationText;
    public TextMeshProUGUI pointsText;
    
    [Header("Player Reference")]
    public GameObject player;
    
    private int playerPoints = 100; // Points earned from killing enemies
    private CharacterHealth playerHealth;
    private CharacterShooting playerShooting;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            playerHealth = player.GetComponent<CharacterHealth>();
            playerShooting = player.GetComponent<CharacterShooting>();
        }
        
        // Setup button listeners
        if (healthButton != null)
            healthButton.onClick.AddListener(() => BuyHealthPotion());
        if (shieldButton != null)
            shieldButton.onClick.AddListener(() => BuyShield());
        if (ammoButton != null)
            ammoButton.onClick.AddListener(() => BuyAmmo());
    }

    public void OnShopOpened()
    {
        // AI Recommendation based on player state
        string recommendation = GenerateRecommendation();
        if (recommendationText != null)
        {
            recommendationText.text = recommendation;
        }
        
        UpdatePointsDisplay();
    }

    string GenerateRecommendation()
    {
        // AI-like logic to recommend items
        if (playerHealth != null)
        {
            float healthPercent = (float)playerHealth.currentHealth / playerHealth.maxHealth;
            
            if (healthPercent < 0.3f)
            {
                return "ចូ! អ្នកមានសុខភាពតិច! ទិញថ្នាំព្យាបាល Antivirus!\n(Chau! Your health is low! Buy Antivirus Potion!)";
            }
        }
        
        if (playerShooting != null && playerShooting.currentAmmo < 10)
        {
            return "បាញ់ខ្លះទៀត! ទិញ Data Bullets!\n(Keep shooting! Buy Data Bullets!)";
        }
        
        // Default recommendation
        return "ជំរាបសួរ! Lok Ta ណែនាំ 2FA Shield សម្រាប់ការការពារ!\n(Hello! Lok Ta recommends 2FA Shield for protection!)";
    }

    void BuyHealthPotion()
    {
        if (playerPoints >= healthPotionCost)
        {
            playerPoints -= healthPotionCost;
            
            if (playerHealth != null)
            {
                playerHealth.Heal(healthPotionAmount);
            }
            
            UpdatePointsDisplay();
            Debug.Log("Bought Health Potion!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    void BuyShield()
    {
        if (playerPoints >= shieldCost)
        {
            playerPoints -= shieldCost;
            
            // Give temporary shield buff
            if (playerHealth != null)
            {
                // TODO: Implement shield system
                playerHealth.currentHealth += 25;
            }
            
            UpdatePointsDisplay();
            Debug.Log("Bought Shield!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    void BuyAmmo()
    {
        if (playerPoints >= ammoCost)
        {
            playerPoints -= ammoCost;
            
            if (playerShooting != null)
            {
                playerShooting.currentAmmo += ammoAmount;
            }
            
            UpdatePointsDisplay();
            Debug.Log("Bought Ammo!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    public void AddPoints(int amount)
    {
        playerPoints += amount;
        UpdatePointsDisplay();
    }

    void UpdatePointsDisplay()
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {playerPoints}";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnShopOpened();
        }
    }
}
