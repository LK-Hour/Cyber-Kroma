using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages scene transitions and game flow
/// Flow: MainMenu -> Tutorial -> Combat -> Victory/Defeat -> MainMenu
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager instance;
    public static GameSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameSceneManager>();
            }
            return instance;
        }
    }

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string tutorialScene = "Tutorial";
    public string networkLobbyScene = "Scene_Network_Core";  // Multiplayer lobby
    public string gameplayScene = "Scene_Level_Design";      // Main gameplay
    public string combatTestScene = "Scene_Combat_Test";     // Testing only
    
    [Header("Transition Settings")]
    public float transitionDelay = 0.5f;
    public bool showLoadingScreen = true;

    void Awake()
    {
        // Singleton pattern - persist across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Public Scene Transition Methods
    
    /// <summary>
    /// Load Main Menu scene
    /// </summary>
    public void LoadMainMenu()
    {
        Debug.Log("🎮 Loading Main Menu...");
        StartCoroutine(LoadSceneAsync(mainMenuScene));
    }

    /// <summary>
    /// Load Tutorial scene
    /// </summary>
    public void LoadTutorial()
    {
        Debug.Log("📚 Loading Tutorial...");
        StartCoroutine(LoadSceneAsync(tutorialScene));
    }

    /// <summary>
    /// Load Network Lobby (multiplayer lobby for Host/Join)
    /// </summary>
    public void LoadNetworkLobby()
    {
        Debug.Log("🌐 Loading Network Lobby...");
        StartCoroutine(LoadSceneAsync(networkLobbyScene));
    }
    
    /// <summary>
    /// Load Main Gameplay scene (Scene_Level_Design)
    /// </summary>
    public void LoadGameplay()
    {
        Debug.Log("⚔️ Loading Main Gameplay...");
        StartCoroutine(LoadSceneAsync(gameplayScene));
    }
    
    /// <summary>
    /// Load Combat Test scene (for testing only)
    /// </summary>
    public void LoadCombatTest()
    {
        Debug.Log("🧪 Loading Combat Test Scene...");
        StartCoroutine(LoadSceneAsync(combatTestScene));
    }

    /// <summary>
    /// Reload current scene (for retry)
    /// </summary>
    public void ReloadCurrentScene()
    {
        Debug.Log("🔄 Reloading current scene...");
        string currentScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsync(currentScene));
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("👋 Quitting game...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    #endregion

    #region Scene Loading with Transition

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Optional: Show loading screen here
        if (showLoadingScreen)
        {
            // TODO: Enable loading screen UI
            Debug.Log("📊 Loading screen would appear here...");
        }

        // Small delay for smooth transition
        yield return new WaitForSeconds(transitionDelay);

        // Start loading scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            // Update loading bar here if needed
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log($"Loading: {progress * 100}%");
            yield return null;
        }

        Debug.Log($"✅ Scene '{sceneName}' loaded!");
    }

    #endregion

    #region Victory/Defeat Handlers

    /// <summary>
    /// Called when player wins (all waves completed)
    /// </summary>
    public void OnVictory()
    {
        Debug.Log("🏆 VICTORY! All waves completed!");
        
        // Show victory UI (handled by WaveManager)
        // Wait a few seconds, then return to main menu
        StartCoroutine(ReturnToMenuAfterDelay(5f));
    }

    /// <summary>
    /// Called when player loses (DataCore destroyed)
    /// </summary>
    public void OnDefeat()
    {
        Debug.Log("💀 DEFEAT! DataCore destroyed!");
        
        // Show defeat UI (handled by WaveManager)
        // Wait a few seconds, then return to main menu
        StartCoroutine(ReturnToMenuAfterDelay(5f));
    }

    IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadMainMenu();
    }

    #endregion
}
