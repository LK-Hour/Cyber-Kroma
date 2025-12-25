using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Automatically links all component references in the scene
/// Run this once to connect WaveManager, Shop, Education UI, etc.
/// Add to any GameObject and it will link everything on Start()
/// </summary>
public class ReferenceLinker : MonoBehaviour
{
    void Start()
    {
        LinkAllReferences();
    }
    
    void LinkAllReferences()
    {
        Debug.Log("🔗 Linking all component references...");
        
        LinkWaveManagerReferences();
        LinkLokTaShopReferences();
        LinkScamEducationUIReferences();
        LinkPlayerPointsReferences();
        LinkDataCoreHealthReferences();
        SetPlayerTag();
        
        Debug.Log("✅ All references linked successfully!");
        
        // Disable this script after running once
        this.enabled = false;
    }
    
    #region WaveManager References
    void LinkWaveManagerReferences()
    {
        GameObject waveManagerObj = GameObject.Find("WaveManager");
        if (waveManagerObj == null)
        {
            Debug.LogWarning("WaveManager not found!");
            return;
        }
        
        WaveManager waveManager = waveManagerObj.GetComponent<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogWarning("WaveManager component not found!");
            return;
        }
        
        // Link spawn points
        GameObject[] spawnPoints = new GameObject[5];
        spawnPoints[0] = GameObject.Find("SpawnPoint1");
        spawnPoints[1] = GameObject.Find("SpawnPoint2");
        spawnPoints[2] = GameObject.Find("SpawnPoint3");
        spawnPoints[3] = GameObject.Find("SpawnPoint4");
        spawnPoints[4] = GameObject.Find("SpawnPoint5");
        
        Transform[] spawnTransforms = new Transform[5];
        for (int i = 0; i < 5; i++)
        {
            if (spawnPoints[i] != null)
            {
                spawnTransforms[i] = spawnPoints[i].transform;
            }
        }
        waveManager.spawnPoints = spawnTransforms;
        
        // Link enemy prefabs (from scene enemies - will need to save as prefabs later)
        GameObject phisher = GameObject.Find("Phisher");
        GameObject ghost = GameObject.Find("GhostAccount");
        GameObject deepFake = GameObject.Find("DeepFake");
        
        if (phisher != null) waveManager.phisherPrefab = phisher;
        if (ghost != null) waveManager.ghostAccountPrefab = ghost;
        if (deepFake != null) waveManager.deepFakePrefab = deepFake;
        
        // Link DataCore
        GameObject dataCore = GameObject.Find("DataCore");
        if (dataCore != null)
        {
            waveManager.dataCore = dataCore.GetComponent<DataCoreHealth>();
        }
        
        // Link Education UI
        GameObject educationManager = GameObject.Find("EducationManager");
        if (educationManager != null)
        {
            waveManager.educationUI = educationManager.GetComponent<ScamEducationUI>();
        }
        
        // Link UI panels
        GameObject shopPanel = GameObject.Find("Canvas/ShopPanel");
        GameObject victoryPanel = GameObject.Find("Canvas/VictoryPanel");
        GameObject defeatPanel = GameObject.Find("Canvas/DefeatPanel");
        GameObject waveText = GameObject.Find("Canvas/HUD/WaveText");
        GameObject enemyCountText = GameObject.Find("Canvas/HUD/EnemyCountText");
        
        if (shopPanel != null) waveManager.shopPanel = shopPanel;
        if (victoryPanel != null) waveManager.victoryPanel = victoryPanel;
        if (defeatPanel != null) waveManager.defeatPanel = defeatPanel;
        if (waveText != null) waveManager.waveText = waveText.GetComponent<TextMeshProUGUI>();
        if (enemyCountText != null) waveManager.enemyCountText = enemyCountText.GetComponent<TextMeshProUGUI>();
        
        Debug.Log("✅ WaveManager references linked");
    }
    #endregion
    
    #region LokTaShop References
    void LinkLokTaShopReferences()
    {
        GameObject shopObj = GameObject.Find("LokTaShop");
        if (shopObj == null)
        {
            Debug.LogWarning("LokTaShop not found!");
            return;
        }
        
        LokTaShop shop = shopObj.GetComponent<LokTaShop>();
        if (shop == null)
        {
            Debug.LogWarning("LokTaShop component not found!");
            return;
        }
        
        // Link shop panel
        GameObject shopPanel = GameObject.Find("Canvas/ShopPanel");
        if (shopPanel != null)
        {
            shop.shopPanel = shopPanel;
        }
        
        // Link recommendation text
        GameObject recText = GameObject.Find("Canvas/ShopPanel/RecommendationText");
        if (recText != null)
        {
            shop.recommendationText = recText.GetComponent<TextMeshProUGUI>();
        }
        
        // Link buttons
        GameObject btnHealth = GameObject.Find("Canvas/ShopPanel/BtnHealth");
        GameObject btnShield = GameObject.Find("Canvas/ShopPanel/BtnShield");
        GameObject btnAmmo = GameObject.Find("Canvas/ShopPanel/BtnAmmo");
        
        if (btnHealth != null) shop.btnHealth = btnHealth.GetComponent<Button>();
        if (btnShield != null) shop.btnShield = btnShield.GetComponent<Button>();
        if (btnAmmo != null) shop.btnAmmo = btnAmmo.GetComponent<Button>();
        
        // Link player
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            shop.player = player;
            shop.playerShooting = player.GetComponent<CharacterShooting>();
            shop.playerHealth = player.GetComponent<CharacterHealth>();
        }
        
        // Link PlayerPoints
        GameObject playerPoints = GameObject.Find("PlayerPoints");
        if (playerPoints != null)
        {
            shop.playerPoints = playerPoints.GetComponent<PlayerPoints>();
        }
        
        Debug.Log("✅ LokTaShop references linked");
    }
    #endregion
    
    #region ScamEducationUI References
    void LinkScamEducationUIReferences()
    {
        GameObject eduManager = GameObject.Find("EducationManager");
        if (eduManager == null)
        {
            Debug.LogWarning("EducationManager not found!");
            return;
        }
        
        ScamEducationUI eduUI = eduManager.GetComponent<ScamEducationUI>();
        if (eduUI == null)
        {
            Debug.LogWarning("ScamEducationUI component not found!");
            return;
        }
        
        // Link education panel
        GameObject eduPanel = GameObject.Find("Canvas/EducationPanel");
        if (eduPanel != null)
        {
            eduUI.educationPanel = eduPanel;
        }
        
        // Link text elements
        GameObject titleText = GameObject.Find("Canvas/EducationPanel/EducationTitle");
        GameObject descText = GameObject.Find("Canvas/EducationPanel/DescriptionText");
        GameObject scamIcon = GameObject.Find("Canvas/EducationPanel/ScamIcon");
        GameObject btnClose = GameObject.Find("Canvas/EducationPanel/BtnCloseEducation");
        
        if (titleText != null) eduUI.titleText = titleText.GetComponent<TextMeshProUGUI>();
        if (descText != null) eduUI.descriptionText = descText.GetComponent<TextMeshProUGUI>();
        if (scamIcon != null) eduUI.scamIcon = scamIcon.GetComponent<Image>();
        if (btnClose != null) eduUI.btnClose = btnClose.GetComponent<Button>();
        
        Debug.Log("✅ ScamEducationUI references linked");
    }
    #endregion
    
    #region PlayerPoints References
    void LinkPlayerPointsReferences()
    {
        GameObject playerPointsObj = GameObject.Find("PlayerPoints");
        if (playerPointsObj == null)
        {
            Debug.LogWarning("PlayerPoints not found!");
            return;
        }
        
        PlayerPoints playerPoints = playerPointsObj.GetComponent<PlayerPoints>();
        if (playerPoints == null)
        {
            Debug.LogWarning("PlayerPoints component not found!");
            return;
        }
        
        // Link points text
        GameObject pointsText = GameObject.Find("Canvas/HUD/PointsText");
        if (pointsText != null)
        {
            playerPoints.pointsText = pointsText.GetComponent<TextMeshProUGUI>();
        }
        
        Debug.Log("✅ PlayerPoints references linked");
    }
    #endregion
    
    #region DataCoreHealth References
    void LinkDataCoreHealthReferences()
    {
        GameObject dataCoreObj = GameObject.Find("DataCore");
        if (dataCoreObj == null)
        {
            Debug.LogWarning("DataCore not found!");
            return;
        }
        
        DataCoreHealth dataCoreHealth = dataCoreObj.GetComponent<DataCoreHealth>();
        if (dataCoreHealth == null)
        {
            Debug.LogWarning("DataCoreHealth component not found!");
            return;
        }
        
        // Link material (try to find DataCoreMaterial)
        Material dataMaterial = Resources.Load<Material>("DataCoreMaterial");
        if (dataMaterial == null)
        {
            // Try alternate path
            dataMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/DataCoreMaterial.mat");
        }
        
        if (dataMaterial != null)
        {
            dataCoreHealth.coreMaterial = dataMaterial;
        }
        else
        {
            Debug.LogWarning("DataCoreMaterial not found in Resources or Assets/Materials/");
        }
        
        Debug.Log("✅ DataCoreHealth references linked");
    }
    #endregion
    
    #region Helper Methods
    void SetPlayerTag()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            player.tag = "Player";
            Debug.Log("✅ Player tag set to 'Player'");
        }
        else
        {
            Debug.LogWarning("Player GameObject not found - tag not set!");
        }
    }
    #endregion
}
