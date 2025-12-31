
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine.EventSystems;
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

        // Step 6: Create player
        SetupPlayer();

        // Step 7: Verify player
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

                // Sample NavMesh to find valid position
                UnityEngine.AI.NavMeshHit hit;
                Vector3 spawnPos = spawnPositions[i];
                if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    spawnPoint.transform.position = hit.position;
                    Debug.Log($"  - Created {spawnName} at NavMesh position {hit.position} (original: {spawnPos})");
                }
                else
                {
                    spawnPoint.transform.position = spawnPos;
                    Debug.LogWarning($"  - Created {spawnName} at {spawnPos} (not on NavMesh - NavMeshAgent may fail)");
                }

                // Add visual marker
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.transform.parent = spawnPoint.transform;
                marker.transform.localPosition = Vector3.zero;
                marker.transform.localScale = Vector3.one * 0.5f;
                marker.GetComponent<Renderer>().material.color = Color.red;
                Destroy(marker.GetComponent<Collider>()); // Remove collider
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

        // Assign enemy prefabs to WaveManager
        waveManager.enemyPrefabs = new GameObject[3];
        if (phisher != null) waveManager.enemyPrefabs[0] = phisher;
        if (ghost != null) waveManager.enemyPrefabs[1] = ghost;
        if (deepFake != null) waveManager.enemyPrefabs[2] = deepFake;

        // Find spawn points
        GameObject[] spawnPointObjs = GameObject.FindGameObjectsWithTag("Untagged");
        Transform[] spawnPoints = new Transform[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject sp = GameObject.Find("SpawnPoint_" + (i + 1));
            if (sp != null) spawnPoints[i] = sp.transform;
        }

        // Assign spawn points to WaveManager
        waveManager.spawnPoints = spawnPoints;

        Debug.Log("  - WaveManager configured with enemy prefabs and spawn points");
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

        // Create control buttons
        CreateUIButton(combatUIObj.transform, "Btn_Shoot", new Vector2(0, -400), "SHOOT");
        CreateUIButton(combatUIObj.transform, "Btn_Reload", new Vector2(200, -400), "RELOAD");
        CreateUIButton(combatUIObj.transform, "Btn_Roll", new Vector2(-200, -400), "ROLL");

        // Add CombatSceneUI component
        if (combatUIObj.GetComponent<CombatSceneUI>() == null)
        {
            combatUIObj.AddComponent<CombatSceneUI>();
            Debug.Log("  - Added CombatSceneUI component");
        }

        // Add ButtonConnector to player (will be created later)
        // This will be handled when player is created

        // Create panels
        CreatePanel(canvas.transform, "VictoryPanel", "VICTORY!");
        CreatePanel(canvas.transform, "DefeatPanel", "DEFEAT");
        CreatePanel(canvas.transform, "ShopPanel", "SHOP");
        CreatePanel(canvas.transform, "PauseMenu", "PAUSED");

        Debug.Log("  - UI elements and buttons created");
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

    private void CreateUIButton(Transform parent, string name, Vector2 position, string text)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);

        RectTransform rt = buttonObj.AddComponent<RectTransform>();
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(150, 60);

        Image img = buttonObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 1f); // Blue button

        UnityEngine.UI.Button btn = buttonObj.AddComponent<UnityEngine.UI.Button>();
        ColorBlock colors = btn.colors;
        colors.highlightedColor = new Color(0.3f, 0.7f, 1.2f);
        colors.pressedColor = new Color(0.1f, 0.5f, 0.9f);
        btn.colors = colors;

        // Button text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.sizeDelta = Vector2.zero;

        // Add EventTrigger for shoot button
        if (name == "Btn_Shoot")
        {
            buttonObj.AddComponent<EventTrigger>();
        }
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

    private void SetupPlayer()
    {
        Debug.Log("[6/7] Setting up Player...");

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            // Create new Player
            player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, -10);
            player.transform.localScale = new Vector3(1, 2, 1);

            // Add CharacterController for movement
            var controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 1, 0);

            // Add required components
            if (player.GetComponent<CharacterHealth>() == null)
            {
                var health = player.AddComponent<CharacterHealth>();
                Debug.Log("  - Added CharacterHealth component");
            }

            if (player.GetComponent<CharacterShooting>() == null)
            {
                var shooting = player.AddComponent<CharacterShooting>();
                Debug.Log("  - Added CharacterShooting component");
            }

            if (player.GetComponent<CharacterMovement>() == null)
            {
                var movement = player.AddComponent<CharacterMovement>();
                Debug.Log("  - Added CharacterMovement component");
            }

            // Add camera
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.transform.SetParent(player.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.6f, 0);
            var camera = cameraObj.AddComponent<Camera>();
            camera.tag = "MainCamera";

            // Add audio listener
            cameraObj.AddComponent<AudioListener>();

            // Setup CharacterShooting references
            var shootingComp = player.GetComponent<CharacterShooting>();
            shootingComp.playerCamera = camera;
            shootingComp.firePoint = cameraObj.transform;

            // Create bullet trail prefab if it doesn't exist
            GameObject bulletTrail = GameObject.Find("BulletTrail");
            if (bulletTrail == null)
            {
                bulletTrail = new GameObject("BulletTrail");
                var lineRenderer = bulletTrail.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.yellow;
                lineRenderer.endColor = Color.red;
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
                bulletTrail.SetActive(false); // Make it a prefab
            }
            shootingComp.bulletTrailPrefab = bulletTrail.GetComponent<LineRenderer>();

            // Add ButtonConnector for UI controls
            if (player.GetComponent<ButtonConnector>() == null)
            {
                player.AddComponent<ButtonConnector>();
                Debug.Log("  - Added ButtonConnector component");
            }

            Debug.Log("  - Created Player with all components at (0, 1, -10)");
        }
        else
        {
            Debug.Log("  - Player already exists");
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
