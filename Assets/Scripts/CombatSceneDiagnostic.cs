using UnityEngine;

/// <summary>
/// Diagnostic script to check Scene_Combat_Test setup
/// Add this to any GameObject and run to see what's missing
/// </summary>
public class CombatSceneDiagnostic : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== COMBAT SCENE DIAGNOSTIC ===");
        CheckPlayerSetup();
        CheckDataCoreSetup();
        CheckEnemySetup();
        CheckWaveManagerSetup();
        CheckUISetup();
        Debug.Log("=== DIAGNOSTIC COMPLETE ===");
    }

    void CheckPlayerSetup()
    {
        Debug.Log("🔍 Checking Player Setup...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("❌ No Player found with 'Player' tag!");
            return;
        }

        Debug.Log("✅ Player found: " + player.name);

        var shooting = player.GetComponent<CharacterShooting>();
        if (shooting == null)
        {
            Debug.LogError("❌ Player missing CharacterShooting component!");
        }
        else
        {
            Debug.Log("✅ CharacterShooting component found");
            if (shooting.playerCamera == null) Debug.LogWarning("⚠️ CharacterShooting.playerCamera not assigned");
            if (shooting.firePoint == null) Debug.LogWarning("⚠️ CharacterShooting.firePoint not assigned");
            if (shooting.bulletTrailPrefab == null) Debug.LogWarning("⚠️ CharacterShooting.bulletTrailPrefab not assigned");
        }

        var health = player.GetComponent<CharacterHealth>();
        if (health == null)
        {
            Debug.LogError("❌ Player missing CharacterHealth component!");
        }
        else
        {
            Debug.Log("✅ CharacterHealth component found");
        }

        var movement = player.GetComponent<CharacterMovement>();
        if (movement == null)
        {
            Debug.LogError("❌ Player missing CharacterMovement component!");
        }
        else
        {
            Debug.Log("✅ CharacterMovement component found");
        }
    }

    void CheckDataCoreSetup()
    {
        Debug.Log("🔍 Checking DataCore Setup...");

        GameObject dataCore = GameObject.FindGameObjectWithTag("DataCore");
        if (dataCore == null)
        {
            Debug.LogError("❌ No DataCore found with 'DataCore' tag!");
            return;
        }

        Debug.Log("✅ DataCore found: " + dataCore.name);

        var coreHealth = dataCore.GetComponent<DataCoreHealth>();
        if (coreHealth == null)
        {
            Debug.LogError("❌ DataCore missing DataCoreHealth component!");
        }
        else
        {
            Debug.Log("✅ DataCoreHealth component found");
        }
    }

    void CheckEnemySetup()
    {
        Debug.Log("🔍 Checking Enemy Setup...");

        // Check if enemy prefabs exist
        GameObject phisher = Resources.Load<GameObject>("Prefabs/Enemies/Phisher");
        GameObject ghost = Resources.Load<GameObject>("Prefabs/Enemies/GhostAccount");
        GameObject deepFake = Resources.Load<GameObject>("Prefabs/Enemies/DeepFake");

        Debug.Log($"Phisher prefab: {(phisher != null ? "✅ Found" : "❌ Missing")}");
        Debug.Log($"GhostAccount prefab: {(ghost != null ? "✅ Found" : "❌ Missing")}");
        Debug.Log($"DeepFake prefab: {(deepFake != null ? "✅ Found" : "❌ Missing")}");

        // Check if enemies have required components
        if (phisher != null)
        {
            CheckEnemyComponents(phisher, "Phisher");
        }
        if (ghost != null)
        {
            CheckEnemyComponents(ghost, "GhostAccount");
        }
        if (deepFake != null)
        {
            CheckEnemyComponents(deepFake, "DeepFake");
        }
    }

    void CheckEnemyComponents(GameObject enemyPrefab, string name)
    {
        var enemyAI = enemyPrefab.GetComponent<EnemyAI>();
        if (enemyAI == null)
        {
            Debug.LogError($"❌ {name} missing EnemyAI component!");
        }
        else
        {
            Debug.Log($"✅ {name} has EnemyAI component");
        }

        var navAgent = enemyPrefab.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError($"❌ {name} missing NavMeshAgent component!");
        }
        else
        {
            Debug.Log($"✅ {name} has NavMeshAgent component");
        }
    }

    void CheckWaveManagerSetup()
    {
        Debug.Log("🔍 Checking WaveManager Setup...");

        var waveManager = FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogError("❌ No WaveManager found in scene!");
            return;
        }

        Debug.Log("✅ WaveManager found");

        if (waveManager.spawnPoints == null || waveManager.spawnPoints.Length == 0)
        {
            Debug.LogWarning("⚠️ WaveManager.spawnPoints not assigned");
        }
        else
        {
            Debug.Log($"✅ WaveManager has {waveManager.spawnPoints.Length} spawn points");
        }

        if (waveManager.enemyPrefabs == null || waveManager.enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("⚠️ WaveManager.enemyPrefabs not assigned");
        }
        else
        {
            Debug.Log($"✅ WaveManager has {waveManager.enemyPrefabs.Length} enemy prefabs");
        }
    }

    void CheckUISetup()
    {
        Debug.Log("🔍 Checking UI Setup...");

        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found in scene!");
            return;
        }

        Debug.Log("✅ Canvas found");

        // Check panels
        GameObject shopPanel = GameObject.Find("Canvas/ShopPanel");
        GameObject victoryPanel = GameObject.Find("Canvas/VictoryPanel");
        GameObject defeatPanel = GameObject.Find("Canvas/DefeatPanel");
        GameObject pauseMenu = GameObject.Find("Canvas/PauseMenu");

        Debug.Log($"ShopPanel: {(shopPanel != null ? "✅ Found" : "❌ Missing")}");
        Debug.Log($"VictoryPanel: {(victoryPanel != null ? "✅ Found" : "❌ Missing")}");
        Debug.Log($"DefeatPanel: {(defeatPanel != null ? "✅ Found" : "❌ Missing")}");
        Debug.Log($"PauseMenu: {(pauseMenu != null ? "✅ Found" : "❌ Missing")}");

        // Check CombatUI
        var combatUI = FindObjectOfType<CombatSceneUI>();
        if (combatUI == null)
        {
            Debug.LogError("❌ No CombatSceneUI found in scene!");
        }
        else
        {
            Debug.Log("✅ CombatSceneUI found");
        }
    }
}