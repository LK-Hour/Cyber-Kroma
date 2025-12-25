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
│   ├── LobbyTitle (TextMeshPro)
│   ├── PlayerCountText (TextMeshPro) - "Players: 0/4"
│   ├── HostButton (Button) - "🏠 HOST GAME"
│   ├── JoinButton (Button) - "🔗 JOIN GAME"
│   ├── IPAddressInput (TMP_InputField) - "127.0.0.1"
│   └── StartGameButton (Button) - "🚀 START GAME"
├── ClassSelectionPanel (Panel - Initially Disabled)
│   ├── FirewallButton (Button) - "🛡️ FIREWALL"
│   ├── DebuggerButton (Button) - "🔧 DEBUGGER"
│   ├── ScannerButton (Button) - "🔍 SCANNER"
│   └── SelectedClassText (TextMeshPro) - "Selected: ..."
└── NetworkLobbyManager.cs component on Canvas
```

**C. Configure NetworkLobbyManager:**
- Drag LobbyPanel → Lobby Panel field
- Drag all UI elements to corresponding fields
- Set Gameplay Scene: "Scene_Level_Design"
- Set Max Players: 4

**D. Test Lobby:**
1. Build → Build Settings → Add Scene_Network_Core
2. Click Play
3. Click "HOST GAME" → Should see "Players: 1/4"
4. Build a standalone and test Host/Join on same network

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
   - Play Mode → Should see "CYBER KROMA" title
   - Click "PLAY GAME" → Loads Combat
   - Click "TUTORIAL" → Shows tutorial steps
   - Click "QUIT" → Exits game

2. **Tutorial:**
   - Shows 5 steps with instructions
   - Press Space/Enter to advance
   - Click "Skip" to go straight to combat
   - Last step: "START GAME" → Loads Combat

3. **Combat:**
   - Defend DataCore from waves
   - Complete all waves → Victory UI → Main Menu
   - DataCore destroyed → Defeat UI → Main Menu

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
- Host Game (Start server)
- Join Game (Enter IP address, default 127.0.0.1:7777)
- Class Selection:
  - 🛡️ **Firewall** - High Defense
  - 🔧 **Debugger** - High Damage
  - 🔍 **Scanner** - Detect stealth enemies (Ghost Accounts)
- Player List (shows 1-4 connected players)
- Ready system (all players select class)
- Host starts game when ready

**Gameplay Sync (Scene_Level_Design):**
- Player movement/shooting synced via NetworkTransform
- Enemy spawns synced via WaveManager (Server Authority)
- DataCore health synced across all clients
- Shop purchases synced
- Victory/Defeat triggers for all players simultaneously

---

## Quick Setup Checklist

- [ ] Create MainMenu scene
- [ ] Create Tutorial scene  
- [ ] Add GameSceneManager to MainMenu scene
- [ ] Add MainMenuUI component with references
- [ ] Add TutorialManager component with references
- [ ] Add scenes to Build Settings (correct order!)
- [ ] Update WaveManager victory/defeat methods
- [ ] Test: MainMenu → Tutorial → Combat → Victory → MainMenu

**Estimated Time:** 6 minutes total
