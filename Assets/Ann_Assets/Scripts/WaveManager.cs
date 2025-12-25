using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject virusPrefab;  // Fast/Weak
    public GameObject wormPrefab;   // Medium
    public GameObject trojanPrefab; // Slow/Strong
    public GameObject bossPrefab;   // Wave 3 Final Boss

    [Header("Spawn Locations")]
    public Transform[] spawnPoints; // Your 4 empty objects

    [Header("Settings")]
    public float timeBetweenWaves = 5f;
    private int currentWave = 0;
    private bool isSpawning = false;

    void Update()
    {
        // Search for any object with the tag "Enemy"
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies are left and we aren't currently spawning a wave, start the next one
        if (enemiesInScene.Length == 0 && !isSpawning)
        {
            StartCoroutine(NextWaveRoutine());
        }
    }

    IEnumerator NextWaveRoutine()
    {
        isSpawning = true;
        currentWave++;
        
        Debug.Log("PREPARING WAVE " + currentWave);
        yield return new WaitForSeconds(timeBetweenWaves);

        if (currentWave == 1)
        {
            // WAVE 1: 5 Viruses
            yield return SpawnGroup(virusPrefab, 5);
        }
        else if (currentWave == 2)
        {
            // WAVE 2: The Swarm (Mix of all 3)
            yield return SpawnGroup(virusPrefab, 4);
            yield return SpawnGroup(wormPrefab, 3);
            yield return SpawnGroup(trojanPrefab, 2);
        }
        else if (currentWave == 3)
        {
            // WAVE 3: THE FINAL BOSS
            Debug.Log("CRITICAL THREAT DETECTED: BOSS SPAWNING!");
            yield return SpawnGroup(bossPrefab, 1);
        }
        else
        {
            Debug.Log("VICTORY! All malware cleared.");
            // Stop spawning after Wave 3
            yield break; 
        }

        isSpawning = false;
    }

    IEnumerator SpawnGroup(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Pick one of your 4 spawn points at random
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Create the enemy
            GameObject newEnemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            
            // IMPORTANT: Make sure the enemy is tagged so the script can count it
            newEnemy.tag = "Enemy"; 

            yield return new WaitForSeconds(1.0f); // 1 second delay between each enemy
        }
    }
}