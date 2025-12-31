using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using System;

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
    public TextMeshProUGUI lobbyCodeText;  // Shows the generated lobby code
    public TextMeshProUGUI statusText;     // Shows connection status
    public TextMeshProUGUI playerCountText;
    public Button hostButton;
    public Button joinButton;
    public Button startGameButton;
    public TMP_InputField joinCodeInput;  // Input for entering join code
    
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
    public string gameplayScene = "Scene_Combat_Test"; // Change this to your main gameplay scene
    public bool useRelay = false; // Toggle between Relay and local network
    public ushort localPort = 7777;
    
    [Header("Player Prefabs")]
    public GameObject firewallPrefab;   // High defense tank
    public GameObject debuggerPrefab;   // High damage DPS
    public GameObject scannerPrefab;    // Detection specialist
    
    private NetworkManager networkManager;
    private string selectedClass = "Firewall"; // Default class
    private string currentLobbyCode = "";
    private bool isInitialized = false;
    
    // Store player class choices (NetworkVariable would be better for multiplayer)
    private static string localPlayerClass = "Firewall";
    
    async void Start()
    {
        networkManager = NetworkManager.Singleton;
        
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager not found! Add NetworkManager to the scene.");
            return;
        }
        
        // Only initialize Unity Services if using Relay
        if (useRelay)
        {
            await InitializeUnityServices();
        }
        else
        {
            isInitialized = true; // Skip Unity Services for local network
            Debug.Log("Using local network mode (no Unity Services needed)");
        }
        
        // Auto-find UI elements if not assigned
        AutoFindUIElements();
        
        SetupUI();
        SetupButtons();
    }
    
    async Task InitializeUnityServices()
    {
        try
        {
            if (statusText != null)
            {
                statusText.text = "Initializing services...";
            }
            
            Debug.Log("Starting Unity Services initialization...");
            
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services initialized");
            }
            
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in as: {AuthenticationService.Instance.PlayerId}");
            }
            
            isInitialized = true;
            
            if (statusText != null)
            {
                statusText.text = "Ready to host or join";
            }
            
            Debug.Log("Unity Services ready for Relay");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}\n{e.StackTrace}");
            isInitialized = false;
            
            if (statusText != null)
            {
                statusText.text = $"<color=#FF0000>Init failed: {e.Message}</color>";
            }
        }
    }
    
    void AutoFindUIElements()
    {
        // Auto-find class selection buttons by name
        if (firewallButton == null)
        {
            GameObject btn = GameObject.Find("Btn_Firewall");
            if (btn != null) firewallButton = btn.GetComponent<Button>();
        }
        
        if (debuggerButton == null)
        {
            GameObject btn = GameObject.Find("Btn_Debugger");
            if (btn != null) debuggerButton = btn.GetComponent<Button>();
        }
        
        if (scannerButton == null)
        {
            GameObject btn = GameObject.Find("Btn_Scanner");
            if (btn != null) scannerButton = btn.GetComponent<Button>();
        }
        
        // Auto-find panels
        if (classSelectionPanel == null)
        {
            GameObject panel = GameObject.Find("ClassSelectionPanel");
            if (panel != null) classSelectionPanel = panel;
        }
        
        // Auto-find selected class text
        if (selectedClassText == null)
        {
            GameObject textObj = GameObject.Find("SelectedClassText");
            if (textObj != null) selectedClassText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        Debug.Log($"Auto-find results: Firewall={firewallButton != null}, Debugger={debuggerButton != null}, Scanner={scannerButton != null}");
    }
    
    void SetupUI()
    {
        // Lobby Title
        if (lobbyTitle != null)
        {
            lobbyTitle.text = "CYBER KROMA LOBBY\n<size=28>Up to 4 Players Co-op</size>";
            lobbyTitle.fontSize = 80;
            lobbyTitle.alignment = TextAlignmentOptions.Center;
            lobbyTitle.color = new Color(0f, 0.9f, 1f); // Cyan
        }
        
        // Join Code Input placeholder
        if (joinCodeInput != null)
        {
            joinCodeInput.text = "";
            var placeholder = joinCodeInput.placeholder.GetComponent<TextMeshProUGUI>();
            if (placeholder != null)
            {
                placeholder.text = "Enter Join Code...";
            }
        }
        
        // Hide lobby code initially
        if (lobbyCodeText != null)
        {
            lobbyCodeText.gameObject.SetActive(false);
        }
        
        // Status text
        if (statusText != null)
        {
            statusText.text = "Ready to host or join";
            statusText.fontSize = 60;
            statusText.alignment = TextAlignmentOptions.Center;
            statusText.color = Color.white;
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
                btnText.text = "HOST GAME";
                btnText.fontSize = 45;
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
                btnText.fontSize = 45;
            }
        }
        
        // Start Game Button - Only visible to host
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(() => {
                Debug.Log("START GAME BUTTON CLICKED!");
                OnStartGameClicked();
            });
            
            TextMeshProUGUI btnText = startGameButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "START GAME";
                btnText.fontSize = 45;
            }
            
            Debug.Log("Start Game button onClick listener added");
        }
        
        // Class Selection Buttons
        SetupClassButton(firewallButton, "Firewall", "FIREWALL\n<size=45>High Defense</size>");
        SetupClassButton(debuggerButton, "Debugger", "DEBUGGER\n<size=45>High Damage</size>");
        SetupClassButton(scannerButton, "Scanner", "SCANNER\n<size=45>Detect Stealth</size>");
    }
    
    void SetupClassButton(Button button, string className, string displayText)
    {
        if (button == null)
        {
            Debug.LogWarning($"⚠️ {className} button is null! Cannot setup.");
            return;
        }
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            Debug.Log($"Button clicked: {className}");
            OnClassSelected(className);
        });
        
        TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            btnText.text = displayText;
            btnText.fontSize = 45;
        }
        
        Debug.Log($"{className} button setup complete");
    }
    
    async void OnHostClicked()
    {
        if (useRelay && !isInitialized)
        {
            Debug.LogError("Unity Services not initialized!");
            if (statusText != null)
            {
                statusText.text = "<color=#FF0000>Services not ready!</color>";
            }
            return;
        }
        
        Debug.Log("Hosting game...");
        
        if (networkManager != null)
        {
            try
            {
                if (useRelay)
                {
                    // Use Unity Relay
                    Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
                    currentLobbyCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                    
                    var transport = networkManager.GetComponent<UnityTransport>();
                    if (transport != null)
                    {
                        transport.SetHostRelayData(
                            allocation.RelayServer.IpV4,
                            (ushort)allocation.RelayServer.Port,
                            allocation.AllocationIdBytes,
                            allocation.Key,
                            allocation.ConnectionData
                        );
                    }
                }
                else
                {
                    // Use local network
                    currentLobbyCode = GenerateLobbyCode();
                    var transport = networkManager.GetComponent<UnityTransport>();
                    if (transport != null)
                    {
                        transport.SetConnectionData("127.0.0.1", localPort);
                    }
                }
                
                // Start host
                networkManager.StartHost();
                
                // Display lobby code
                if (lobbyCodeText != null)
                {
                    lobbyCodeText.gameObject.SetActive(true);
                    lobbyCodeText.text = $"LOBBY CODE:\n<size=64><color=#FFD700>{currentLobbyCode}</color></size>\n<size=24>Share this with friends!</size>";
                    lobbyCodeText.fontSize = 32;
                    lobbyCodeText.alignment = TextAlignmentOptions.Center;
                }
                
                // Update status
                if (statusText != null)
                {
                    statusText.text = "<color=#00FF00>Hosting! Waiting for players...</color>";
                    statusText.fontSize = 40;
                }
                
                // Enable start button for host
                if (startGameButton != null)
                {
                    startGameButton.interactable = true;
                    Debug.Log("Start Game button ENABLED for host");
                }
                else
                {
                    Debug.LogError("Start Game button is NULL! Assign it in Inspector.");
                }
                
                // Show class selection
                ShowClassSelection();
                
                UpdatePlayerCount();
                
                Debug.Log($"Lobby created! Code: {currentLobbyCode} (Relay: {useRelay})");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create lobby: {e}");
                if (statusText != null)
                {
                    statusText.text = "<color=#FF0000>Failed to create lobby!</color>";
                }
            }
        }
    }
    
    async void OnJoinClicked()
    {
        if (useRelay && !isInitialized)
        {
            Debug.LogError("Unity Services not initialized!");
            if (statusText != null)
            {
                statusText.text = "<color=#FF0000>Services not ready!</color>";
            }
            return;
        }
        
        Debug.Log("Joining game...");
        
        if (networkManager != null && joinCodeInput != null)
        {
            // Get join code from input field
            string joinCode = joinCodeInput.text.Trim().ToUpper();
            
            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogWarning("Please enter a join code!");
                if (statusText != null)
                {
                    statusText.text = "<color=#FF0000>Please enter a join code!</color>";
                }
                return;
            }
            
            // Validate join code (6 characters for both relay and local)
            if (joinCode.Length != 6)
            {
                Debug.LogWarning("Join code must be 6 characters!");
                if (statusText != null)
                {
                    statusText.text = "<color=#FF0000>Code must be 6 characters!</color>";
                }
                return;
            }
            
            // Update status
            if (statusText != null)
            {
                statusText.text = $"<color=#FFFF00>Joining lobby: {joinCode}...</color>";
            }
            
            try
            {
                if (useRelay)
                {
                    // Join via Unity Relay
                    JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                    
                    var transport = networkManager.GetComponent<UnityTransport>();
                    if (transport != null)
                    {
                        transport.SetClientRelayData(
                            allocation.RelayServer.IpV4,
                            (ushort)allocation.RelayServer.Port,
                            allocation.AllocationIdBytes,
                            allocation.Key,
                            allocation.ConnectionData,
                            allocation.HostConnectionData
                        );
                    }
                }
                else
                {
                    // Join via local network
                    var transport = networkManager.GetComponent<UnityTransport>();
                    if (transport != null)
                    {
                        transport.SetConnectionData("127.0.0.1", localPort);
                    }
                }
                
                // Start client
                networkManager.StartClient();
                
                // Show class selection
                ShowClassSelection();
                
                // Update status
                if (statusText != null)
                {
                    statusText.text = $"<color=#00FF00>Joined lobby: {joinCode}</color>";
                }
                
                Debug.Log($"Successfully joined lobby: {joinCode} (Relay: {useRelay})");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to join lobby: {e}");
                if (statusText != null)
                {
                    statusText.text = $"<color=#FF0000>Failed to join: {e.Message}</color>";
                }
            }
        }
    }
    
    void OnStartGameClicked()
    {
        Debug.Log("Starting game...");
        
        if (networkManager != null && networkManager.IsHost)
        {
            // Save the selected class for the game scene
            PlayerPrefs.SetString("SelectedClass", selectedClass);
            PlayerPrefs.Save();
            
            Debug.Log($"Saved class selection: {selectedClass}");
            
            // Load gameplay scene for all clients
            networkManager.SceneManager.LoadScene(gameplayScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("Only the host can start the game!");
        }
    }
    
    /// <summary>
    /// Get the player's selected class (called from game scene)
    /// </summary>
    public static string GetSelectedClass()
    {
        return PlayerPrefs.GetString("SelectedClass", "Firewall");
    }
    
    void OnClassSelected(string className)
    {
        selectedClass = className;
        localPlayerClass = className; // Save for when game starts
        
        Debug.Log($"Selected class: {className}");
        
        // Update selected class text
        if (selectedClassText != null)
        {
            selectedClassText.text = $"Selected: <color=#FFD700>{className}</color>";
            selectedClassText.fontSize = 36;
        }
        
        // Visual feedback - highlight selected button
        HighlightSelectedButton(className);
        
        // Show class description
        ShowClassDescription(className);
        
        Debug.Log($"Character selected: {className} - Will spawn when game starts!");
    }
    
    void ShowClassDescription(string className)
    {
        string description = "";
        
        switch (className)
        {
            case "Firewall":
                description = "FIREWALL\n• High Defense\n• Protects Data Core\n• Absorbs virus attacks";
                break;
            case "Debugger":
                description = "🔧 DEBUGGER\n• High Damage\n• Eliminates threats\n• Fast fire rate";
                break;
            case "Scanner":
                description = "SCANNER\n• Detects stealth\n• Reveals hidden viruses\n• Support role";
                break;
        }
        
        if (statusText != null)
        {
            statusText.text = description;
            statusText.fontSize = 24;
        }
    }
    
    void HighlightSelectedButton(string className)
    {
        // Reset all button colors
        ResetButtonColor(firewallButton);
        ResetButtonColor(debuggerButton);
        ResetButtonColor(scannerButton);
        
        // Highlight selected button
        Button selectedButton = null;
        if (className == "Firewall") selectedButton = firewallButton;
        else if (className == "Debugger") selectedButton = debuggerButton;
        else if (className == "Scanner") selectedButton = scannerButton;
        
        if (selectedButton != null)
        {
            var colors = selectedButton.colors;
            colors.normalColor = new Color(1f, 0.84f, 0f, 1f); // Gold
            colors.selectedColor = new Color(1f, 0.84f, 0f, 1f);
            selectedButton.colors = colors;
        }
    }
    
    void ResetButtonColor(Button button)
    {
        if (button == null) return;
        
        var colors = button.colors;
        colors.normalColor = Color.white;
        colors.selectedColor = Color.white;
        button.colors = colors;
    }
    
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
    
    /// <summary>
    /// Generate a random 6-character alphanumeric lobby code
    /// </summary>
    string GenerateLobbyCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Exclude similar chars (I,O,0,1)
        System.Random random = new System.Random();
        char[] code = new char[6];
        
        for (int i = 0; i < 6; i++)
        {
            code[i] = chars[random.Next(chars.Length)];
        }
        
        return new string(code);
    }
    
    void UpdatePlayerCount()
    {
        // Only server/host can access ConnectedClients
        if (playerCountText != null && networkManager != null && networkManager.IsServer)
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
        // Continuously update player count (only on server)
        if (networkManager != null && networkManager.IsListening && networkManager.IsServer)
        {
            UpdatePlayerCount();
        }
    }
}
