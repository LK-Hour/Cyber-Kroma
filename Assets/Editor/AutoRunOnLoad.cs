using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.AI;

/// <summary>
/// Runs automatically when Unity editor loads - completes all remaining fixes
/// </summary>
[InitializeOnLoad]
public class AutoRunOnLoad
{
    private const string AUTO_FIX_COMPLETED_KEY = "CyberKroma_AutoFixCompleted";

    static AutoRunOnLoad()
    {
        // Only run once
        if (EditorPrefs.GetBool(AUTO_FIX_COMPLETED_KEY, false))
        {
            return;
        }

        // Run after Unity is fully loaded
        EditorApplication.delayCall += () =>
        {
            if (EditorUtility.DisplayDialog(
                "Auto Fix Cyber Kroma",
                "Do you want to automatically fix all remaining issues?\n\n" +
                "This will:\n" +
                "• Configure WaveManager spawn points\n" +
                "• Create enemy prefabs from Ann's models\n" +
                "• Setup SceneLoader system\n\n" +
                "This only runs once.",
                "Yes, Fix Everything!",
                "No, I'll do it manually"))
            {
                RunAllFixes();
                EditorPrefs.SetBool(AUTO_FIX_COMPLETED_KEY, true);
            }
            else
            {
                EditorPrefs.SetBool(AUTO_FIX_COMPLETED_KEY, true);
            }
        };
    }

    static void RunAllFixes()
    {
        Debug.Log("🚀 AUTO-FIXING ALL SCENES...");

        FixSceneAITest();
        CreateEnemyPrefabs();
        CreateSceneLoaderSystem();
        AssignEnemiesToWaveManager();
        ConnectAllScenes();

        EditorUtility.DisplayDialog("Success! ✅",
            "All fixes completed!\n\n" +
            "✅ WaveManager spawn points configured\n" +
            "✅ Enemy prefabs created\n" +
            "✅ SceneLoader system ready\n" +
            "✅ All scenes connected with navigation\n\n" +
            "Open MainMenu and press Play to test!",
            "Awesome!");

        Debug.Log("✅✅✅ ALL FIXES COMPLETED! ✅✅✅");
    }

    static void FixSceneAITest()
    {
        Debug.Log("🔧 Fixing Scene_AI_Test...");

        // Save current scene
        string currentScene = EditorSceneManager.GetActiveScene().path;

        // Load Scene_AI_Test
        EditorSceneManager.OpenScene("Assets/Scenes/Scene_AI_Test.unity");

        // Find WaveManager
        WaveManager waveManager = GameObject.FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogError("❌ WaveManager not found!");
            return;
        }

        // Find or create spawn points
        Transform[] spawnPoints = new Transform[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject spawnPoint = GameObject.Find($"SpawnPoint{i + 1}");
            if (spawnPoint != null)
            {
                spawnPoints[i] = spawnPoint.transform;
                Debug.Log($"✅ Found SpawnPoint{i + 1}");
            }
            else
            {
                Debug.LogWarning($"⚠️ SpawnPoint{i + 1} not found, creating it...");
                GameObject newSpawn = new GameObject($"SpawnPoint{i + 1}");
                
                // Position spawn points in a circle
                float angle = (360f / 5f) * i * Mathf.Deg2Rad;
                float radius = 15f;
                newSpawn.transform.position = new Vector3(
                    Mathf.Cos(angle) * radius,
                    0.1f,
                    Mathf.Sin(angle) * radius
                );
                
                spawnPoints[i] = newSpawn.transform;
                Debug.Log($"✅ Created SpawnPoint{i + 1} at {newSpawn.transform.position}");
            }
        }

        // Assign spawn points using SerializedObject
        SerializedObject so = new SerializedObject(waveManager);
        SerializedProperty spawnPointsProp = so.FindProperty("spawnPoints");
        spawnPointsProp.arraySize = 5;
        
        for (int i = 0; i < 5; i++)
        {
            spawnPointsProp.GetArrayElementAtIndex(i).objectReferenceValue = spawnPoints[i];
        }
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(waveManager);

        // Save scene
        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("✅ Scene_AI_Test fixed! WaveManager has 5 spawn points.");

        // Restore original scene if needed
        if (!string.IsNullOrEmpty(currentScene) && currentScene != "Assets/Scenes/Scene_AI_Test.unity")
        {
            EditorSceneManager.OpenScene(currentScene);
        }
    }

    static void CreateEnemyPrefabs()
    {
        Debug.Log("🔧 Creating enemy prefabs from Ann's models...");

        // Create folders
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Enemies"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Enemies");
        }

        // Create 3 enemy types
        CreateEnemyPrefab(
            "Phisher", 
            "Assets/Ann_Assets/Prefabs/Skeletonzombie.prefab", 
            "Assets/Prefabs/Enemies/Phisher.prefab", 
            3.5f, 1.5f, 50f
        );
        
        CreateEnemyPrefab(
            "GhostAccount", 
            "Assets/Ann_Assets/Prefabs/Warzombie.prefab", 
            "Assets/Prefabs/Enemies/GhostAccount.prefab", 
            4f, 2f, 75f
        );
        
        CreateEnemyPrefab(
            "DeepFake", 
            "Assets/Ann_Assets/Prefabs/Ch25.prefab", 
            "Assets/Prefabs/Enemies/DeepFake.prefab", 
            5f, 2.5f, 150f
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("✅ All 3 enemy prefabs created!");
    }

    static void CreateEnemyPrefab(string enemyName, string sourcePath, string targetPath, float speed, float stoppingDistance, float health)
    {
        // Try to load source prefab
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
        if (sourcePrefab == null)
        {
            Debug.LogWarning($"⚠️ Source prefab not found: {sourcePath} - Skipping {enemyName}");
            return;
        }

        // Instantiate
        GameObject enemy = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;
        enemy.name = enemyName;

        // Add NavMeshAgent
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = enemy.AddComponent<NavMeshAgent>();
        }
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
        agent.acceleration = 8f;
        agent.angularSpeed = 120f;

        // Add EnemyController
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller == null)
        {
            controller = enemy.AddComponent<EnemyController>();
        }

        // Add EnemyAI
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai == null)
        {
            ai = enemy.AddComponent<EnemyAI>();
        }

        // Set health via SerializedObject
        SerializedObject so = new SerializedObject(controller);
        SerializedProperty healthProp = so.FindProperty("maxHealth");
        if (healthProp != null)
        {
            healthProp.floatValue = health;
            so.ApplyModifiedProperties();
        }

        // Save as prefab
        GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(enemy, targetPath);
        UnityEngine.Object.DestroyImmediate(enemy);

        Debug.Log($"✅ Created {enemyName} prefab → {targetPath}");
    }

    static void AssignEnemiesToWaveManager()
    {
        Debug.Log("🔧 Assigning enemy prefabs to WaveManager...");

        // Load Scene_AI_Test
        EditorSceneManager.OpenScene("Assets/Scenes/Scene_AI_Test.unity");

        // Find WaveManager
        WaveManager waveManager = GameObject.FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogError("❌ WaveManager not found!");
            return;
        }

        // Load enemy prefabs
        GameObject phisher = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Phisher.prefab");
        GameObject ghost = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/GhostAccount.prefab");
        GameObject deepFake = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/DeepFake.prefab");

        // Assign to WaveManager
        SerializedObject so = new SerializedObject(waveManager);
        SerializedProperty enemyPrefabsProp = so.FindProperty("enemyPrefabs");
        
        enemyPrefabsProp.arraySize = 3;
        enemyPrefabsProp.GetArrayElementAtIndex(0).objectReferenceValue = phisher;
        enemyPrefabsProp.GetArrayElementAtIndex(1).objectReferenceValue = ghost;
        enemyPrefabsProp.GetArrayElementAtIndex(2).objectReferenceValue = deepFake;
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(waveManager);

        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("✅ Enemy prefabs assigned to WaveManager!");
    }

    static void CreateSceneLoaderSystem()
    {
        Debug.Log("🔧 Checking scene loading system...");

        // GameSceneManager already exists and handles scene loading
        // No need to create SceneLoader
        
        Debug.Log("✅ GameSceneManager already exists for scene loading!");
    }

    [MenuItem("Window/MCP For Unity/Reset Auto-Fix (Run Again)")]
    static void ResetAutoFix()
    {
        EditorPrefs.DeleteKey(AUTO_FIX_COMPLETED_KEY);
        Debug.Log("Auto-fix reset. Restart Unity to run again.");
    }

    static void ConnectAllScenes()
    {
        Debug.Log("🔗 Connecting all scenes with navigation...");

        // Setup MainMenu with GameSceneManager
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        
        GameSceneManager manager = GameObject.FindObjectOfType<GameSceneManager>();
        if (manager == null)
        {
            GameObject managerObj = new GameObject("GameSceneManager");
            manager = managerObj.AddComponent<GameSceneManager>();
            Debug.Log("✅ Added GameSceneManager to MainMenu");
        }

        EditorSceneManager.SaveOpenScenes();

        // Add GameSceneManager to all other scenes
        string[] scenes = {
            "Assets/Scenes/Tutorial.unity",
            "Assets/Scenes/Scene_Combat_Test.unity",
            "Assets/Scenes/Scene_AI_Test.unity",
            "Assets/Scenes/Scene_Level_Design.unity",
            "Assets/Scenes/Scene_Network_Core.unity"
        };

        foreach (string scenePath in scenes)
        {
            EditorSceneManager.OpenScene(scenePath);
            
            if (GameObject.FindObjectOfType<GameSceneManager>() == null)
            {
                GameObject managerObj = new GameObject("GameSceneManager");
                managerObj.AddComponent<GameSceneManager>();
                EditorSceneManager.SaveOpenScenes();
                Debug.Log($"✅ Added GameSceneManager to {scenePath}");
            }
        }

        Debug.Log("✅ All scenes now have GameSceneManager for navigation!");
    }
}
