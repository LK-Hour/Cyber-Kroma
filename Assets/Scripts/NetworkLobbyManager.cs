using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Network Lobby Manager - Handles multiplayer lobby for up to 4 players
/// Allows Host/Join via LAN or IP, class selection (Firewall, Debugger, Scanner)
/// Uses Netcode for GameObjects (NGO)
/// </summary>
public class NetworkLobbyManager : MonoBehaviour
{
    [Header("Lobby UI")]
    public GameObject lobbyPanel;
    public TextMeshProUGUI lobbyTitle;
    public TextMeshProUGUI playerCountText;
    public Button hostButton;
    public Button joinButton;
    public Button startGameButton;
    public TMP_InputField ipAddressInput;
    
    [Header("Class Selection UI")]
    public GameObject classSelectionPanel;
    public Button firewallButton;
    public Button debuggerButton;
    public Button scannerButton;
    public TextMeshProUGUI selectedClassText;
    
    [Header("Player List")]
    public Transform playerListContainer;
    public GameObject playerListItemPrefab;
    
    [Header("Settings")]
    public int maxPlayers = 4;
    public string gameplayScene = "Scene_Level_Design";
    
    private NetworkManager networkManager;
    private string selectedClass = "Firewall"; // Default class
    
    void Start()
    {
        networkManager = NetworkManager.Singleton;
        
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager not found! Add NetworkManager to the scene.");
            return;
        }
        
        SetupUI();
        SetupButtons();
    }
    
    void SetupUI()
    {
        // Lobby Title
        if (lobbyTitle != null)
        {
            lobbyTitle.text = "🌐 CYBER KROMA LOBBY\n<size=28>Up to 4 Players Co-op</size>";
            lobbyTitle.fontSize = 48;
            lobbyTitle.alignment = TextAlignmentOptions.Center;
            lobbyTitle.color = new Color(0f, 0.9f, 1f); // Cyan
        }
        
        // IP Input placeholder
        if (ipAddressInput != null)
        {
            ipAddressInput.text = "127.0.0.1"; // Default localhost
            ipAddressInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter IP Address...";
        }
        
        // Start button initially disabled (only host can start)
        if (startGameButton != null)
        {
            startGameButton.interactable = false;
        }
        
        // Show lobby panel, hide class selection
        if (lobbyPanel != null) lobbyPanel.SetActive(true);
        if (classSelectionPanel != null) classSelectionPanel.SetActive(false);
    }
    
    void SetupButtons()
    {
        // Host Button - Start server and become host
        if (hostButton != null)
        {
            hostButton.onClick.RemoveAllListeners();
            hostButton.onClick.AddListener(OnHostClicked);
            
            TextMeshProUGUI btnText = hostButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "🏠 HOST GAME";
                btnText.fontSize = 32;
            }
        }
        
        // Join Button - Connect to host
        if (joinButton != null)
        {
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(OnJoinClicked);
            
            TextMeshProUGUI btnText = joinButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "🔗 JOIN GAME";
                btnText.fontSize = 32;
            }
        }
        
        // Start Game Button - Only visible to host
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(OnStartGameClicked);
            
            TextMeshProUGUI btnText = startGameButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "🚀 START GAME";
                btnText.fontSize = 36;
            }
        }
        
        // Class Selection Buttons
        SetupClassButton(firewallButton, "Firewall", "🛡️ FIREWALL\n<size=20>High Defense</size>");
        SetupClassButton(debuggerButton, "Debugger", "🔧 DEBUGGER\n<size=20>High Damage</size>");
        SetupClassButton(scannerButton, "Scanner", "🔍 SCANNER\n<size=20>Detect Stealth</size>");
    }
    
    void SetupClassButton(Button button, string className, string displayText)
    {
        if (button == null) return;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClassSelected(className));
        
        TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            btnText.text = displayText;
            btnText.fontSize = 28;
        }
    }
    
    #region Button Handlers
    
    void OnHostClicked()
    {
        Debug.Log("🏠 Starting as Host...");
        
        if (networkManager != null)
        {
            networkManager.StartHost();
            
            // Enable start button for host
            if (startGameButton != null)
            {
                startGameButton.interactable = true;
            }
            
            // Show class selection
            ShowClassSelection();
            
            UpdatePlayerCount();
        }
    }
    
    void OnJoinClicked()
    {
        Debug.Log("🔗 Joining game...");
        
        if (networkManager != null && ipAddressInput != null)
        {
            // Get IP from input field
            string ipAddress = ipAddressInput.text;
            
            // Set connection data
            var transport = networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            if (transport != null)
            {
                transport.SetConnectionData(ipAddress, 7777); // Default port 7777
            }
            
            networkManager.StartClient();
            
            // Show class selection
            ShowClassSelection();
            
            UpdatePlayerCount();
        }
    }
    
    void OnStartGameClicked()
    {
        Debug.Log("🚀 Starting game...");
        
        if (networkManager != null && networkManager.IsHost)
        {
            // Load gameplay scene for all clients
            networkManager.SceneManager.LoadScene(gameplayScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("Only the host can start the game!");
        }
    }
    
    void OnClassSelected(string className)
    {
        selectedClass = className;
        Debug.Log($"✅ Selected class: {className}");
        
        if (selectedClassText != null)
        {
            selectedClassText.text = $"Selected: <color=#FFD700>{className}</color>";
        }
        
        // TODO: Send class selection to server via RPC
        // This will sync the player's class choice across the network
    }
    
    #endregion
    
    void ShowClassSelection()
    {
        if (classSelectionPanel != null)
        {
            classSelectionPanel.SetActive(true);
        }
        
        // Disable host/join buttons after joining
        if (hostButton != null) hostButton.interactable = false;
        if (joinButton != null) joinButton.interactable = false;
    }
    
    void UpdatePlayerCount()
    {
        if (playerCountText != null && networkManager != null)
        {
            int connectedPlayers = networkManager.ConnectedClients.Count;
            playerCountText.text = $"Players: {connectedPlayers}/{maxPlayers}";
            
            // Change color based on player count
            if (connectedPlayers >= maxPlayers)
            {
                playerCountText.color = Color.green; // Full lobby
            }
            else if (connectedPlayers >= 2)
            {
                playerCountText.color = Color.yellow; // Ready to start
            }
            else
            {
                playerCountText.color = Color.white; // Waiting for players
            }
        }
    }
    
    void Update()
    {
        // Continuously update player count
        if (networkManager != null && networkManager.IsListening)
        {
            UpdatePlayerCount();
        }
    }
}
