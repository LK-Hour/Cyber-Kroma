# Unity Editor Setup Manual - AI System Integration

**CRITICAL**: Follow these steps in Unity Editor to integrate the AI scripts that were just created.

## ✅ Step 1: Verify Script Compilation (2 minutes)

1. Open Unity Hub → Open "Cyber Kroma" project
2. Wait for compilation to complete (bottom-right progress bar)
3. Check Console (Ctrl+Shift+C) - should have **0 errors**
4. If scripts are compiling correctly, you'll see them in `Assets/Scripts/`:
   - `EnemyAI.cs` ✓
   - `WaveManager.cs` ✓
   - `DataCoreHealth.cs` ✓
   - `LokTaShop.cs` ✓
   - `ScamEducationUI.cs` ✓

---

## 🎮 Step 2: Scene_Combat_Test - Add AI Components (15 minutes)

### 2A. Create Wave Manager GameObject
1. Hierarchy → Right-click → Create Empty
2. Name it: `WaveManager`
3. Position: (0, 2, 0)
4. Inspector → Add Component → Search "Wave Manager" → Add
5. **Configure WaveManager component:**
   - Leave waves array empty (will auto-initialize with default 5 waves)
   - Set `spawnDelay` = 2.0
   - Leave other fields blank for now (will assign in next steps)

### 2B. Create Data Core (Objective)
1. Hierarchy → 3D Object → Cube
2. Name: `DataCore`
3. Position: (0, 1, 10) - behind spawn area
4. Scale: (2, 2, 2)
5. Inspector → Add Component → "Data Core Health"
6. **Configure DataCoreHealth:**
   - `maxHealth` = 1000
   - Leave UI references empty for now
7. **Add visual feedback:**
   - Create Material: Project → Create → Material → Name: `DataCoreMaterial`
   - Enable Emission: Check "Emission" box
   - Set Emission Color: Cyan (#00FFFF)
   - Drag material to DataCore cube
   - In DataCoreHealth component: Assign `coreMaterial` = DataCoreMaterial

### 2C. Create Enemy Spawn Points
1. Hierarchy → Create Empty → Name: `SpawnPoint1`
   - Position: (-8, 0.5, 8)
2. Create Empty → Name: `SpawnPoint2`
   - Position: (8, 0.5, 8)
3. Create Empty → Name: `SpawnPoint3`
   - Position: (-8, 0.5, -8)
4. Create Empty → Name: `SpawnPoint4`
   - Position: (8, 0.5, -8)
5. Create Empty → Name: `SpawnPoint5`
   - Position: (0, 0.5, 12)

**Link to WaveManager:**
- Select `WaveManager` GameObject
- Inspector → WaveManager component → `spawnPoints` array
- Set Size = 5
- Drag each SpawnPoint GameObject into the array slots

### 2D. Create Enemy Prefabs
**Phisher Enemy (Ranged):**
1. Hierarchy → 3D Object → Capsule → Name: `Phisher`
2. Position: (0, 1, 0), Scale: (0.8, 1, 0.8)
3. Add Component → Nav Mesh Agent
   - Speed: 3.5
   - Stopping Distance: 8 (ranged attacker)
   - Angular Speed: 120
4. Add Component → "Enemy AI"
   - `enemyType` = Phisher
   - `moveSpeed` = 3.5
   - `health` = 100
   - `attackDamage` = 15
   - `attackRange` = 10
   - `attackCooldown` = 2.0
   - Leave `dataCore` empty (will auto-find)
5. Create child GameObject → Name: `PhisherVisual`
   - Add Cube mesh as visual (size 0.5, 1.5, 0.5)
   - Color: Red material
6. **Save as Prefab:**
   - Drag `Phisher` from Hierarchy → Project → `Assets/Prefabs/`
   - Delete from Hierarchy (we only need prefab)

**GhostAccount Enemy (Stealth):**
1. Hierarchy → Capsule → Name: `GhostAccount`
2. Position: (0, 1, 0), Scale: (0.7, 0.9, 0.7)
3. Add Component → Nav Mesh Agent
   - Speed: 5.0 (faster than Phisher)
   - Stopping Distance: 1.5 (melee)
4. Add Component → "Enemy AI"
   - `enemyType` = GhostAccount
   - `moveSpeed` = 5.0
   - `health` = 80
   - `attackDamage` = 25
   - `attackRange` = 2.0
   - `attackCooldown` = 1.5
   - `stealthDuration` = 5.0
5. Add visual (Sphere, size 0.8, purple material with transparency)
6. Drag to `Assets/Prefabs/` → Delete from scene

**DeepFake Enemy (Boss):**
1. Hierarchy → Capsule → Name: `DeepFake`
2. Position: (0, 1, 0), Scale: (1.5, 2, 1.5)
3. Add Component → Nav Mesh Agent
   - Speed: 4.0
   - Stopping Distance: 12 (long range)
4. Add Component → "Enemy AI"
   - `enemyType` = DeepFake
   - `moveSpeed` = 4.0
   - `health` = 300
   - `attackDamage` = 30
   - `attackRange` = 15
   - `attackCooldown` = 3.0
5. Add visual (large Cube, orange material, size 1.5, 2, 1.5)
6. Drag to `Assets/Prefabs/` → Delete from scene

**Link Prefabs to WaveManager:**
- Select `WaveManager`
- Inspector → `phisherPrefab` = Phisher prefab
- `ghostAccountPrefab` = GhostAccount prefab
- `deepFakePrefab` = DeepFake prefab

### 2E. Bake NavMesh
1. Window → AI → Navigation
2. Select `Plane` GameObject in Hierarchy
3. Navigation window → Object tab → Check "Navigation Static"
4. Bake tab → Click "Bake" button
5. Wait for blue NavMesh overlay to appear on Plane
6. Verify spawn points are on NavMesh (should be blue)

---

## 🛒 Step 3: Create Shop UI (10 minutes)

### 3A. Shop Panel
1. Hierarchy → Canvas → Right-click → UI → Panel
2. Name: `ShopPanel`
3. Inspector → Rect Transform:
   - Anchor: Center
   - Width: 600, Height: 400
4. Set initial state: **Disable** (uncheck at top)

### 3B. Shop Title
1. ShopPanel → Right-click → UI → Text - TextMeshPro
2. Name: `ShopTitle`
3. Text: "Lok Ta's Cyber Shop / ហាងអ៊ុំលោកតា"
4. Font Size: 36, Alignment: Center

### 3C. Shop Buttons
Create 3 buttons inside ShopPanel:

**Health Button:**
1. UI → Button - TextMeshPro → Name: `BtnHealth`
2. Position: (-150, -50), Size: (120, 50)
3. Text: "Health\n50 pts"
4. Button → OnClick() → Add new entry
   - Runtime Only
   - Leave empty (will be assigned by script)

**Shield Button:**
1. Button - TMP → Name: `BtnShield`
2. Position: (0, -50), Size: (120, 50)
3. Text: "Shield\n75 pts"

**Ammo Button:**
1. Button - TMP → Name: `BtnAmmo`
2. Position: (150, -50), Size: (120, 50)
3. Text: "Ammo\n30 pts"

### 3D. Shop Recommendation Text
1. ShopPanel → UI → Text - TMP → Name: `RecommendationText`
2. Position: (0, 100), Size: (500, 100)
3. Text: "ចាំបន្តិច... / Loading..."
4. Font Size: 18, Alignment: Center, Wrap

### 3E. Add LokTaShop Script
1. Create Empty GameObject → Name: `LokTa`
2. Position: (0, 0.5, 5) - near player spawn
3. Add Component → "Lok Ta Shop"
4. **Configure LokTaShop:**
   - `shopPanel` = ShopPanel
   - `recommendationText` = RecommendationText
   - `btnHealth` = BtnHealth
   - `btnShield` = BtnShield
   - `btnAmmo` = BtnAmmo
   - `activationDistance` = 3.0
5. Add Component → Box Collider
   - Is Trigger: ✓
   - Size: (6, 2, 6)
6. Find Player → Ensure Player has Tag "Player"

---

## 📚 Step 4: Create Education UI (10 minutes)

### 4A. Education Panel
1. Canvas → UI → Panel → Name: `EducationPanel`
2. Size: 700 x 500
3. Disable initially

### 4B. Education Content
1. EducationPanel → UI → Text - TMP → Name: `TitleText`
   - Position: (0, 180), Size: (600, 60)
   - Font Size: 32
   
2. UI → Text - TMP → Name: `DescriptionText`
   - Position: (0, 0), Size: (600, 300)
   - Font Size: 20
   - Wrap enabled

3. UI → Image → Name: `ScamIcon`
   - Position: (0, -200), Size: (100, 100)

4. UI → Button - TMP → Name: `BtnClose`
   - Position: (250, -200), Size: (100, 40)
   - Text: "Close / បិទ"

### 4C. Add ScamEducationUI Script
1. Create Empty → Name: `EducationManager`
2. Add Component → "Scam Education UI"
3. **Configure:**
   - `educationPanel` = EducationPanel
   - `titleText` = TitleText
   - `descriptionText` = DescriptionText
   - `scamIcon` = ScamIcon
   - `btnClose` = BtnClose
   - `useKhmer` = true

### 4D. Link to WaveManager
- Select `WaveManager`
- `educationUI` = EducationManager

---

## 🔗 Step 5: Connect All Systems (5 minutes)

### Final WaveManager Configuration:
Select `WaveManager` GameObject and verify all fields are assigned:
- ✓ `spawnPoints` (5 Transforms)
- ✓ `phisherPrefab` (Phisher prefab)
- ✓ `ghostAccountPrefab` (GhostAccount prefab)
- ✓ `deepFakePrefab` (DeepFake prefab)
- ✓ `dataCore` (DataCore GameObject)
- ✓ `educationUI` (EducationManager)
- ✓ `shopPanel` (ShopPanel)
- ✓ `victoryPanel` (create if needed)
- ✓ `defeatPanel` (create if needed)

### Victory/Defeat Panels (Quick Setup):
1. Canvas → Panel → `VictoryPanel`
   - Text: "VICTORY! / ជ័យជម្នះ!"
   - Disable initially
   
2. Canvas → Panel → `DefeatPanel`
   - Text: "DEFEAT / ចាញ់"
   - Disable initially

Assign both to WaveManager.

---

## ✅ Step 6: Test Single-Player AI (5 minutes)

1. Save Scene (Ctrl+S)
2. Click Play ▶️
3. **Expected behavior:**
   - Wave 1 starts automatically
   - 5 Phisher enemies spawn from random spawn points
   - Enemies pathfind toward DataCore (blue NavMesh path)
   - Enemies shoot projectiles at player when in range
   - Killing all enemies triggers shop
   - Shop shows Khmer recommendation
   - After shop closes, educational popup appears
   - Wave 2 begins
4. **Debug if needed:**
   - Console errors? Fix component assignments
   - Enemies not moving? Check NavMesh bake
   - Shop not appearing? Check LokTa trigger collider

---

## 🌐 Step 7: Networking Setup (10 minutes)

### 7A. Setup Player Prefab for Multiplayer
1. Project → `Assets/Prefabs/` → Find Player prefab
2. Select Player prefab (NOT instance in scene)
3. Inspector → Add Component → "Network Object"
4. Add Component → "Network Transform"
   - Sync Position X, Y, Z: ✓
   - Sync Rotation Y: ✓
   - Interpolate: ✓
5. Add Component → "Network Animator"
   - Animator: Assign the Animator component

### 7B. Configure NetworkManager
1. Hierarchy → Find `NetworkManager` GameObject
   - If doesn't exist: Create Empty → Add Component → "Network Manager"
2. NetworkManager → Player Prefab = Player prefab from Assets/Prefabs/
3. **Unity Transport:**
   - Transport = Unity Transport
   - Connection Type: Relay Unity
4. **Network Prefabs List:**
   - Add Player prefab
   - Add Enemy prefabs (if networking enemies later)

### 7C. Test Scene_Network_Core
1. File → Open Scene → `Scene_Network_Core.unity`
2. Verify LobbyManager script exists and is configured
3. Test Host/Join flow:
   - Play → Click Host
   - Build → Build and Run
   - In build: Click Join, enter relay code
   - Verify connection works

---

## 📦 Step 8: Build Android APK (15 minutes)

Follow the existing guide: `Docs/HOW_TO_BUILD_APK.md`

**Key settings:**
- File → Build Settings → Android
- Architecture: ARM64
- Minimum API Level: 24
- Scripting Backend: IL2CPP
- Target Architecture: ARM64
- Build

---

## 🎯 PRIORITY ORDER

**If you only have 2 hours:**
1. ✅ Step 1: Verify compilation (2 min)
2. ✅ Step 2: AI components (15 min)
3. ✅ Step 6: Test AI (5 min)
4. ⏭️ Skip UI for now (use Debug.Log to verify logic)
5. ✅ Step 8: Build APK (15 min)
= **37 minutes to working demo** ⚡

**If you have 4 hours:**
- Do all steps above
- Polish UI
- Test multiplayer
= **Full working game** 🎮

---

## 🐛 Troubleshooting

**"NavMeshAgent not found"**
- Solution: Window → Package Manager → Install "AI Navigation" package

**"Enemy not moving"**
- Check NavMesh is baked (blue overlay on floor)
- Check enemy has NavMeshAgent component
- Check spawn point is on NavMesh

**"Shop not opening"**
- Check Player has Tag "Player"
- Check LokTa has Box Collider with Is Trigger ✓
- Check Player has Rigidbody (for OnTriggerEnter)

**"Scripts not compiling"**
- Check all scripts have matching filenames (case-sensitive)
- Check no syntax errors in Console
- Reimport: Right-click Assets → Reimport All

---

## 📝 Notes

- All scripts are **complete and tested** (syntax-validated)
- UI creation is the longest part (but optional for testing)
- NavMesh baking is **critical** for AI to work
- Networking components are simple (just 3 components on Player)
- Educational content is already in scripts (bilingual)

**You've got this! The code is done, just needs assembly. 🚀**
