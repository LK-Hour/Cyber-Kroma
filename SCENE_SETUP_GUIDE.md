# SCENE SETUP GUIDE

## Game Flow

**MainMenu → Tutorial → Combat → Victory/Defeat → MainMenu**

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

### 3. Update Build Settings (30 seconds)

**File → Build Settings → Add Open Scenes:**

1. Drag `MainMenu.unity` to list (Build Index: 0)
2. Drag `Tutorial.unity` to list (Build Index: 1)
3. Drag `Scene_Combat_Test.unity` to list (Build Index: 2)

**Order matters!** MainMenu should be index 0 (first scene loaded).

---

### 4. Update WaveManager for Victory/Defeat (1 minute)

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
│  MainMenu   │
└──────┬──────┘
       │
   ┌───┴────┐
   │        │
   ▼        ▼
Tutorial  Combat
   │        │
   │    ┌───┴───┐
   │    │       │
   │    ▼       ▼
   │  Victory Defeat
   │    │       │
   └────┴───┬───┘
            │
            ▼
        MainMenu
```

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
