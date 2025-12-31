using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Spawns the correct player prefab based on selected class
/// Attach to a GameObject in the gameplay scene
/// </summary>
public class PlayerClassSpawner : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject firewallPrefab;   // High defense tank
    public GameObject debuggerPrefab;   // 🔧 High damage DPS
    public GameObject scannerPrefab;    // Detection specialist
    
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public bool spawnOnStart = true;
    
    void Start()
    {
        if (spawnOnStart)
        {
            SpawnPlayerByClass();
        }
    }
    
    public void SpawnPlayerByClass()
    {
        // Get the class selected in the lobby
        string selectedClass = PlayerPrefs.GetString("SelectedClass", "Firewall");
        
        Debug.Log($"Spawning player as: {selectedClass}");
        
        // Select the correct prefab
        GameObject prefabToSpawn = null;
        
        switch (selectedClass)
        {
            case "Firewall":
                prefabToSpawn = firewallPrefab;
                break;
            case "Debugger":
                prefabToSpawn = debuggerPrefab;
                break;
            case "Scanner":
                prefabToSpawn = scannerPrefab;
                break;
            default:
                prefabToSpawn = firewallPrefab; // Fallback to Firewall
                Debug.LogWarning($"Unknown class: {selectedClass}, using Firewall");
                break;
        }
        
        if (prefabToSpawn == null)
        {
            Debug.LogError($"❌ Prefab for {selectedClass} is not assigned!");
            return;
        }
        
        // Determine spawn position
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion spawnRot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;
        
        // Spawn the player
        GameObject player = Instantiate(prefabToSpawn, spawnPos, spawnRot);
        player.name = $"Player_{selectedClass}";
        
        // Apply class-specific stats
        ApplyClassStats(player, selectedClass);
        
        Debug.Log($"Player spawned as {selectedClass} at {spawnPos}");
    }
    
    void ApplyClassStats(GameObject player, string className)
    {
        // Find CharacterHealth component
        CharacterHealth health = player.GetComponent<CharacterHealth>();
        CharacterShooting shooting = player.GetComponent<CharacterShooting>();
        CharacterMovement movement = player.GetComponent<CharacterMovement>();
        
        if (health == null || shooting == null || movement == null)
        {
            Debug.LogWarning("⚠️ Player missing required components for stat adjustment");
            return;
        }
        
        // Apply stats based on class
        switch (className)
        {
            case "Firewall":
                // Tank: High HP, slow, medium damage
                health.maxHealth = 200;
                movement.walkSpeed = 2.5f;
                movement.runSpeed = 5f;
                // shooting.damage stays default
                Debug.Log("Firewall stats: HP=200, Speed=Slow, Defense=High");
                break;
                
            case "Debugger":
                // DPS: Medium HP, fast, high damage
                health.maxHealth = 100;
                movement.walkSpeed = 4f;
                movement.runSpeed = 8f;
                // shooting.damage would be increased (need to add damage property)
                Debug.Log("🔧 Debugger stats: HP=100, Speed=Fast, Damage=High");
                break;
                
            case "Scanner":
                // Support: Low HP, medium speed, detection
                health.maxHealth = 80;
                movement.walkSpeed = 3.5f;
                movement.runSpeed = 7f;
                // Add detection radius boost (would need DetectionSystem component)
                Debug.Log("Scanner stats: HP=80, Speed=Medium, Detection=High");
                break;
        }
    }
}
