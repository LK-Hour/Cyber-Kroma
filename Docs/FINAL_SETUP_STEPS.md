# Final Setup Steps - Scene_AI_Test Complete Assembly

## ✅ What's Already Done (Automated via Unity MCP)

**Scene Structure:**
- ✅ Player (from existing prefab: Assets/TextMesh Pro/Resources/Player.prefab)
- ✅ Ground (15x15 Plane for NavMesh)
- ✅ Main Camera + Directional Light
- ✅ Independence Monument (from GLB asset)
- ✅ DataCore cube with DataCoreHealth component + cyan material
- ✅ WaveManager GameObject with WaveManager component
- ✅ PlayerPoints GameObject with PlayerPoints component
- ✅ 5 SpawnPoints positioned around arena
- ✅ 3 Enemy capsules: Phisher (red), GhostAccount (purple), DeepFake (orange)
- ✅ All enemies have NavMeshAgent + EnemyAI components
- ✅ Canvas + EventSystem for UI
- ✅ EducationManager with ScamEducationUI component
- ✅ LokTaShop GameObject with LokTaShop component
- ✅ UI Panels created: ShopPanel, EducationPanel, VictoryPanel, DefeatPanel, WaveUI, PointsUI

---

## 🔧 Manual Steps Required in Unity Editor (30-45 minutes)

### Step 1: Bake NavMesh (5 minutes) - CRITICAL!

**Without NavMesh, enemies won't move!**

1. Select `Ground` GameObject in Hierarchy
2. Inspector → Check "Navigation Static" (top right)
3. Window → AI → Navigation
4. Navigation window → Bake tab
5. Click **"Bake"** button
6. Wait for blue NavMesh overlay to appear on Ground
7. Verify all 5 SpawnPoints are on blue area

---

### Step 2: Link WaveManager References (5 minutes)

**Select WaveManager GameObject:**

1. **Spawn Points Array:**
   - Set Size = 5
   - Drag: SpawnPoint_1, SpawnPoint_2, SpawnPoint_3, SpawnPoint_4, SpawnPoint_5

2. **Enemy Prefabs:**
   - Phisher Prefab: Drag `Phisher` from Hierarchy to field (or save as prefab first)
   - Ghost Account Prefab: Drag `GhostAccount`
   - DeepFake Prefab: Drag `DeepFake`

3. **References:**
   - Data Core: Drag `DataCore` GameObject
   - Education UI: Drag `EducationManager` GameObject
   - Shop Panel: Drag `ShopPanel` GameObject
   - Victory Panel: Drag `VictoryPanel` GameObject
   - Defeat Panel: Drag `DefeatPanel` (will need to create this)

---

### Step 3: Save Enemy Prefabs (3 minutes)

**For reusability:**

1. Drag `Phisher` from Hierarchy → `Assets/Prefabs/Enemies/` folder
2. Drag `GhostAccount` → `Assets/Prefabs/Enemies/`
3. Drag `DeepFake` → `Assets/Prefabs/Enemies/`
4. Now link these prefabs to WaveManager (Step 2)

---

### Step 4: Configure Enemy AI Components (5 minutes)

**Select Phisher:**
- EnemyAI component:
  - Enemy Type: Phisher
  - Move Speed: 3.5
  - Health: 100
  - Attack Damage: 15
  - Attack Range: 10
  - Attack Cooldown: 2.0

**Select GhostAccount:**
- EnemyAI component:
  - Enemy Type: GhostAccount
  - Move Speed: 5.0
  - Health: 80
  - Attack Damage: 25
  - Attack Range: 2.0
  - Attack Cooldown: 1.5
  - Stealth Duration: 5.0

**Select DeepFake:**
- EnemyAI component:
  - Enemy Type: DeepFake
  - Move Speed: 4.0
  - Health: 300
  - Attack Damage: 30
  - Attack Range: 15
  - Attack Cooldown: 3.0

---

### Step 5: Setup UI Panels (15 minutes)

**ShopPanel (child of Canvas):**
1. Add Component → UI → Image (background)
2. Set Color: Dark semi-transparent (RGBA: 0, 0, 0, 180)
3. Rect Transform: Width 600, Height 400, Anchors Center
4. **Initially Disabled** (uncheck at top of Inspector)

**Create 3 Buttons as children:**
- BtnHealth: Position (-150, -50), Text "Health\n50 pts"
- BtnShield: Position (0, -50), Text "Shield\n75 pts"
- BtnAmmo: Position (150, -50), Text "Ammo\n30 pts"

**Link to LokTaShop:**
- Select LokTaShop GameObject
- Shop Panel = ShopPanel
- Health Button = BtnHealth
- Shield Button = BtnShield
- Ammo Button = BtnAmmo

---

**EducationPanel (child of Canvas):**
1. Add Component → UI → Image
2. Width 700, Height 500
3. **Initially Disabled**
4. Create children:
   - TitleText (TextMeshPro): Position (0, 180), Size 600x60, Font Size 32
   - DescriptionText (TextMeshPro): Position (0, 0), Size 600x300, Font Size 20
   - BtnClose (Button): Position (250, -200), Text "Close / បិទ"

**Link to EducationManager:**
- Education Panel = EducationPanel
- Title Text = TitleText
- Description Text = DescriptionText
- Btn Close = BtnClose

---

**VictoryPanel + DefeatPanel:**
1. Similar to ShopPanel
2. Add large TextMeshPro: "VICTORY! / ជ័យជម្នះ!" or "DEFEAT / ចាញ់"
3. **Initially Disabled**
4. Link to WaveManager

---

**WaveUI + PointsUI:**
1. Both as TextMeshPro - Text (no panel needed)
2. WaveUI: Top-left corner, Text "Wave 1/5"
3. PointsUI: Top-right corner, Text "Points: 100"
4. Link:
   - WaveManager → waveText = WaveUI
   - WaveManager → enemyCountText = (create another text)
   - PlayerPoints → pointsText = PointsUI

---

### Step 6: Configure DataCore Visual Feedback (2 minutes)

**Select DataCore:**
- DataCoreHealth component:
  - Core Material: Drag `Assets/Materials/DataCoreMaterial.mat`
  - (Optional) Add Particle System for hit effect

---

### Step 7: Test Gameplay Loop (5 minutes)

1. **Save Scene** (Ctrl+S)
2. Click **Play** ▶️
3. **Expected Flow:**
   - Wave 1 starts automatically
   - 5 Phisher enemies spawn from random spawn points
   - Enemies pathfind toward DataCore (must have NavMesh!)
   - Kill enemies → earn points
   - All enemies dead → Shop opens
   - Close shop → Education popup
   - Close education → Wave 2 begins
   - Repeat until Wave 5
   - Victory or Core destroyed (Defeat)

4. **Debug if needed:**
   - No enemy movement? → Bake NavMesh!
   - Shop not opening? → Check WaveManager links
   - No points? → Check PlayerPoints component assigned

---

## 🎯 Quick Reference Component Links

```
WaveManager:
├─ spawnPoints[5] → SpawnPoint_1 through SpawnPoint_5
├─ enemyPrefabs[3] → Phisher, GhostAccount, DeepFake prefabs
├─ dataCore → DataCore GameObject
├─ educationUI → EducationManager
├─ shopPanel → ShopPanel
└─ victoryPanel → VictoryPanel

PlayerPoints:
└─ pointsText → PointsUI (TextMeshPro)

DataCoreHealth:
└─ coreMaterial → DataCoreMaterial.mat

EducationManager (ScamEducationUI):
├─ educationPanel → EducationPanel
├─ titleText → TitleText
├─ descriptionText → DescriptionText
└─ btnClose → BtnClose

LokTaShop:
├─ shopPanel → ShopPanel
├─ healthButton → BtnHealth
├─ shieldButton → BtnShield
├─ ammoButton → BtnAmmo
└─ player → Player GameObject (auto-finds with tag)
```

---

## 🚀 After Testing Works

1. **Save Prefabs:** Ensure all enemy prefabs saved in Assets/Prefabs/Enemies/
2. **Commit Changes:** Git commit all Unity scene files
3. **Build APK:** File → Build Settings → Android → Build
4. **Test on Device:** Install APK and verify gameplay works on phone

---

## ⚡ Fastest Path (If Rushed)

**Minimum to make it playable (15 minutes):**
1. ✅ Bake NavMesh (Step 1) - MUST DO
2. ✅ Link WaveManager spawn points + prefabs (Step 2)
3. ⏭️ Skip detailed UI for now
4. ✅ Test that enemies spawn and move
5. ✅ Fix any critical errors

**Can add UI polish later!**

---

## 📝 Current Scene Status

**File:** `Assets/Scenes/Scene_AI_Test_Working.unity/Scene_AI_Test_New.unity`

**Completion:** ~85%
- ✅ All GameObjects created
- ✅ All components added
- ✅ Materials assigned
- ⏳ NavMesh needs baking (1 click!)
- ⏳ Component references need linking (drag & drop)
- ⏳ UI panels need detail work

**The hard work is done - just needs final assembly in Unity Editor!** 🎮
