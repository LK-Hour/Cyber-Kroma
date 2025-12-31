using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Combat Scene UI Manager - Displays ammo, health, kills, wave info
/// </summary>
public class CombatSceneUI : MonoBehaviour
{
    [Header("Player Stats UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public Slider healthBar;
    
    [Header("Combat Stats UI")]
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI pointsText;
    
    [Header("Game State UI")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject pauseMenu;
    public GameObject shopPanel;
    public GameObject educationPanel;
    
    private int totalKills = 0;
    private CharacterHealth playerHealth;
    private CharacterShooting playerShooting;
    
    void Start()
    {
        // Auto-find player components
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<CharacterHealth>();
            playerShooting = player.GetComponent<CharacterShooting>();
        }
        
        // Auto-find panels if not assigned
        if (victoryPanel == null) victoryPanel = GameObject.Find("Canvas/VictoryPanel");
        if (defeatPanel == null) defeatPanel = GameObject.Find("Canvas/DefeatPanel");
        if (pauseMenu == null) pauseMenu = GameObject.Find("Canvas/PauseMenu");
        if (shopPanel == null) shopPanel = GameObject.Find("Canvas/ShopPanel");
        if (educationPanel == null) educationPanel = GameObject.Find("Canvas/EducationPanel");
        
        // Hide end game panels
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
        if (educationPanel != null) educationPanel.SetActive(false);
        
        UpdateAllUI();
    }
    
    void Update()
    {
        UpdatePlayerStats();
    }
    
    void UpdatePlayerStats()
    {
        // Update health
        if (playerHealth != null && healthText != null)
        {
            int curHP = playerHealth.GetCurrentHealth();
            int maxHP = playerHealth.maxHealth;
            healthText.text = $"HP: {Mathf.Max(0, curHP)}/{maxHP}";
            if (healthBar != null)
            {
                healthBar.value = maxHP > 0 ? curHP / (float)maxHP : 0f;
            }
        }
        
        // Update ammo
        if (playerShooting != null && ammoText != null)
        {
            ammoText.text = $"AMMO: {playerShooting.currentAmmo}/{playerShooting.maxAmmo}";
        }
    }
    
    public void UpdateWaveInfo(int currentWave, int totalWaves)
    {
        if (waveText != null)
        {
            waveText.text = $"WAVE {currentWave}/{totalWaves}";
        }
    }
    
    public void UpdateEnemiesRemaining(int count)
    {
        if (enemiesRemainingText != null)
        {
            enemiesRemainingText.text = $"ENEMIES: {count}";
        }
    }
    
    public void OnEnemyKilled()
    {
        totalKills++;
        if (killCountText != null)
        {
            killCountText.text = $"KILLS: {totalKills}";
        }
    }
    
    public void UpdatePoints(int points)
    {
        if (pointsText != null)
        {
            pointsText.text = $"POINTS: {points}";
        }
    }
    
    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f; // Pause game
        }
    }
    
    public void ShowDefeat()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0f; // Pause game
        }
    }
    
    void UpdateAllUI()
    {
        UpdateWaveInfo(1, 3);
        UpdateEnemiesRemaining(0);
        UpdatePoints(0);
        if (killCountText != null) killCountText.text = "KILLS: 0";
    }
}
