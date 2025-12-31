using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int totalWaves = 3;
    public int currentWave = 0;
    public float timeBetweenWaves = 10f;
    
    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs; // [0]=Phisher, [1]=GhostAccount, [2]=DeepFake
    
    [Header("Wave Configurations")]
    public WaveConfig[] waves;
    
    [Header("UI References")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemyCountText;
    public GameObject shopPanel;
    public GameObject victoryPanel;
    
    private List<EnemyAI> activeEnemies = new List<EnemyAI>();
    private bool gameOver = false;
    private LokTaShop shop;

    [System.Serializable]
    public class WaveConfig
    {
        public int phisherCount = 5;
        public int ghostCount = 2;
        public int deepFakeCount = 0;
        public float spawnDelay = 0.5f;
    }

    void Start()
    {
        shop = FindObjectOfType<LokTaShop>();
        
        // Create default waves if not configured
        if (waves == null || waves.Length == 0)
        {
            CreateDefaultWaves();
        }
        
        StartCoroutine(GameLoop());
    }

    void CreateDefaultWaves()
    {
        waves = new WaveConfig[3];
        
        // Wave 1: Easy intro
        waves[0] = new WaveConfig { phisherCount = 5, ghostCount = 0, deepFakeCount = 0, spawnDelay = 1f };
        
        // Wave 2: Medium difficulty with ghosts
        waves[1] = new WaveConfig { phisherCount = 7, ghostCount = 3, deepFakeCount = 0, spawnDelay = 0.7f };
        
        // Wave 3: Final boss wave
        waves[2] = new WaveConfig { phisherCount = 8, ghostCount = 5, deepFakeCount = 1, spawnDelay = 0.5f };
    }

    IEnumerator GameLoop()
    {
        while (currentWave < totalWaves && !gameOver)
        {
            currentWave++;
            UpdateWaveUI();
            
            // Show wave start message
            Debug.Log($"Wave {currentWave} Starting!");
            
            // Spawn wave
            yield return StartCoroutine(SpawnWave(waves[currentWave - 1]));
            
            // Wait for all enemies to be defeated
            while (activeEnemies.Count > 0 && !gameOver)
            {
                yield return null;
            }
            
            if (gameOver) break;
            
            // Wave completed
            Debug.Log($"Wave {currentWave} Completed!");
            
            // Show educational popup (if not final wave)
            if (currentWave < totalWaves)
            {
                ShowEducationalPopup();
                
                // Show shop
                OpenShop();
                
                // Wait for shop phase
                yield return new WaitForSeconds(timeBetweenWaves);
                
                CloseShop();
            }
        }
        
        // All waves completed
        if (!gameOver)
        {
            OnVictory();
        }
    }

    IEnumerator SpawnWave(WaveConfig config)
    {
        // Spawn Phishers
        for (int i = 0; i < config.phisherCount; i++)
        {
            SpawnEnemy(0); // Phisher
            yield return new WaitForSeconds(config.spawnDelay);
        }
        
        // Spawn Ghost Accounts
        for (int i = 0; i < config.ghostCount; i++)
        {
            SpawnEnemy(1); // GhostAccount
            yield return new WaitForSeconds(config.spawnDelay);
        }
        
        // Spawn DeepFakes (Boss)
        for (int i = 0; i < config.deepFakeCount; i++)
        {
            SpawnEnemy(2); // DeepFake
            yield return new WaitForSeconds(config.spawnDelay);
        }
    }

    void SpawnEnemy(int enemyTypeIndex)
    {
        if (enemyPrefabs == null || enemyTypeIndex >= enemyPrefabs.Length) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        
        // Pick random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn enemy
        GameObject enemyObj = Instantiate(enemyPrefabs[enemyTypeIndex], spawnPoint.position, Quaternion.identity);
        EnemyAI enemy = enemyObj.GetComponent<EnemyAI>();
        
        if (enemy != null)
        {
            activeEnemies.Add(enemy);
            UpdateEnemyCountUI();
        }
    }

    public void OnEnemyDeath(EnemyAI enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            UpdateEnemyCountUI();

            // Award points to player
            PlayerPoints playerPoints = PlayerPoints.Instance;
            if (playerPoints != null)
            {
                playerPoints.OnEnemyKilled(enemy.enemyType);
            }
        }
    }

    public void OnCoreDestroyed()
    {
        gameOver = true;
        Debug.Log("💀 Game Over - Core Destroyed!");
        
        // Show defeat UI panel
        GameObject defeatPanel = GameObject.Find("DefeatPanel");
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
        
        Time.timeScale = 0f;
        
        // Return to main menu after 5 seconds
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.OnDefeat();
        }
    }

    void OnVictory()
    {
        Debug.Log("🏆 Victory! All waves defeated!");
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        Time.timeScale = 0f;
        
        // Return to main menu after 5 seconds
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.OnVictory();
        }
    }

    void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
        
        if (shop != null)
        {
            shop.OnShopOpened();
        }
    }

    void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    void ShowEducationalPopup()
    {
        // Get the scam education system
        ScamEducationUI education = FindObjectOfType<ScamEducationUI>();
        if (education != null)
        {
            // Show info about the enemies just fought
            if (currentWave == 1)
            {
                education.ShowScamInfo("Phishing");
            }
            else if (currentWave == 2)
            {
                education.ShowScamInfo("GhostAccount");
            }
            else if (currentWave == 5)
            {
                education.ShowScamInfo("DeepFake");
            }
        }
    }

    void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}/{totalWaves}";
        }
    }

    void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {activeEnemies.Count}";
        }
    }
}
