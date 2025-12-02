using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [Header("UI References - Assign in Inspector")]
    public GameObject lobbyUI;
    public TMPro.TMP_InputField joinCodeInput;
    public TMPro.TMP_Text statusText;
    public TMPro.TMP_Text joinCodeDisplayText;

    private async void Start()
    {
        // Initialize Unity Services (Required for Relay)
        await InitializeUnityServices();
    }

    /// <summary>
    /// Step 1: Sign in to Unity Services
    /// </summary>
    private async Task InitializeUnityServices()
    {
        try
        {
            UpdateStatus("Signing in...");
            await UnityServices.InitializeAsync();
            
            // Sign in anonymously (no login required)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            UpdateStatus("Ready! Create or Join a game.");
            Debug.Log("✅ Unity Services Initialized");
        }
        catch (Exception e)
        {
            UpdateStatus("Error: " + e.Message);
            Debug.LogError($"Failed to initialize Unity Services: {e}");
        }
    }

    /// <summary>
    /// HOST: Create a new game and get a join code
    /// </summary>
    public async void CreateGame()
    {
        try
        {
            UpdateStatus("Creating game...");

            // Create a Relay allocation (max 4 players for this example)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); // 3 + 1 host = 4 total

            // Get the join code that others will use
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            // Configure the transport to use this Relay allocation
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            // Start as Host
            NetworkManager.Singleton.StartHost();

            // Display the join code
            UpdateStatus("Game Created!");
            ShowJoinCode(joinCode);

            Debug.Log($"✅ Host started. Join Code: {joinCode}");
        }
        catch (Exception e)
        {
            UpdateStatus("Failed to create game: " + e.Message);
            Debug.LogError($"Error creating game: {e}");
        }
    }

    /// <summary>
    /// CLIENT: Join an existing game using a join code
    /// </summary>
    public async void JoinGame()
    {
        string joinCode = joinCodeInput.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(joinCode))
        {
            UpdateStatus("Please enter a join code!");
            return;
        }

        try
        {
            UpdateStatus("Joining game...");

            // Join the Relay using the code
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // Configure the transport to use this Relay allocation
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );

            // Start as Client
            NetworkManager.Singleton.StartClient();

            UpdateStatus("Joined successfully!");
            HideLobbyUI();

            Debug.Log($"✅ Client joined with code: {joinCode}");
        }
        catch (Exception e)
        {
            UpdateStatus("Failed to join: " + e.Message);
            Debug.LogError($"Error joining game: {e}");
        }
    }

    /// <summary>
    /// Update the status text in the UI
    /// </summary>
    private void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
        
        Debug.Log($"[LobbyManager] {message}");
    }

    /// <summary>
    /// Show the join code to the host
    /// </summary>
    private void ShowJoinCode(string code)
    {
        if (joinCodeDisplayText != null)
        {
            joinCodeDisplayText.text = $"Share this code:\n{code}";
            joinCodeDisplayText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hide the lobby UI when game starts
    /// </summary>
    private void HideLobbyUI()
    {
        if (lobbyUI != null)
            lobbyUI.SetActive(false);
    }
}