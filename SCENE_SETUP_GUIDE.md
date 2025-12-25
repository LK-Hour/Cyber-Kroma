# SCENE SETUP GUIDE - MULTIPLAYER VERSION

## Game Flow (Following Game Design Document)

**MainMenu → Tutorial (Optional) → Scene_Network_Core (Lobby) → Scene_Level_Design (Main Game) → Victory/Defeat → MainMenu**

## Scene Roles

1. **MainMenu** - Title screen with Play/Tutorial/Quit buttons
2. **Tutorial** - Educational intro (5 steps explaining gameplay)
3. **Scene_Network_Core** - **Multiplayer Lobby** (Host/Join, up to 4 players, class selection)
4. **Scene_Level_Design** - **Main Gameplay** (Cyber Phnom Penh with Independence Monument)
5. **Scene_Combat_Test** - Testing only (single-player debug)

---

## Manual Setup Required in Unity

### 1. Create MainMenu Scene (2 minutes)

**A. Create Scene:**
1. File → New Scene → Basic (Built-in)
2. File → Save As → `Assets/Scenes/MainMenu.unity`

**B. Create UI:**
```
Canvas (Screen Space - Overlay)
├── GameSceneManagerObj (Empty GameObject)
│   └── GameSceneManager.cs component
├── MainMenuPanel (Panel)
│   ├── TitleText (TextMeshPro)
│   ├── PlayButton (Button)
│   ├── TutorialButton (Button)
│   └── QuitButton (Button)
└── MainMenuUI.cs component on Canvas
```

**C. Configure MainMenuUI:**
- Drag MainMenuPanel → Main Menu Panel field
- Drag TitleText → Title Text field
- Drag PlayButton → Play Button field
- Drag TutorialButton → Tutorial Button field
- Drag QuitButton → Quit Button field

**D. Style (Auto-styled by UIStyleManager):**
- Background: Dark purple gradient
- Title: Cyan→Magenta animated
- Buttons: Gold with hover effects

---

### 2. Create Tutorial Scene (2 minutes)

**A. Create Scene:**
1. File → New Scene → Basic (Built-in)
2. File → Save As → `Assets/Scenes/Tutorial.unity`

**B. Create UI:**
```
Canvas (Screen Space - Overlay)
├── TutorialPanel (Panel)
│   ├── TitleText (TextMeshPro)
│   ├── InstructionText (TextMeshPro - large area)
│   ├── StartGameButton (Button)
│   └── SkipButton (Button)
└── TutorialManager.cs component on Canvas
```

**C. Configure TutorialManager:**
- Drag TutorialPanel → Tutorial Panel field
- Drag TitleText → Title Text field  
- Drag InstructionText → Instruction Text field
- Drag StartGameButton → Start Game Button field
- Drag SkipButton → Skip Button field

---

### 3. Setup Scene_Network_Core (Lobby) - IMPORTANT! (5 minutes)

**This is your multiplayer lobby where players Host/Join!**

**A. Add NetworkManager:**
1. Open `Scene_Network_Core.unity`
2. Create Empty GameObject → Name: "NetworkManager"
3. Add Component → **NetworkManager** (from Netcode for GameObjects)
4. Add Component → **Unity Transport** (UTP)
5. Configure NetworkManager:
   - Max Connections: 4
   - Connection Approval: ✓ (optional)

**B. Create Lobby UI:**
```
Canvas (Screen Space - Overlay)
├── LobbyPanel (Panel)
│   ├── LobbyTitle (TextMeshPro) - "🌐 CYBER KROMA LOBBY"
│   ├── LobbyCodeText (TextMeshPro) - "🔑 Lobby Code: ______" (hidden initially)
│   ├── PlayerCountText (TextMeshPro) - "Players: 0/4"
│   ├── HostButton (Button) - "🏠 HOST GAME"
│   ├── JoinButton (Button) - "🔗 JOIN GAME"
│   ├── JoinCodeInput (TMP_InputField) - "Enter Join Code..."
│   └── StartGameButton (Button) - "🚀 START GAME"
├── ClassSelectionPanel (Panel - Initially Disabled)
│   ├── FirewallButton (Button) - "🛡️ FIREWALL"
│   ├── DebuggerButton (Button) - "🔧 DEBUGGER"
│   ├── ScannerButton (Button) - "🔍 SCANNER"
│   └── SelectedClassText (TextMeshPro) - "Selected: ..."
└── NetworkLobbyManager.cs component on Canvas
```

**✅ Already Setup via Unity MCP!** The lobby UI has been automatically created in Scene_Network_Core.

**C. Configure NetworkLobbyManager:**
- Drag LobbyPanel → Lobby Panel field
- Drag LobbyTitle → Lobby Title field
- Drag LobbyCodeText → Lobby Code Text field (shows generated code)
- Drag PlayerCountText → Player Count Text field
- Drag HostButton → Host Button field
- Drag JoinButton → Join Button field
- Drag JoinCodeInput → Join Code Input field (where players enter code)
- Drag StartGameButton → Start Game Button field
- Drag ClassSelectionPanel → Class Selection Panel field
- Drag FirewallButton, DebuggerButton, ScannerButton → Corresponding fields
- Drag SelectedClassText → Selected Class Text field
- Set Gameplay Scene: "Scene_Level_Design"
- Set Max Players: 4

**D. Test Lobby:**
1. Build → Build Settings → Add Scene_Network_Core
2. Click Play in Unity Editor
3. Click "HOST GAME" → Should see:
   - "🔑 Lobby Code: **A7K9M2**" (example 6-character code)
   - "Players: 1/4"
4. Build a standalone build:
   - Host clicks "HOST GAME" → Gets lobby code
   - Friend clicks "JOIN GAME" → Enters the same code → Joins!
5. Both players select class (Firewall/Debugger/Scanner)
6. Host clicks "START GAME" → Both load Scene_Level_Design

---

### 4. Setup Scene_Level_Design (Main Gameplay) - IMPORTANT! (10 minutes)

**A. Setup Networked Gameplay:**
1. Open `Scene_Level_Design.unity`
2. **CRITICAL:** Add NetworkManager if not present (same as Step 3A)
3. Find Independence Monument (environment centerpiece)
4. Ensure Player prefab has:
   - ✅ NetworkObject component
   - ✅ NetworkTransform component  
   - ✅ NetworkAnimator component
   - ✅ CharacterController, CharacterMovement, CharacterShooting, CharacterHealth

**B. Setup Scene References:**
- WaveManager → Set enemy prefabs, spawn points
- DataCore → Must have "DataCore" tag
- Ground → Navigation Static ✓ → Bake NavMesh
- MasterSceneConfigurator → Add to Canvas (auto-configures everything)

**C. Test Multiplayer:**
1. Host in Scene_Network_Core
2. Press "START GAME" → Loads Scene_Level_Design
3. All connected players spawn
4. Enemies spawn and attack DataCore
5. Players defend cooperatively!

---

### 5. Update Build Settings (1 minute)

**File → Build Settings → Add Open Scenes IN ORDER:**

1. **MainMenu.unity** (Build Index: 0) - First scene loaded
2. **Tutorial.unity** (Build Index: 1)
3. **Scene_Network_Core.unity** (Build Index: 2) - **Lobby**
4. **Scene_Level_Design.unity** (Build Index: 3) - **Main Game**
5. Scene_Combat_Test.unity (Build Index: 4) - Optional testing

**Platform:** Switch to Android if deploying to mobile

---

Add to **WaveManager.cs** OnVictory() and OnDefeat():

```csharp
void OnVictory()
{
    ShowVictoryPanel();
    
    // Return to main menu after 5 seconds
    if (GameSceneManager.Instance != null)
    {
        GameSceneManager.Instance.OnVictory();
    }
}

void OnDefeat()
{
    ShowDefeatPanel();
    
    // Return to main menu after 5 seconds
    if (GameSceneManager.Instance != null)
    {
        GameSceneManager.Instance.OnDefeat();
    }
}
```

---

## Testing Flow

1. **Start from MainMenu:**
   - Play Mode → Should see "⚡ CYBER KROMA ⚡" title
   - Click "▶️ PLAY GAME" → Loads Scene_Network_Core (Lobby)
   - Click "📚 TUTORIAL" → Shows tutorial steps
   - Click "❌ QUIT" → Exits game

2. **Tutorial:**
   - Shows 5 steps with instructions about:
     - Objective (defend DataCore)
     - Controls (WASD, Mouse, Shoot)
     - Enemies (Phisher, Ghost, DeepFake)
     - Shop system
   - Press Space/Enter to advance
   - Click "⏭️ Skip Tutorial" → Jump to lobby
   - Last step: "🚀 START GAME" → Loads Scene_Network_Core

3. **Lobby (Scene_Network_Core):**
   - **Host:** Click "🏠 HOST GAME" → Get join code (e.g., "K7H2M9")
   - **Join:** Click "🔗 JOIN GAME" → Enter code "K7H2M9" → Connect!
   - Select class: 🛡️ Firewall / 🔧 Debugger / 🔍 Scanner
   - Host clicks "🚀 START GAME" → All players load Scene_Level_Design

4. **Combat (Scene_Level_Design):**
   - Defend DataCore from 3 waves of enemies
   - Complete Wave 3 → Victory UI → Main Menu (after 5 seconds)
   - DataCore health reaches 0 → Defeat UI → Main Menu (after 5 seconds)

---

## Scene Flow Diagram

```
┌─────────────┐
│  MainMenu   │ ← Game starts here
└──────┬──────┘
       │
   ┌───┴─────┐
   │         │
   ▼         ▼
Tutorial   Scene_Network_Core (Lobby)
   │         │
   │    ┌────┴────┐
   │    │  Host/  │ ← Up to 4 players
   │    │  Join   │
   │    └────┬────┘
   │         │
   └─────┬───┘
         ▼
   Scene_Level_Design (Main Game)
         │
    ┌────┴────┐
    │         │
    ▼         ▼
  Victory   Defeat
    │         │
    └────┬────┘
         ▼
     MainMenu
```

---

## Multiplayer Features (From GDD)

**Lobby (Scene_Network_Core):**
- **Host Game** - Generates unique 6-character join code (e.g., "A7K9M2")
- **Join Game** - Enter host's join code to connect (no IP addresses needed!)
- **Join Code Format:**
  - 6 alphanumeric characters (excludes I, O, 0, 1 to avoid confusion)
  - Example codes: "K7H2M9", "PQ3X8F", "ZY4N6T"
  - Much easier than remembering "192.168.1.143:7777"!
- **Class Selection:**
  - 🛡️ **Firewall** - High Defense, tank role
  - 🔧 **Debugger** - High Damage, DPS role
  - 🔍 **Scanner** - Detect stealth enemies (Ghost Accounts)
- **Player List** - Shows 1-4 connected players with their names
- **Ready System** - All players must select class before starting
- **Host Control** - Only host can click "START GAME" button

**Gameplay Sync (Scene_Level_Design):**
- Player movement/shooting synced via NetworkTransform
- Enemy spawns synced via WaveManager (Server Authority)
- DataCore health synced across all clients
- Shop purchases synced
- Victory/Defeat triggers for all players simultaneously

---

## Quick Setup Checklist

**Automated via Unity MCP (Already Done!):**
- ✅ Scene_Network_Core lobby UI created
- ✅ NetworkLobbyManager component added
- ✅ Join code system implemented
- ✅ GameSceneManager added to Scene_Network_Core

**Manual Steps Required:**
- [ ] Create MainMenu scene (2 min) - See Step 1
- [ ] Create Tutorial scene (2 min) - See Step 2
- [ ] Add NetworkManager to Scene_Network_Core - See Step 3A
- [ ] Configure NetworkLobbyManager references - See Step 3C
- [ ] Setup Scene_Level_Design with NetworkManager - See Step 4
- [ ] Add scenes to Build Settings in correct order - See Step 5
- [ ] Bake NavMesh in Scene_Level_Design
- [ ] Test full flow: MainMenu → Lobby (Join Code) → Combat → Victory/Defeat

**Estimated Time:** 
- Manual setup: ~15 minutes
- Testing: ~10 minutes
- **Total: ~25 minutes**
