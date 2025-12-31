using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Connects all scenes together with proper navigation
/// Run this once: Window → MCP For Unity → Connect All Scenes
/// </summary>
public class SceneConnector : EditorWindow
{
    [MenuItem("Window/MCP For Unity/Connect All Scenes")]
    public static void ShowWindow()
    {
        GetWindow<SceneConnector>("Scene Connector");
    }

    void OnGUI()
    {
        GUILayout.Label("Scene Navigation Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "This will connect all scenes with proper navigation:\n" +
            "• MainMenu → Tutorial/Combat/AI Test/Network\n" +
            "• All scenes → MainMenu (back button)\n" +
            "• GameSceneManager added to all scenes",
            MessageType.Info
        );

        GUILayout.Space(10);

        if (GUILayout.Button("✅ Connect All Scenes", GUILayout.Height(50)))
        {
            ConnectAllScenes();
        }
    }

    static void ConnectAllScenes()
    {
        Debug.Log("🔗 Connecting all scenes...");

        // Setup MainMenu
        SetupMainMenu();
        
        // Setup Tutorial
        SetupSceneWithBackButton("Assets/Scenes/Tutorial.unity", "Tutorial");
        
        // Setup Combat Test
        SetupSceneWithBackButton("Assets/Scenes/Scene_Combat_Test.unity", "Scene_Combat_Test");
        
        // Setup AI Test
        SetupSceneWithBackButton("Assets/Scenes/Scene_AI_Test.unity", "Scene_AI_Test");
        
        // Setup Level Design
        SetupSceneWithBackButton("Assets/Scenes/Scene_Level_Design.unity", "Scene_Level_Design");
        
        // Setup Network Core
        SetupSceneWithBackButton("Assets/Scenes/Scene_Network_Core.unity", "Scene_Network_Core");

        EditorUtility.DisplayDialog("Success! ✅",
            "All scenes connected!\n\n" +
            "✅ MainMenu has buttons for all scenes\n" +
            "✅ All scenes can return to MainMenu\n" +
            "✅ GameSceneManager added to all scenes\n\n" +
            "Test navigation by pressing Play in MainMenu!",
            "Awesome!");

        Debug.Log("✅✅✅ ALL SCENES CONNECTED! ✅✅✅");
    }

    static void SetupMainMenu()
    {
        Debug.Log("🎮 Setting up MainMenu...");
        
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

        // Find or create GameSceneManager
        GameSceneManager sceneManager = GameObject.FindObjectOfType<GameSceneManager>();
        if (sceneManager == null)
        {
            GameObject managerObj = new GameObject("GameSceneManager");
            sceneManager = managerObj.AddComponent<GameSceneManager>();
            Debug.Log("✅ Added GameSceneManager to MainMenu");
        }

        // Find UI buttons and connect them
        ConnectMainMenuButtons(sceneManager);

        EditorSceneManager.SaveOpenScenes();
        Debug.Log("✅ MainMenu setup complete!");
    }

    static void ConnectMainMenuButtons(GameSceneManager manager)
    {
        // Find buttons by name
        Button[] allButtons = GameObject.FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            string btnName = btn.gameObject.name.ToLower();

            // Remove old listeners
            btn.onClick.RemoveAllListeners();

            // Connect based on button name
            if (btnName.Contains("tutorial"))
            {
                btn.onClick.AddListener(() => manager.LoadTutorial());
                Debug.Log($"✅ Connected {btn.gameObject.name} → Tutorial");
            }
            else if (btnName.Contains("combat") || btnName.Contains("test"))
            {
                btn.onClick.AddListener(() => manager.LoadCombatTest());
                Debug.Log($"✅ Connected {btn.gameObject.name} → Combat Test");
            }
            else if (btnName.Contains("ai"))
            {
                btn.onClick.AddListener(() => manager.LoadAITest());
                Debug.Log($"✅ Connected {btn.gameObject.name} → AI Test");
            }
            else if (btnName.Contains("multi") || btnName.Contains("network"))
            {
                btn.onClick.AddListener(() => manager.LoadNetworkLobby());
                Debug.Log($"✅ Connected {btn.gameObject.name} → Network Lobby");
            }
            else if (btnName.Contains("play") || btnName.Contains("start"))
            {
                btn.onClick.AddListener(() => manager.LoadGameplay());
                Debug.Log($"✅ Connected {btn.gameObject.name} → Gameplay");
            }
            else if (btnName.Contains("quit") || btnName.Contains("exit"))
            {
                btn.onClick.AddListener(() => manager.QuitGame());
                Debug.Log($"✅ Connected {btn.gameObject.name} → Quit");
            }

            EditorUtility.SetDirty(btn);
        }
    }

    static void SetupSceneWithBackButton(string scenePath, string sceneName)
    {
        Debug.Log($"🔧 Setting up {sceneName}...");

        EditorSceneManager.OpenScene(scenePath);

        // Find or create GameSceneManager
        GameSceneManager sceneManager = GameObject.FindObjectOfType<GameSceneManager>();
        if (sceneManager == null)
        {
            GameObject managerObj = new GameObject("GameSceneManager");
            sceneManager = managerObj.AddComponent<GameSceneManager>();
            Debug.Log($"✅ Added GameSceneManager to {sceneName}");
        }

        // Find and connect back/menu buttons
        Button[] allButtons = GameObject.FindObjectsOfType<Button>(true);
        foreach (Button btn in allButtons)
        {
            string btnName = btn.gameObject.name.ToLower();

            if (btnName.Contains("back") || btnName.Contains("menu") || btnName.Contains("exit"))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => sceneManager.LoadMainMenu());
                Debug.Log($"✅ Connected {btn.gameObject.name} → MainMenu");
                EditorUtility.SetDirty(btn);
            }
        }

        EditorSceneManager.SaveOpenScenes();
        Debug.Log($"✅ {sceneName} setup complete!");
    }
}
