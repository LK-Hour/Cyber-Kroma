using UnityEngine;
using TMPro;

/// <summary>
/// Player points economy system for shop purchases
/// Points are earned by killing enemies (managed by WaveManager)
/// </summary>
public class PlayerPoints : MonoBehaviour
{
    [Header("Points Configuration")]
    public int currentPoints = 0;
    public int startingPoints = 100; // Give players some starting points

    [Header("Points Per Enemy Type")]
    public int pointsPerPhisher = 10;
    public int pointsPerGhostAccount = 15;
    public int pointsPerDeepFake = 50;

    [Header("UI References")]
    public TextMeshProUGUI pointsText;

    private static PlayerPoints instance;
    public static PlayerPoints Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerPoints>();
            }
            return instance;
        }
    }

    void Awake()
    {
        // Singleton pattern - ensure only one instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentPoints = startingPoints;
        UpdateUI();
    }

    /// <summary>
    /// Add points when enemy is killed (called by WaveManager or EnemyAI)
    /// </summary>
    public void AddPoints(int amount)
    {
        currentPoints += amount;
        Debug.Log($"💰 Gained {amount} points! Total: {currentPoints}");
        UpdateUI();
    }

    /// <summary>
    /// Spend points on shop items (called by LokTaShop)
    /// Returns true if purchase successful, false if not enough points
    /// </summary>
    public bool SpendPoints(int amount)
    {
        if (currentPoints >= amount)
        {
            currentPoints -= amount;
            Debug.Log($"💸 Spent {amount} points. Remaining: {currentPoints}");
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log($"❌ Not enough points! Need {amount}, have {currentPoints}");
            return false;
        }
    }

    /// <summary>
    /// Check if player can afford an item
    /// </summary>
    public bool CanAfford(int cost)
    {
        return currentPoints >= cost;
    }

    /// <summary>
    /// Get current points (for shop AI recommendations)
    /// </summary>
    public int GetPoints()
    {
        return currentPoints;
    }

    /// <summary>
    /// Update UI text display
    /// </summary>
    private void UpdateUI()
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {currentPoints}";
        }
    }

    /// <summary>
    /// Award points based on enemy type (called by WaveManager)
    /// </summary>
    public void OnEnemyKilled(EnemyAI.EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyAI.EnemyType.Phisher:
                AddPoints(pointsPerPhisher);
                break;
            case EnemyAI.EnemyType.GhostAccount:
                AddPoints(pointsPerGhostAccount);
                break;
            case EnemyAI.EnemyType.DeepFake:
                AddPoints(pointsPerDeepFake);
                break;
        }
    }

    /// <summary>
    /// Reset points to starting amount (for new game)
    /// </summary>
    public void ResetPoints()
    {
        currentPoints = startingPoints;
        UpdateUI();
    }
}
