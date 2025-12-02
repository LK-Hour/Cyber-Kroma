# 👑 Leader Guide: Kimhour's Complete Workflow
**Role:** The Integrator (Network & Build Manager)  
**Your Mission:** Connect everyone's work and make the game run on Android phones with multiplayer.

---

## 📋 Table of Contents
1. [Initial Setup (Already Done ✅)](#initial-setup)
2. [Week 1: Learning Netcode Basics](#week-1-netcode-basics)
3. [Week 2: Integrating Team Members' Work](#week-2-integration)
4. [Week 3: Testing & Final Build](#week-3-final-build)
5. [Daily/Weekly Checklist](#checklist)

---

## ✅ Initial Setup (Already Done)
You've completed:
- ✅ Created Unity Project (3D URP)
- ✅ Set up GitHub Repository
- ✅ Created 4 scenes for team members
- ✅ Added README.md

**Next:** Start working in your scene: `Scene_Network_Core`

---

## 🎮 Week 1: Netcode Basics (Dec 2-8)

### Day 1-2: Install Netcode for GameObjects (NGO)

#### Step 1: Install the Package
1. Open Unity
2. Go to **Window** → **Package Manager**
3. Click the **+** button (top left)
4. Select **Add package by name...**
5. Type: `com.unity.netcode.gameobjects`
6. Click **Add**
7. Wait for installation (2-3 minutes)

#### Step 2: Create Network Manager
1. In your scene `Scene_Network_Core`, right-click in **Hierarchy**
2. Select **Create Empty**
3. Rename it to `NetworkManager`
4. With `NetworkManager` selected, click **Add Component** in Inspector
5. Search for `NetworkManager` and add it
6. You'll see a warning "No NetworkTransport Selected" - that's normal for now

#### Step 3: Add Unity Transport
1. Still on the `NetworkManager` object
2. Click **Add Component**
3. Search for `Unity Transport`
4. Add it
5. Back in the `NetworkManager` component:
   - Find the **NetworkTransport** field
   - Drag the `NetworkManager` GameObject into this field (it will auto-select the Unity Transport component)

**✅ Checkpoint:** You should now see `NetworkManager` with `UnityTransport` attached and no errors.

---

### Day 3-4: Set Up Unity Relay (For Online Play)

#### Why Relay?
Without Relay, players need to know each other's IP addresses and open ports on their routers. Relay acts as a "middleman" so players can connect using a simple join code.

#### Step 1: Create Unity Gaming Services Project
1. Go to [https://dashboard.unity3d.com/](https://dashboard.unity3d.com/)
2. Sign in with your Unity account
3. Click **Create Project** (or select your existing project)
4. Name it: `Cyber Kroma`
5. Click **Create**

#### Step 2: Link Unity Editor to Services
1. Back in Unity Editor, go to **Edit** → **Project Settings**
2. Click **Services** (in the left menu)
3. Click **Select Organization** → Choose your organization
4. Click **Create** or **Link** to connect to the project you just made

#### Step 3: Enable Relay
1. Go to [https://dashboard.unity3d.com/](https://dashboard.unity3d.com/)
2. Select your `Cyber Kroma` project
3. In the left menu, find **Multiplayer** → **Relay**
4. Click **Set Up Relay**
5. Click **Enable** (It's free - no credit card needed)

#### Step 4: Install Relay Package in Unity
1. Back in Unity: **Window** → **Package Manager**
2. Click **+** → **Add package by name...**
3. Type: `com.unity.services.relay`
4. Click **Add**

**✅ Checkpoint:** Relay is now enabled on the cloud and installed in Unity.

---

### Day 5-7: Create the Lobby Manager Script

#### Step 1: Create the Script
1. In **Project** window, go to **Assets** (create a `Scripts` folder if you don't have one)
2. Right-click in `Scripts` folder → **Create** → **C# Script**
3. Name it: `LobbyManager`
4. Double-click to open in your code editor

#### Step 2: Write the Lobby Manager Code

```csharp
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
```

**Save the script (Ctrl+S).**

#### Step 3: Create the Lobby UI

1. In **Hierarchy**, right-click → **UI** → **Canvas**
2. Right-click on `Canvas` → **UI** → **Panel** (rename to `LobbyPanel`)
3. Right-click on `LobbyPanel` → **UI** → **Button** (rename to `CreateGameButton`)
   - Click on the button's **Text** child → Change text to: "Create Game"
4. Right-click on `LobbyPanel` → **UI** → **Button** (rename to `JoinGameButton`)
   - Click on the button's **Text** child → Change text to: "Join Game"
5. Right-click on `LobbyPanel` → **UI** → **Input Field - TextMeshPro** (rename to `JoinCodeInput`)
   - If prompted to import TMP Essentials, click "Import TMP Essentials"
   - In the placeholder text, type: "Enter Join Code"
6. Right-click on `LobbyPanel` → **UI** → **Text - TextMeshPro** (rename to `StatusText`)
   - Change text to: "Initializing..."
7. Right-click on `LobbyPanel` → **UI** → **Text - TextMeshPro** (rename to `JoinCodeDisplay`)
   - Change text to: "Join Code: "
   - Set this to **inactive** (uncheck the checkbox next to its name in Inspector)

#### Step 4: Connect the Script to UI

1. In **Hierarchy**, select `NetworkManager`
2. Click **Add Component** → Search for `LobbyManager` → Add it
3. In the `LobbyManager` component:
   - Drag `LobbyPanel` to **Lobby UI**
   - Drag `JoinCodeInput` to **Join Code Input**
   - Drag `StatusText` to **Status Text**
   - Drag `JoinCodeDisplay` to **Join Code Display Text**
4. Select `CreateGameButton`:
   - Scroll down to **On Click ()**
   - Click **+**
   - Drag `NetworkManager` into the empty field
   - Click **No Function** → `LobbyManager` → `CreateGame()`
5. Select `JoinGameButton`:
   - Scroll down to **On Click ()**
   - Click **+**
   - Drag `NetworkManager` into the empty field
   - Click **No Function** → `LobbyManager` → `JoinGame()`

**✅ Checkpoint:** Press Play. You should see "Ready! Create or Join a game." Click "Create Game" and you should get a join code like "ABC123".

---

## 🔗 Week 2: Integration (Dec 9-15)

### When Pranha Finishes the Player (Around Dec 9-10)

#### Step 1: Get Pranha's Player Prefab
1. Ask Pranha to **commit and push** his latest work
2. In your terminal: `git pull origin main`
3. In Unity, go to **Assets** → Look for a folder like `Prefabs` or `Player`
4. Find `PlayerPrefab` (or whatever Pranha named it)

#### Step 2: Add Network Components to the Player
1. Select the `PlayerPrefab`
2. Click **Add Component** → Search `NetworkObject` → Add it
3. Click **Add Component** → Search `NetworkTransform` → Add it
4. Click **Add Component** → Search `NetworkAnimator` → Add it
   - In `NetworkAnimator`, drag the player's `Animator` component into the **Animator** field

#### Step 3: Register the Player in NetworkManager
1. Select `NetworkManager` in Hierarchy
2. In the `NetworkManager` component:
   - Find **Player Prefab**
   - Drag `PlayerPrefab` into this field
3. **Important:** The player prefab must be in a **Resources** folder or **added to the NetworkPrefabs list**:
   - Scroll down in `NetworkManager` to **NetworkPrefabs**
   - Click **+**
   - Drag `PlayerPrefab` into the new slot

#### Step 4: Spawn the Player
The player will automatically spawn when a client connects. But you might want to control *where*:

1. Create empty GameObjects in your scene called `SpawnPoint1`, `SpawnPoint2`, etc.
2. Position them where you want players to appear
3. In `LobbyManager.cs`, after `StartHost()` or `StartClient()`, you can set spawn positions (optional for now)

**✅ Checkpoint:** Click "Create Game" → Open a second Unity Editor → Click "Join Game" with the code → Both should see two players!

---

### When An Finishes the Enemy AI (Around Dec 12-13)

#### Step 1: Get An's Enemy Prefab
1. `git pull origin main`
2. Find `EnemyPrefab` and `WaveManager` script in **Assets**

#### Step 2: Add Network Components to Enemy
1. Select `EnemyPrefab`
2. Add `NetworkObject` component
3. Add `NetworkTransform` component
4. Add to `NetworkManager` → **NetworkPrefabs** list

#### Step 3: Modify Wave Spawner for Multiplayer
**Problem:** If both Host and Client run the WaveManager, enemies spawn twice!

**Solution:** Only the **Server** should spawn enemies.

In An's `WaveManager.cs`, wrap the spawn logic:

```csharp
using Unity.Netcode;

public class WaveManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // Only the server spawns enemies
        if (IsServer)
        {
            StartCoroutine(SpawnWave());
        }
    }
}
```

**You'll need to help An with this part** - show him this code snippet.

---

### When Longboren Finishes the Map (Around Dec 13-14)

#### Step 1: Get the Map Scene
1. `git pull origin main`
2. Open `Scene_Level_Design`

#### Step 2: Combine Everything
1. Copy the `NetworkManager` from `Scene_Network_Core` (Ctrl+C)
2. Paste it into `Scene_Level_Design` (Ctrl+V)
3. Find An's `WaveManager` → Drag it into this scene
4. Make sure the `DataCore` object (from An's scene) is in this scene too

#### Step 3: Test Build
1. Go to **File** → **Build Settings**
2. Click **Add Open Scenes** (to add `Scene_Level_Design`)
3. Remove the other test scenes (select and click **Remove**)
4. Click **Build** (not Build and Run yet)
5. Choose a folder like `Builds/PC_Test`
6. Wait for build
7. Run the `.exe` → Click "Create Game"
8. In Unity Editor, click Play → Click "Join Game" with the code
9. They should connect!

**✅ Checkpoint:** PC build and Editor can play together.

---

## 📱 Week 3: Android Build (Dec 16-22)

### Day 1-2: Set Up Android Build Tools

#### Step 1: Install Android Build Support (If Not Already)
1. Close Unity
2. Open **Unity Hub**
3. Go to **Installs**
4. Click the **⚙️ (gear icon)** on your Unity version
5. Click **Add Modules**
6. Check:
   - ✅ Android Build Support
   - ✅ Android SDK & NDK Tools
   - ✅ OpenJDK
7. Click **Install** (This takes 10-30 minutes)

#### Step 2: Configure Android Settings
1. Open Unity
2. Go to **File** → **Build Settings**
3. Select **Android**
4. Click **Switch Platform** (wait 2-5 minutes)
5. Click **Player Settings**
6. In **Player Settings**:
   - **Company Name:** Your name or "CADT"
   - **Product Name:** Cyber Kroma
   - **Package Name:** `com.cadt.cyberkroma` (must be lowercase, no spaces)
7. Scroll down to **Other Settings**:
   - **Minimum API Level:** Android 7.0 'Nougat' (API Level 24)
   - **Target API Level:** Automatic (highest installed)
   - **Scripting Backend:** IL2CPP
   - **Target Architectures:** ✅ ARM64 (uncheck ARMv7)

#### Step 3: Configure Graphics for URP
1. Still in **Player Settings** → **Other Settings**
2. Find **Graphics APIs**
3. Make sure **OpenGLES3** is at the top (or Vulkan for newer phones)
4. Go to **Edit** → **Project Settings** → **Graphics**
5. Make sure **Scriptable Render Pipeline Settings** is set to your URP asset (should already be set)

**✅ Checkpoint:** Build Settings shows "Android" as current platform.

---

### Day 3-4: First Android Test Build

#### Step 1: Enable Developer Mode on Your Phone
1. On your phone: **Settings** → **About Phone**
2. Tap **Build Number** 7 times (it will say "You are now a developer!")
3. Go back → **Settings** → **Developer Options**
4. Turn on **USB Debugging**

#### Step 2: Connect Phone to Computer
1. Plug phone into computer via USB
2. Phone will show a popup: "Allow USB Debugging?" → Tap **Allow**
3. In Unity, go to **File** → **Build Settings**
4. Click **Refresh** next to "Run Device"
5. Your phone should appear in the dropdown

#### Step 3: Build and Run
1. In **Build Settings**, check **Development Build** (for testing)
2. Click **Build and Run**
3. Save the APK as `CyberKroma_Test.apk`
4. Wait 5-15 minutes (first build is slow)
5. The game should auto-install and launch on your phone!

**✅ Checkpoint:** Game runs on your phone (even if it crashes or has bugs - that's expected).

---

### Day 5-6: Test Multiplayer on 2 Phones

#### Step 1: Build APK File
1. Uncheck **Development Build** (for final build)
2. Click **Build** (not Build and Run)
3. Save as `CyberKroma_v1.apk`

#### Step 2: Install on Multiple Devices
1. Copy the APK to Google Drive or send via WhatsApp
2. Download on each phone
3. Install (you may need to allow "Install from Unknown Sources")

#### Step 3: Test Connection
1. **Phone 1:** Open game → Click "Create Game" → Note the join code
2. **Phone 2:** Open game → Enter join code → Click "Join Game"
3. They should see each other!

**Common Issues:**
- **"Failed to join"**: Both phones must be online (WiFi or Mobile Data)
- **Player doesn't move**: Make sure Pranha's Input System is set to **Touchscreen**, not just Gamepad
- **Lag**: Normal for Relay - it routes through Unity's servers

---

### Day 7: Final Testing & Polish

#### Checklist Before Submission:
- [ ] 4 players can join and see each other
- [ ] Pranha's shooting works on mobile
- [ ] An's enemies spawn and move (only on server)
- [ ] Longboren's UI is readable on small screens
- [ ] Game doesn't crash when someone leaves
- [ ] APK size is under 200MB (check file size)

#### Build Settings Optimization (If APK is Too Big):
1. **Edit** → **Project Settings** → **Player** → **Other Settings**
2. **Managed Stripping Level:** Medium (or High if you're brave)
3. **Edit** → **Project Settings** → **Quality**
4. Set default quality to **Medium** for Android

---

## 📝 Your Daily Checklist

### Every Day:
- [ ] `git pull origin main` (before you start work)
- [ ] Check Discord/WhatsApp for team updates
- [ ] Test your `Scene_Network_Core` (press Play, create game, check for errors)
- [ ] `git add .` → `git commit -m "UPDATE: ..."` → `git push origin main` (before you close Unity)

### Every 3 Days (Sync with Team):
1. **Pull everyone's changes:** `git pull origin main`
2. **Open Longboren's scene:** `Scene_Level_Design`
3. **Add missing prefabs:**
   - Drag Pranha's `PlayerPrefab` to `NetworkManager` → Player Prefab
   - Drag An's `WaveManager` into the scene
4. **Test in Editor:** Create Game → Join Game (2 Editor windows)
5. **Build APK:** `File` → `Build Settings` → `Build`
6. **Share in group chat:** "New test build ready - [link to APK]"

### Every Week (Friday):
- [ ] Build a clean APK and send to everyone
- [ ] Write a short message: "What's new this week: [list features]"
- [ ] Test on 2-3 devices together

---

## 🆘 Common Problems & Solutions

### Problem: "NetworkManager not found"
**Solution:** Make sure you added `using Unity.Netcode;` at the top of your scripts.

### Problem: "Player spawns but can't move on Android"
**Solution:** 
- Pranha needs to use **Input System** (not old Input Manager)
- In his Input Actions, set **Control Scheme** to **Touchscreen**
- Add on-screen joysticks using **UI** → **Canvas** → add touch controls

### Problem: "Enemies spawn twice (once per player)"
**Solution:** In An's code, wrap spawn logic with:
```csharp
if (IsServer)
{
    // Only server spawns
}
```

### Problem: "Build failed - Unable to find Android SDK"
**Solution:**
1. **Edit** → **Preferences** → **External Tools**
2. Check **Android SDK Tools Installed with Unity**
3. Restart Unity

### Problem: "Game crashes on phone but works in Editor"
**Solution:**
- Check **Logcat** (install via Package Manager: `com.unity.mobile.android-logcat`)
- Common cause: Missing "Read/Write" permission on audio files → Select audio file → Check "Load in Background"

---

## 📚 What to Learn

### Don't Panic If You Don't Know:
- How Netcode works internally (you just use it like a tool)
- Advanced networking concepts (RPCs, ownership transfer) - not needed for basic multiplayer
- How to make your own Relay server - Unity's is free and works

### What You MUST Understand:
- **Host vs Client:** Host is both a server and a player. Clients just play.
- **Server Authority:** Only the server decides if an enemy dies (to prevent cheating).
- **NetworkObject:** Unity's way of saying "sync this across the network."
- **Prefabs:** Why we need them (so Unity can spawn copies on all devices).

---

## 🎯 Your Success Metrics

By **Dec 22**, you should have:
1. ✅ A working APK file
2. ✅ 4 phones can connect and play together
3. ✅ No crashes for 5 minutes of gameplay
4. ✅ All team members' work is integrated (Player, AI, Map)

---

## 💡 Pro Tips

1. **Build to Android Weekly, Not Daily:** It's slow. Test in Editor first.
2. **Use Git Branches for Risky Changes:** If you're testing something crazy, create a branch: `git checkout -b experimental`.
3. **Keep a "Working" APK:** Every time you get a stable build, rename it `CyberKroma_STABLE_v1.apk` and keep it. If you break something, you have a backup.
4. **Test with 2 Unity Editors First:** Open Unity twice (File → New Window). Click "Create Game" in one, "Join Game" in the other. Way faster than building to phone.
5. **Read the Console:** 90% of bugs tell you what's wrong in the Console. If it says "NullReferenceException at Line 42", go to Line 42.

---

## 📅 Timeline Summary

| Date | Task | Deliverable |
|------|------|-------------|
| Dec 2-4 | Install NGO, set up Relay | Can create/join games in Editor |
| Dec 5-8 | Build Lobby UI | Working join code system |
| Dec 9-11 | Integrate Pranha's player | 2 players can move together |
| Dec 12-14 | Integrate An's AI + Longboren's map | Enemies spawn in the Phnom Penh map |
| Dec 15-17 | First Android build | APK runs on phone |
| Dec 18-20 | Multiplayer testing on phones | 4 phones can play together |
| Dec 21-22 | Polish & final build | Submit APK for grading |

---

## 🚀 Final Words

You're the glue that holds the project together. Your team is building the features, but **you're the one who makes them play together.**

When things break (and they will):
1. Check the Console first
2. Google the error + "Unity Netcode"
3. Ask the team if they changed something
4. If stuck for 1 hour, message me or ask ChatGPT

You got this. Start with Week 1, Day 1. One step at a time.

Good luck! 🎮
