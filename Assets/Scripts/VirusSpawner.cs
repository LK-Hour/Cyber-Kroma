using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class VirusSpawner : MonoBehaviour
{
    [Header("Virus Settings")]
    public GameObject virusPrefab;
    public int maxVirus = 10;
    public float spawnRange = 10f;      // How far from spawner to spawn
    public float minDistance = 2f;      // Minimum distance between viruses
    public float minHeight = 1f;        // Min Y position
    public float maxHeight = 3f;        // Max Y position

    private List<GameObject> spawnedViruses = new List<GameObject>();

    void Start()
    {
        if (!Application.isPlaying)
        {
            SpawnViruses();
        }
    }

    public void SpawnViruses()
    {
        // Clear previously spawned viruses in editor
        foreach (GameObject v in spawnedViruses)
        {
            if (v != null)
                DestroyImmediate(v);
        }
        spawnedViruses.Clear();

        List<Vector3> positions = new List<Vector3>();
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = 1000;

        while (spawned < maxVirus && attempts < maxAttempts)
        {
            attempts++;

            // Random position around the spawner
            Vector3 pos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(minHeight, maxHeight),
                Random.Range(-spawnRange, spawnRange)
            ) + transform.position;

            // Check spacing
            bool tooClose = false;
            foreach (Vector3 otherPos in positions)
            {
                if (Vector3.Distance(pos, otherPos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
#if UNITY_EDITOR
                GameObject virus = (GameObject)PrefabUtility.InstantiatePrefab(virusPrefab, this.transform);
                virus.transform.position = pos;
#else
                GameObject virus = Instantiate(virusPrefab, pos, Quaternion.identity);
#endif
                spawnedViruses.Add(virus);
                positions.Add(pos);
                spawned++;
            }
        }

        if (spawned < maxVirus)
        {
            //Debug.LogWarning("Could not spawn all viruses due to spacing constraints.");
        }
    }
}
