
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.AI.Navigation;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Automated setup script for Scene_Combat_Test
/// Run this ONCE from Unity menu: Tools > Setup Combat Scene
/// </summary>
public class CombatSceneSetup : MonoBehaviour
{
    [Header("Auto-Setup Configuration")]
    [Tooltip("Run this script to auto-configure Scene_Combat_Test")]
    public bool autoSetupOnStart = false;

    [Header("References to Find")]
    public WaveManager waveManager;
    public GameObject dataCore;
    public GameObject player;
    public Canvas canvas;

    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupCombatScene();
        }
    }

    [ContextMenu("Setup Combat Scene")]
    public void SetupCombatScene()
    {
        Debug.Log("=== COMBAT SCENE SETUP STARTED ===");

        // Step 1: Find or create DataCore
        SetupDataCore();

        // Step 2: Create spawn points
        SetupSpawnPoints();

        // Step 3: Setup WaveManager with prefabs
        SetupWaveManager();

        // Step 4: Create Combat UI
        SetupCombatUI();

        // Step 5: Setup NavMesh
        SetupNavMesh();

        // Step 6: Verify player
        VerifyPlayer();

        Debug.Log("=== COMBAT SCENE SETUP COMPLETE ===");
        Debug.Log("Next step: Select Ground plane and bake NavMesh (Window > AI > Navigation)");
    }

    private void SetupDataCore()
    {
        Debug.Log("[1/6] Setting up DataCore...");

        dataCore = GameObject.FindGameObjectWithTag("DataCore");
        if (dataCore == null)
        {
            // Create new DataCore
            dataCore = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dataCore.name = "DataCore";
            dataCore.tag = "DataCore";
            dataCore.transform.position = new Vector3(0, 1, 0);
            dataCore.transform.localScale = new Vector3(2, 2, 2);

            // Add DataCoreHealth if it exists
            if (dataCore.GetComponent<DataCoreHealth>() == null)
            {
                var coreHealth = dataCore.AddComponent<DataCoreHealth>();
                Debug.Log("  - Added DataCoreHealth component");
            }

            // Make it glow
            var renderer = dataCore.GetComponent<Renderer>();
            if (renderer != null)
            {
                var mat = new Material(Shader.Find("Standard"));
                mat.color = Color.cyan;
                mat.SetFloat("_Metallic", 0.5f);
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.cyan * 0.5f);
                renderer.material = mat;
            }

            Debug.Log("  - Created DataCore at center (0, 1, 0)");
        }
        else
        {
            Debug.Log("  - DataCore already exists");
        }
    }

    private void SetupSpawnPoints()
    {
        Debug.Log("[2/6] Setting up spawn points...");

        GameObject spawnPointsParent = GameObject.Find("SpawnPoints");
        if (spawnPointsParent == null)
        {
            spawnPointsParent = new GameObject("SpawnPoints");
        }

        Vector3[] spawnPositions = new Vector3[]
        {
            new Vector3(-10, 0.5f, 10),
            new Vector3(10, 0.5f, 10),
            new Vector3(-10, 0.5f, -10),
            new Vector3(10, 0.5f, -10),
            new Vector3(0, 0.5f, 15)
        };

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            string spawnName = "SpawnPoint_" + (i + 1);
            GameObject spawnPoint = GameObject.Find(spawnName);
            
            if (spawnPoint == null)
            {
                spawnPoint = new GameObject(spawnName);
                spawnPoint.transform.parent = spawnPointsParent.transform;
                spawnPoint.transform.position = spawnPositions[i];

                // Add visual marker
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.transform.parent = spawnPoint.transform;
                marker.transform.localPosition = Vector3.zero;
                marker.transform.localScale = Vector3.one * 0.5f;
                marker.GetComponent<Renderer>().material.color = Color.red;
                Destroy(marker.GetComponent<Collider>()); // Remove collider

                Debug.Log($"  - Created {spawnName} at {spawnPositions[i]}");
            }
        }
    }

    private void SetupWaveManager()
    {
        Debug.Log("[3/6] Setting up WaveManager...");

        waveManager = FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            GameObject wmObj = new GameObject("WaveManager");
            waveManager = wmObj.AddComponent<WaveManager>();
            Debug.Log("  - Created WaveManager GameObject");
        }

        // Load enemy prefabs
        GameObject phisher = Resources.Load<GameObject>("Prefabs/Enemies/Phisher");
        GameObject ghost = Resources.Load<GameObject>("Prefabs/Enemies/GhostAccount");
        GameObject deepFake = Resources.Load<GameObject>("Prefabs/Enemies/DeepFake");

        Debug.Log($"  - Loaded Phisher: {(phisher != null ? "OK" : "MISSING")}");
        Debug.Log($"  - Loaded GhostAccount: {(ghost != null ? "OK" : "MISSING")}");
        Debug.Log($"  - Loaded DeepFake: {(deepFake != null ? "OK" : "MISSING")}");

        // Find spawn points
        GameObject[] spawnPointObjs = GameObject.FindGameObjectsWithTag("Untagged");
        Transform[] spawnPoints = new Transform[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject sp = GameObject.Find("SpawnPoint_" + (i + 1));
            if (sp != null) spawnPoints[i] = sp.transform;
        }

        Debug.Log("  - WaveManager configured (assign enemy prefabs manually in Inspector)");
        Debug.Log("  - Assign spawn points array in Inspector");
    }

    private void SetupCombatUI()
    {
        Debug.Log("[4/6] Setting up Combat UI...");

        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            Debug.Log("  - Created Canvas");
        }

        // Create CombatUI GameObject
        GameObject combatUIObj = new GameObject("CombatUI");
        combatUIObj.transform.SetParent(canvas.transform, false);

        // Create UI elements
        CreateUIText(combatUIObj.transform, "HealthText", new Vector2(-800, 500), "HP: 100/100");
        CreateUIText(combatUIObj.transform, "AmmoText", new Vector2(-800, 450), "AMMO: 30/120");
        CreateUIText(combatUIObj.transform, "WaveText", new Vector2(700, 500), "WAVE 1/3");
        CreateUIText(combatUIObj.transform, "EnemiesText", new Vector2(700, 450), "ENEMIES: 0");
        CreateUIText(combatUIObj.transform, "KillsText", new Vector2(700, 400), "KILLS: 0");
        CreateUIText(combatUIObj.transform, "PointsText", new Vector2(700, 350), "POINTS: 0");

        // Add CombatSceneUI component
        if (combatUIObj.GetComponent<CombatSceneUI>() == null)
        {
            combatUIObj.AddComponent<CombatSceneUI>();
            Debug.Log("  - Added CombatSceneUI component");
        }

        // Create panels
        CreatePanel(canvas.transform, "VictoryPanel", "VICTORY!");
        CreatePanel(canvas.transform, "DefeatPanel", "DEFEAT");

        Debug.Log("  - UI elements created (assign references in CombatSceneUI)");
    }

    private void CreateUIText(Transform parent, string name, Vector2 position, string text)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rt = textObj.AddComponent<RectTransform>();
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(300, 50);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 24;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Left;
    }

    private void CreatePanel(Transform parent, string name, string titleText)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);

        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        Image img = panel.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.8f);

        // Title text
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panel.transform, false);
        TextMeshProUGUI title = titleObj.AddComponent<TextMeshProUGUI>();
        title.text = titleText;
        title.fontSize = 72;
        title.alignment = TextAlignmentOptions.Center;
        title.color = Color.white;

        RectTransform titleRT = titleObj.GetComponent<RectTransform>();
        titleRT.anchoredPosition = new Vector2(0, 100);
        titleRT.sizeDelta = new Vector2(800, 150);

        panel.SetActive(false);
    }

    private void SetupNavMesh()
    {
        Debug.Log("[5/6] Checking NavMesh setup...");

        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            Debug.LogWarning("  - No GameObject named 'Ground' found");
            Debug.LogWarning("  - You need to manually add NavMeshSurface to your ground plane");
        }
        else
        {
            NavMeshSurface surface = ground.GetComponent<NavMeshSurface>();
            if (surface == null)
            {
                surface = ground.AddComponent<NavMeshSurface>();
                Debug.Log("  - Added NavMeshSurface to Ground");
                Debug.LogWarning("  - IMPORTANT: Select Ground and click 'Bake' in NavMeshSurface component!");
            }
            else
            {
                Debug.Log("  - NavMeshSurface already exists");
            }
        }
    }

    private void VerifyPlayer()
    {
        Debug.Log("[6/6] Verifying player setup...");

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("  - No Player found! Player tag not assigned or player doesn't exist");
        }
        else
        {
            Debug.Log("  - Player found: " + player.name);

            if (player.GetComponent<CharacterHealth>() == null)
                Debug.LogWarning("  - Player missing CharacterHealth component!");

            if (player.GetComponent<CharacterShooting>() == null)
                Debug.LogWarning("  - Player missing CharacterShooting component!");

            if (player.GetComponent<CharacterMovement>() == null)
                Debug.LogWarning("  - Player missing CharacterMovement component!");
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CombatSceneSetup))]
public class CombatSceneSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CombatSceneSetup setup = (CombatSceneSetup)target;

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Click the button below to auto-setup Scene_Combat_Test", MessageType.Info);

        if (GUILayout.Button("Setup Combat Scene", GUILayout.Height(40)))
        {
            setup.SetupCombatScene();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("After running setup:\n1. Select Ground and bake NavMesh\n2. Assign enemy prefabs to WaveManager\n3. Assign UI references in CombatSceneUI", MessageType.Warning);
    }
}
#endif
