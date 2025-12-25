using UnityEngine;

/// <summary>
/// Master configurator - runs all configuration scripts on scene load
/// Add this to Canvas or any GameObject in the scene
/// It will automatically configure UI, enemies, and references
/// </summary>
public class MasterSceneConfigurator : MonoBehaviour
{
    [Header("Auto-Configuration")]
    public bool configureUIOnStart = true;
    public bool configureEnemiesOnStart = true;
    public bool linkReferencesOnStart = true;
    
    void Start()
    {
        Debug.Log("🚀 Master Scene Configurator starting...");
        
        if (configureUIOnStart)
        {
            ConfigureUI();
        }
        
        if (configureEnemiesOnStart)
        {
            ConfigureEnemies();
        }
        
        if (linkReferencesOnStart)
        {
            LinkReferences();
        }
        
        Debug.Log("✅ Master Scene Configuration complete!");
        Debug.Log("🎮 Ready to play! Press Play button to start!");
        
        // Disable after running once
        this.enabled = false;
    }
    
    void ConfigureUI()
    {
        UIAutoConfigurator uiConfig = GetComponent<UIAutoConfigurator>();
        if (uiConfig == null)
        {
            uiConfig = gameObject.AddComponent<UIAutoConfigurator>();
        }
        // UIAutoConfigurator runs in its own Start()
    }
    
    void ConfigureEnemies()
    {
        EnemyPrefabConfigurator enemyConfig = GetComponent<EnemyPrefabConfigurator>();
        if (enemyConfig == null)
        {
            enemyConfig = gameObject.AddComponent<EnemyPrefabConfigurator>();
        }
        // EnemyPrefabConfigurator runs in its own Start()
    }
    
    void LinkReferences()
    {
        ReferenceLinker linker = GetComponent<ReferenceLinker>();
        if (linker == null)
        {
            linker = gameObject.AddComponent<ReferenceLinker>();
        }
        // ReferenceLinker runs in its own Start()
    }
}
