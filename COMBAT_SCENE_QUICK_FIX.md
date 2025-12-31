# 🎮 COMBAT SCENE QUICK FIX - Get It Working in 5 Minutes!

## The Problem
- ❌ Enemies not moving (standing still)
- ❌ WaveManager only showing debug logs
- ❌ No UI for ammo/kills/health

## The Solution
**3 main issues:**
1. **NavMesh not baked** (enemies can't navigate)
2. **Enemy prefabs not assigned** to WaveManager
3. **UI elements not created** in scene

---

## 🚀 FASTEST FIX (5 Steps)

### Step 1: Bake NavMesh (CRITICAL!)
1. Open Scene_Combat_Test in Unity
2. Select your **Ground** plane (the floor/terrain)
3. In Inspector, click **Add Component**
4. Search for "NavMesh Surface" and add it
5. Click the **"Bake"** button at the bottom
6. You should see a **blue overlay** on the ground

**Without this, enemies will NEVER move!**

---

### Step 2: Assign Enemy Prefabs
1. Find/Create **WaveManager** GameObject in scene
2. In Inspector, find the **Enemy Prefabs** array
3. Set Size: **3**
4. Drag prefabs from Assets/Prefabs/Enemies/:
   - Element 0: **Phisher**
   - Element 1: **GhostAccount**
   - Element 2: **DeepFake**

---

### Step 3: Create Spawn Points
1. Create empty GameObject: **"SpawnPoints"**
2. Create 5 child objects:
   - SpawnPoint_1 at position (-10, 0.5, 10)
   - SpawnPoint_2 at position (10, 0.5, 10)
   - SpawnPoint_3 at position (-10, 0.5, -10)
   - SpawnPoint_4 at position (10, 0.5, -10)
   - SpawnPoint_5 at position (0, 0.5, 15)
3. Drag all 5 into WaveManager's **Spawn Points** array

---

### Step 4: Tag DataCore
1. Find your DataCore GameObject (the cube in center)
2. Set Tag to **"DataCore"** (create tag if needed)
3. Make sure it has **DataCoreHealth** component

---

### Step 5: Press Play and Test!
- Enemies should spawn after 3 seconds
- They should walk toward DataCore
- Shoot them to kill them
- Wave completes when all enemies dead

---

## 🎨 OPTIONAL: Add Combat UI

If you want the ammo/kills/health UI:

### Quick UI Setup
1. I already created **CombatSceneUI.cs** script
2. Create UI elements in Canvas:

**Right-click Canvas → UI → Text - TextMeshPro:**
- "HealthText" - position (-800, 500)
- "AmmoText" - position (-800, 450)
- "KillsText" - position (700, 500)
- "WaveText" - position (700, 450)
- "EnemiesText" - position (700, 400)
- "PointsText" - position (700, 350)

3. Create GameObject "CombatUI" under Canvas
4. Add component **CombatSceneUI**
5. Drag all the text elements into the script's fields

---

## ⚠️ Common Issues

### Enemies still not moving?
- ✅ Check Ground has NavMeshSurface component
- ✅ Check NavMesh is baked (blue overlay visible)
- ✅ Check enemy prefabs have NavMeshAgent component
- ✅ Check Console for "agent.isOnNavMesh = false" errors

### Enemies not spawning?
- ✅ Check enemyPrefabs array has 3 prefabs assigned
- ✅ Check spawnPoints array has 5 spawn points
- ✅ Check Console for null reference errors

### Enemies spawning but frozen?
- ✅ NavMesh NOT baked (most common issue!)
- ✅ Ground doesn't have NavMeshObstacle or wrong layer
- ✅ Spawn points are outside NavMesh area

### UI not showing?
- ✅ Canvas exists in scene
- ✅ Canvas has CanvasScaler component
- ✅ Text elements are children of Canvas
- ✅ CombatSceneUI references assigned

---

## 🤖 AUTOMATED SETUP (Alternative)

I created an automated setup script:

1. Add **CombatSceneSetup.cs** component to any GameObject
2. In Inspector, click **"Setup Combat Scene"** button
3. It will auto-create:
   - DataCore
   - Spawn points
   - UI elements
   - NavMesh Surface (but you still need to BAKE)

After running, you still need to:
- Manually **bake NavMesh**
- Assign **enemy prefabs** to WaveManager

---

## 📋 Verification Checklist

Before pressing Play, verify:

- [ ] Ground has NavMeshSurface and is baked (blue overlay)
- [ ] WaveManager has 3 enemy prefabs assigned
- [ ] WaveManager has 5 spawn points assigned
- [ ] DataCore exists with "DataCore" tag
- [ ] Player exists with "Player" tag
- [ ] Player has CharacterHealth, CharacterShooting, CharacterMovement
- [ ] Canvas exists in scene
- [ ] EventSystem exists in scene

---

## 🎯 Expected Behavior

When everything works:

1. Press Play
2. After 3 seconds: 5 Phisher enemies spawn
3. Enemies walk toward DataCore (blue capsules moving)
4. Shoot enemies - they take damage and die
5. Kill counter increases
6. When all 5 dead: Wave 1 complete
7. Shop opens (10 second break)
8. Wave 2 starts: 7 Phishers + 3 Ghosts
9. Repeat until all waves complete
10. Victory screen shows!

---

## 🔧 Debug Tips

**Enable Gizmos in Scene View:**
- Click "Gizmos" button (top right)
- Enable NavMesh display
- You should see blue walkable areas

**Check Console Messages:**
- "Wave 1 starting!" - Good!
- "Spawning X enemies" - Good!
- "Enemy spawned: Phisher" - Good!
- "enemy.isOnNavMesh = false" - BAD! NavMesh not baked!
- "spawnPoints is null" - BAD! Spawn points not assigned!
- "enemyPrefabs is null" - BAD! Prefabs not assigned!

---

## 📞 Still Not Working?

Check these files exist and are correct:
- ✅ [WaveManager.cs](Assets/Scripts/WaveManager.cs)
- ✅ [EnemyAI.cs](Assets/Scripts/EnemyAI.cs)
- ✅ [CombatSceneUI.cs](Assets/Scripts/CombatSceneUI.cs)
- ✅ [Phisher.prefab](Assets/Prefabs/Enemies/Phisher.prefab)
- ✅ [GhostAccount.prefab](Assets/Prefabs/Enemies/GhostAccount.prefab)
- ✅ [DeepFake.prefab](Assets/Prefabs/Enemies/DeepFake.prefab)

If any missing, let me know!

---

## 🎮 Game Design Per GDD

**Wave 1 (Tutorial):**
- 5 Phisher enemies (basic ranged)
- Slow spawn rate
- Easy difficulty

**Wave 2 (Medium):**
- 7 Phishers + 3 GhostAccounts (invisible)
- Faster spawn
- Medium difficulty

**Wave 3 (Boss):**
- 8 Phishers + 5 Ghosts + 1 DeepFake (boss)
- Fast spawn
- Hard difficulty

**Between Waves:**
- 10 second break
- Shop opens (buy health/ammo/shields)
- Educational popup about cybersecurity

**Victory:**
- Complete all 3 waves
- DataCore survives

**Defeat:**
- DataCore health reaches 0
- Game over screen

---

Need more help? Check the console logs - they'll tell you exactly what's missing!
