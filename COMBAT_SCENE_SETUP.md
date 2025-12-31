# COMBAT SCENE SETUP GUIDE - Scene_Combat_Test

## 🎯 Complete Setup Checklist

### 1. NAVMESH - CRITICAL FOR ENEMY MOVEMENT

**Without this, enemies won't move!**

1. Select your **Ground** plane in Scene_Combat_Test
2. Add Component → **NavMesh Surface**
3. In NavMesh Surface component:
   - Agent Type: Humanoid
   - Collect Objects: All
   - Include Layers: Everything (or at least Default + Ground)
4. Click **"Bake"** button at the bottom
5. You should see a blue overlay on walkable surfaces

**Without NavMesh baked, enemies will stand still!**

---

### 2. WAVE MANAGER SETUP

**Select WaveManager GameObject:**

#### Spawn Points Array (Size: 5)
- SpawnPoint_1 at (-10, 0, 10)
- SpawnPoint_2 at (10, 0, 10)
- SpawnPoint_3 at (-10, 0, -10)
- SpawnPoint_4 at (10, 0, -10)
- SpawnPoint_5 at (0, 0, 15)

#### Enemy Prefabs Array (Size: 3)
- [0] Phisher prefab (Assets/Prefabs/Enemies/Phisher.prefab)
- [1] GhostAccount prefab (Assets/Prefabs/Enemies/GhostAccount.prefab)
- [2] DeepFake prefab (Assets/Prefabs/Enemies/DeepFake.prefab)

**If you don't have these prefabs yet:**
1. Create 3 capsules in scene
2. For each, add:
   - NavMeshAgent component
   - EnemyAI component
   - Set enemy type (Phisher, GhostAccount, or DeepFake)
3. Drag them to Assets/Prefabs/Enemies/ to create prefabs
4. Delete from scene
5. Assign to WaveManager

#### UI References:
- Wave Text: UI element showing "Wave X/3"
- Enemy Count Text: Shows "Enemies: X"
- Shop Panel: Shop UI panel GameObject
- Victory Panel: Victory screen GameObject

---

### 3. DATA CORE SETUP

**Create/Find DataCore GameObject:**
1. GameObject → 3D Object → Cube
2. Name: "DataCore"
3. Tag: "DataCore" (create tag if needed)
4. Position: (0, 1, 0) - center of map
5. Add Component → **DataCoreHealth**
6. Material: Cyan emissive material
7. Scale: (2, 2, 2) to make it visible

---

### 4. PLAYER SETUP

**Your player MUST have:**
- Tag: "Player"
- CharacterHealth component (with maxHealth set)
- CharacterShooting component (with currentAmmo/maxAmmo)
- CharacterMovement component
- Collider (for enemy detection)

---

### 5. COMBAT UI SETUP

**Create Canvas:**
1. Hierarchy → UI → Canvas
2. Canvas Scaler: Scale With Screen Size (1920x1080)

**Add UI Elements (all TextMeshPro):**

**Top Left Corner:**
- Health Text: "HP: 100/100"
- Ammo Text: "AMMO: 30/120"

**Top Right Corner:**
- Wave Text: "WAVE 1/3"
- Enemies Text: "ENEMIES: 5"
- Kills Text: "KILLS: 0"
- Points Text: "POINTS: 0"

**Create GameObject: "CombatUI"**
- Add Component → **CombatSceneUI**
- Assign all UI text references

---

### 6. ENEMY PREFAB REQUIREMENTS

**Each enemy prefab needs:**

✅ NavMeshAgent component:
   - Speed: 3.5 (Phisher), 5.0 (Ghost), 4.0 (DeepFake)
   - Stopping Distance: 2.0
   - Auto Braking: YES

✅ EnemyAI component:
   - Enemy Type: Set correctly
   - Max Health: 50/30/100
   - Move Speed: matches NavMeshAgent
   - Attack Damage: 10/5/30
   - Attack Range: 2/2/3
   - Data Core: Leave empty (auto-finds)

✅ Capsule Collider:
   - Is Trigger: NO
   - Radius: 0.5

✅ Rigidbody (optional but recommended):
   - Use Gravity: YES
   - Is Kinematic: NO
   - Constraints: Freeze Rotation X, Z

✅ Visual:
   - Capsule or imported model
   - Color: Red (Phisher), Purple (Ghost), Orange (DeepFake)

---

### 7. TESTING CHECKLIST

Start Play Mode and check:

1. ✅ Wave UI appears: "Wave 1/3"
2. ✅ Enemies spawn at spawn points after 3 seconds
3. ✅ Enemies have blue NavMesh path (Scene view)
4. ✅ Enemies move toward DataCore
5. ✅ Health/Ammo UI updates
6. ✅ Shooting enemies reduces their health
7. ✅ Enemies die and disappear
8. ✅ Kill counter increases
9. ✅ Wave completes when all enemies dead
10. ✅ Next wave starts after delay

---

### 8. COMMON ISSUES & FIXES

**🔴 Enemies don't move:**
- → NavMesh not baked! See step 1
- → NavMeshAgent component missing
- → Agent not on NavMesh (check ground has NavMesh)

**🔴 Enemies don't spawn:**
- → Enemy prefabs not assigned to WaveManager
- → Spawn points array empty
- → Check Console for errors

**🔴 No UI showing:**
- → UI text references not assigned to CombatSceneUI
- → Canvas missing Graphic Raycaster
- → EventSystem missing in scene

**🔴 Can't shoot enemies:**
- → Enemy colliders missing
- → Shooting script not working
- → Wrong layer setup

**🔴 Enemies don't die:**
- → TakeDamage() not being called
- → CharacterShooting not hitting
- → Check raycast layers

---

### 9. WAVE CONFIGURATION

**In WaveManager, waves auto-configure as:**

**Wave 1 (Tutorial):**
- 5 Phishers
- 0 Ghosts
- 0 DeepFakes

**Wave 2 (Medium):**
- 7 Phishers
- 3 Ghosts (invisible)
- 0 DeepFakes

**Wave 3 (Boss):**
- 8 Phishers
- 5 Ghosts
- 1 DeepFake (boss)

**Between waves:**
- 10 second break
- Shop opens
- Educational popup shows

---

### 10. FINAL TOUCHES

**Victory Panel:**
- Shows when all waves complete
- Button to return to main menu

**Defeat Panel:**
- Shows when DataCore destroyed
- Button to retry or main menu

**Shop Panel:**
- Health Potion: 50 points
- Shield Boost: 75 points
- Ammo Refill: 25 points

---

## 🚀 QUICK START (Minimum to get it working)

If you just want to see it work ASAP:

1. **Bake NavMesh** on ground (Step 1)
2. **Create 1 enemy capsule** with NavMeshAgent + EnemyAI
3. **Save as prefab**, assign to WaveManager slot [0]
4. **Create 1 spawn point**, assign to WaveManager
5. **Press Play**

You should see 1 enemy spawn and walk to DataCore!

Then expand from there.

---

## 📝 SCRIPT INTEGRATION

**WaveManager auto-integrates with:**
- PlayerPoints (awards points on kill)
- DataCoreHealth (game over when destroyed)
- ScamEducationUI (shows popups between waves)
- LokTaShop (opens between waves)
- CombatSceneUI (updates all combat UI)

**Everything connects automatically via FindObjectOfType!**

---

Need help? Check Unity Console for error messages. Most issues are:
1. Missing NavMesh (90% of movement issues)
2. Missing prefab assignments (80% of spawn issues)
3. Missing tag "Player" or "DataCore" (50% of targeting issues)
