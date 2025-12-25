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
    
    private CharacterHealth playerHealth;
    private CharacterShooting playerShooting;
    private PlayerPoints playerPoints;

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

        // Get PlayerPoints singleton
        playerPoints = PlayerPoints.Instance;
        
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
        
    }

    string GenerateRecommendation()
    {
        // AI-like logic to recommend items
        if (playerHealth != null)
        {
            float healthPercent = (float)playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
            
            if (healthPercent < 0.3f)
            {
                return "ចៅ! អស់ឈាមហើយ! ទិញថ្នាំព្យាបាល Antivirus!\n(Chau! Your health is low! Buy Antivirus Potion!)";
            }
        }
        
        if (playerShooting != null && playerShooting.currentAmmo < 10)
        {
            return "បាញ់ទៀតទៅ! ហើយទិញ Data Bulletsផងចៅ!\n(Keep shooting! Buy Data Bullets!)";
        }
        
        // Default recommendation
        return "ជំរាបសួរ! Lok Ta ណែនាំ 2FA Shield សម្រាប់ការការពារ!\n(Hello! Lok Ta recommends 2FA Shield for protection!)";
    }

    void BuyHealthPotion()
    {
        if (playerPoints != null && playerPoints.SpendPoints(healthPotionCost))
        {
            if (playerHealth != null)
            {
                playerHealth.Heal(healthPotionAmount);
            }
            
            Debug.Log("Bought Health Potion!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    void BuyShield()
    {
        if (playerPoints != null && playerPoints.SpendPoints(shieldCost))
        {
            // Give temporary shield buff
            if (playerHealth != null)
            {
                // Shield = extra health
                playerHealth.Heal(25);
            }
            
            Debug.Log("Bought Shield!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }

    void BuyAmmo()
    {
        if (playerPoints != null && playerPoints.SpendPoints(ammoCost))
        {
            if (playerShooting != null)
            {
                playerShooting.currentAmmo += ammoAmount;
            }
            
            Debug.Log("Bought Ammo!");
        }
        else
        {
            Debug.Log("Not enough points!");
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
