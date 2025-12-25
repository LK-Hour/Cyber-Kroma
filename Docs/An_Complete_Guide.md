# AN'S COMPLETE TASK GUIDE
## AI, Wave Spawning & Shop System Implementation

**Your Role:** Logic Master (AI, Spawning & Shop)  
**Working Scene:** `Scene_AI_Test`  
**Goal:** Make enemies chase the core, create wave spawning system, and build "Lok Ta" shop  
**Timeline:** Complete in stages over 3 weeks

---

## 📋 PREREQUISITES CHECKLIST

Before starting, ensure you have:
- [ ] Unity project pulled from GitHub
- [ ] `Scene_AI_Test` scene opened (YOUR scene only)
- [ ] Unity Input System package installed
- [ ] TextMeshPro package imported

---

## 🎯 PART 1: ENEMY ASSETS & NAVIGATION (Week 1, Days 1-2)

### Step 1.1: Download Enemy Model

1. **Go to Mixamo.com**
   - Create a free account (use your Google/Facebook)
   - Search for **"Zombie"** or **"Mutant"** character
   - Click on the character you like

2. **Download the Character**
   - Click **"Download"**
   - Format: **FBX for Unity (.fbx)**
   - Pose: **T-Pose**
   - Skin: **With Skin** (checked)
   - Click Download

3. **Download Animations** (Download these separately)
   - Search **"Walking"** → Download (FBX, 30fps, Without Skin)
   - Search **"Idle"** → Download (FBX, 30fps, Without Skin)
   - Search **"Punching"** or **"Attack"** → Download (FBX, 30fps, Without Skin)

### Step 1.2: Import to Unity

1. **Create Folder Structure**
   ```
   Assets/
   └── An_Work/
       ├── Models/
       ├── Animations/
       ├── Scripts/
       └── Prefabs/
   ```
   - Right-click in Project → Create → Folder → Name it `An_Work`
   - Inside `An_Work`, create 4 more folders as shown above

2. **Import Files**
   - Drag the **T-Pose FBX** into `Assets/An_Work/Models/`
   - Drag all **Animation FBX files** into `Assets/An_Work/Animations/`
   - Unity will auto-import them

3. **Configure Model**
   - Select the T-Pose model in Project
   - In Inspector → **Rig Tab**
     - Animation Type: **Humanoid**
     - Click **Apply**

4. **Configure Animations**
   - Select **Walking.fbx** in Project
   - Inspector → **Rig Tab**
     - Animation Type: **None** (since it's animation-only)
   - Inspector → **Animation Tab**
     - Check **Loop Time** (so walking loops forever)
     - Click **Apply**
   - Repeat for Idle and Attack animations

### Step 1.3: Create Enemy Prefab

1. **Place Model in Scene**
   - Drag the T-Pose model from `Models/` folder into `Scene_AI_Test`
   - Rename it to **"Enemy_Malware"**
   - Reset Position to (0, 0, 0)

2. **Add Components**
   - Select Enemy_Malware in Hierarchy
   - Click **Add Component** → Search **"NavMesh Agent"**
   - Configure NavMesh Agent:
     - Speed: **3.5**
     - Stopping Distance: **2** (stops 2 units from target)
     - Radius: **0.5**
     - Height: **2**

3. **Create Animator**
   - Right-click in `An_Work/` → Create → **Animator Controller**
   - Name it **"Enemy_Animator"**
   - Select Enemy_Malware in Hierarchy
   - Add Component → **Animator**
   - Drag **Enemy_Animator** into the Controller slot

4. **Setup Animation States**
   - Double-click **Enemy_Animator** to open Animator window
   - Right-click in grid → **Create State → Empty**
   - Name it **"Idle"**
   - In Inspector, drag **Idle animation** into Motion field
   - Right-click → Create State → Name it **"Walking"**
   - Drag **Walking animation** into Motion field
   - Right-click → Create State → Name it **"Attack"**
   - Drag **Attack animation** into Motion field

5. **Create Parameters**
   - In Animator window, click **Parameters** tab (top left)
   - Click **+** → **Float** → Name it **"Speed"**
   - Click **+** → **Trigger** → Name it **"Attack"**

6. **Create Transitions**
   - Right-click **Idle** → Make Transition → Click **Walking**
   - Select the transition arrow
   - In Inspector:
     - Conditions: Add **Speed** Greater **0.1**
     - Uncheck **Has Exit Time**
   - Right-click **Walking** → Make Transition → Click **Idle**
   - Conditions: Add **Speed** Less **0.1**
   - Right-click **Walking** → Make Transition → Click **Attack**
   - Conditions: Add **Attack** trigger

---

## 🧠 PART 2: ENEMY AI SCRIPT (Week 1, Days 3-4)

### Step 2.1: Create the Data Core

1. **Create Target Object**
   - Right-click in Hierarchy → 3D Object → **Cube**
   - Rename to **"DataCore"**
   - Position: **(0, 1, 0)**
   - Scale: **(2, 2, 2)**

2. **Visual Setup**
   - Create new Material: Right-click → Create → Material
   - Name it **"Core_Material"**
   - Set Color to **Cyan** (0, 255, 255)
   - Check **Emission**
   - Emission Color: **Bright Cyan**
   - Drag material onto DataCore

3. **Add Tag**
   - Select DataCore
   - Inspector → Tag dropdown → **Add Tag**
   - Click **+** → Name: **"Core"**
   - Select DataCore again → Tag → **Core**

### Step 2.2: Setup Navigation Mesh

1. **Create Ground**
   - Hierarchy → 3D Object → **Plane**
   - Name: **"Ground"**
   - Position: (0, 0, 0)
   - Scale: **(5, 1, 5)** (makes it 50x50 units)

2. **Bake NavMesh**
   - Window → AI → **Navigation**
   - Select **Ground** in Hierarchy
   - In Navigation window → Object tab → Check **Navigation Static**
   - Click **Bake** tab
   - Agent Radius: **0.5**
   - Agent Height: **2**
   - Click **Bake** button at bottom
   - You should see a **blue overlay** on the ground (this is the walkable area)

### Step 2.3: Write Enemy AI Script

1. **Create Script**
   - In `Assets/An_Work/Scripts/`
   - Right-click → Create → C# Script
   - Name: **"EnemyAI"**

2. **Open Script** (double-click) and replace ALL content with:

```csharp
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform target; // The Data Core
    
    [Header("Settings")]
    public float attackRange = 2.5f;
    public float attackCooldown = 2f;
    public float damage = 10f;
    
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;
    
    // Components
    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime;
    
    void Start()
    {
        // Get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Set health
        currentHealth = maxHealth;
        
        // Find the Data Core automatically
        GameObject coreObject = GameObject.FindGameObjectWithTag("Core");
        if (coreObject != null)
        {
            target = coreObject.transform;
        }
        else
        {
            Debug.LogError("No DataCore found! Make sure it has 'Core' tag.");
        }
    }
    
    void Update()
    {
        if (target == null || currentHealth <= 0)
            return;
            
        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        // If close enough, attack. Otherwise, move toward target
        if (distanceToTarget <= attackRange)
        {
            AttackTarget();
        }
        else
        {
            MoveToTarget();
        }
        
        // Update animator speed parameter
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    
    void MoveToTarget()
    {
        // Tell NavMesh to move toward core
        agent.SetDestination(target.position);
    }
    
    void AttackTarget()
    {
        // Stop moving
        agent.SetDestination(transform.position);
        
        // Face the target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        // Attack if cooldown is ready
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            DealDamageToCore();
            lastAttackTime = Time.time;
        }
    }
    
    void DealDamageToCore()
    {
        // This will be connected to Core Health later
        Debug.Log("Enemy attacks core for " + damage + " damage!");
        
        // Try to damage the core
        DataCoreHealth coreHealth = target.GetComponent<DataCoreHealth>();
        if (coreHealth != null)
        {
            coreHealth.TakeDamage(damage);
        }
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject, 0.5f);
    }
}
```

3. **Attach Script to Enemy**
   - Select **Enemy_Malware** in Hierarchy
   - Drag **EnemyAI** script onto it (or Add Component → EnemyAI)
   - The script will auto-find the DataCore

4. **Test Movement**
   - Press **Play**
   - The enemy should walk toward the blue cube
   - Check Console for "Enemy attacks core" messages

---

## 💚 PART 3: DATA CORE HEALTH SYSTEM (Week 2, Day 1)

### Step 3.1: Create Core Health Script

1. **Create Script**
   - In `Assets/An_Work/Scripts/`
   - Right-click → Create → C# Script
   - Name: **"DataCoreHealth"**

2. **Code the Script:**

```csharp
using UnityEngine;

public class DataCoreHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 1000f;
    private float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Data Core initialized with " + maxHealth + " HP");
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log("Core HP: " + currentHealth + "/" + maxHealth);
        
        if (currentHealth <= 0)
        {
            CoreDestroyed();
        }
    }
    
    void CoreDestroyed()
    {
        Debug.Log("GAME OVER - Data Core Destroyed!");
        // This will trigger game over later
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
```

3. **Attach Script**
   - Select **DataCore** in Hierarchy
   - Drag **DataCoreHealth** script onto it

4. **Test Damage**
   - Press Play
   - Wait for enemies to reach the core
   - Watch Console for health decreasing

---

## 🌊 PART 4: WAVE SPAWNER SYSTEM (Week 2, Days 2-4)

### Step 4.1: Create Spawn Points

1. **Create Empty Objects**
   - Right-click in Hierarchy → Create Empty
   - Name: **"SpawnPoint_1"**
   - Position: **(-10, 0, 0)**
   - Repeat 3 more times:
     - SpawnPoint_2: **(10, 0, 0)**
     - SpawnPoint_3: **(0, 0, -10)**
     - SpawnPoint_4: **(0, 0, 10)**

2. **Visual Markers (Optional)**
   - Select SpawnPoint_1
   - In Inspector, click the **Cube icon** (top left)
   - Choose a **red color**
   - Repeat for all spawn points

### Step 4.2: Create Enemy Prefab

1. **Make Prefab**
   - Select **Enemy_Malware** in Hierarchy
   - Drag it into `Assets/An_Work/Prefabs/` folder
   - You now have a reusable enemy prefab

2. **Delete from Scene**
   - Delete Enemy_Malware from Hierarchy (we'll spawn it via script)

### Step 4.3: Create Wave Manager

1. **Create Manager Object**
   - Hierarchy → Create Empty
   - Name: **"WaveManager"**
   - Position: (0, 0, 0)

2. **Create Script**
   - In `Assets/An_Work/Scripts/`
   - Create C# Script: **"WaveManager"**

3. **Write the Code:**

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 0;
    public int maxWaves = 5;
    public float timeBetweenWaves = 10f;
    
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int baseEnemiesPerWave = 5;
    public float enemySpawnDelay = 1f;
    
    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    
    // Tracking
    private int enemiesAlive = 0;
    private bool waveInProgress = false;
    
    void Start()
    {
        Debug.Log("Wave Manager ready. Starting in 5 seconds...");
        Invoke("StartNextWave", 5f);
    }
    
    void Update()
    {
        // Check if wave is complete
        if (waveInProgress && enemiesAlive <= 0)
        {
            WaveComplete();
        }
    }
    
    void StartNextWave()
    {
        currentWave++;
        
        if (currentWave > maxWaves)
        {
            GameWon();
            return;
        }
        
        Debug.Log("=== WAVE " + currentWave + " STARTING ===");
        waveInProgress = true;
        
        // Calculate enemies for this wave (increases each wave)
        int enemiesToSpawn = baseEnemiesPerWave + (currentWave - 1) * 2;
        
        StartCoroutine(SpawnWave(enemiesToSpawn));
    }
    
    IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }
    
    void SpawnEnemy()
    {
        // Choose random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
        // Track it
        enemiesAlive++;
        
        // Subscribe to death event
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            StartCoroutine(WatchEnemy(enemy));
        }
    }
    
    IEnumerator WatchEnemy(GameObject enemy)
    {
        // Wait until enemy is destroyed
        while (enemy != null)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        // Enemy died
        enemiesAlive--;
        Debug.Log("Enemy defeated. Remaining: " + enemiesAlive);
    }
    
    void WaveComplete()
    {
        waveInProgress = false;
        Debug.Log("=== WAVE " + currentWave + " COMPLETE ===");
        Debug.Log("Next wave in " + timeBetweenWaves + " seconds...");
        
        // Trigger shop phase here later
        
        Invoke("StartNextWave", timeBetweenWaves);
    }
    
    void GameWon()
    {
        Debug.Log("=== ALL WAVES COMPLETE - VICTORY! ===");
    }
}
```

4. **Setup in Inspector**
   - Select **WaveManager** in Hierarchy
   - In Inspector:
     - **Enemy Prefab:** Drag `Enemy_Malware` from Prefabs folder
     - **Spawn Points:** Set Size to **4**
     - Drag each SpawnPoint (1-4) into the array slots

5. **Test Waves**
   - Press Play
   - Watch enemies spawn in waves
   - Check Console for wave progress

---

## 🛒 PART 5: "LOK TA" SHOP SYSTEM (Week 3)

### Step 5.1: Get Lok Ta Model

1. **Option A: Free Asset Store**
   - Open Asset Store (Window → Asset Store)
   - Search: **"Old Man"** or **"Elder"**
   - Download a free character
   - Import to project

2. **Option B: Mixamo**
   - Go to Mixamo.com
   - Search: **"Old Man"** or use any character
   - Download with T-Pose
   - Import to Unity

3. **Place in Scene**
   - Drag model into Scene_AI_Test
   - Name: **"LokTa_Shopkeeper"**
   - Position: **(-5, 0, -5)** (corner of map)
   - Scale: **(1.2, 1.2, 1.2)** (make him slightly bigger)

### Step 5.2: Create Holographic Material

1. **Create Material**
   - Right-click → Create → Material
   - Name: **"Hologram_Material"**

2. **Configure Material**
   - Rendering Mode: **Transparent**
   - Base Color: **Light Blue** (RGB: 100, 200, 255)
   - Alpha: **150** (semi-transparent)
   - Check **Emission**
   - Emission Color: **Bright Cyan**
   - Emission Intensity: **2**

3. **Apply Material**
   - Drag material onto LokTa model
   - He should now glow blue

### Step 5.3: Create Trigger Zone

1. **Add Collider**
   - Select LokTa_Shopkeeper
   - Add Component → **Sphere Collider**
   - Check **Is Trigger**
   - Radius: **3** (player must be within 3 units)

2. **Visualize Zone**
   - In Scene view, you'll see a green sphere
   - This is the shop interaction zone

### Step 5.4: Create Shop UI

1. **Create Canvas**
   - Hierarchy → UI → **Canvas**
   - Name: **"ShopUI"**
   - Canvas Scaler → UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: **1920 x 1080**

2. **Create Panel**
   - Right-click Canvas → UI → **Panel**
   - Name: **"ShopPanel"**
   - Color: **Black** with Alpha **200** (semi-transparent)

3. **Create Title**
   - Right-click ShopPanel → UI → **Text - TextMeshPro**
   - Name: **"ShopTitle"**
   - Text: **"LOK TA'S SHOP"**
   - Font Size: **48**
   - Alignment: **Center**
   - Position at top of panel

4. **Create Buttons**
   - Right-click ShopPanel → UI → **Button - TextMeshPro**
   - Name: **"BuyHealthButton"**
   - Text: **"Antivirus Potion (50 Credits)"**
   - Font Size: **24**
   
   - Duplicate (Ctrl+D) for second button
   - Name: **"BuyShieldButton"**
   - Text: **"2FA Shield (75 Credits)"**

5. **Layout Buttons**
   - Arrange buttons vertically in the panel
   - Leave space between them

6. **Hide Panel by Default**
   - Select **ShopPanel**
   - Uncheck the checkbox at top of Inspector (deactivates it)

### Step 5.5: Create Shop Logic Script

1. **Create Script**
   - `Assets/An_Work/Scripts/` → **"LokTaShop"**

2. **Write Code:**

```csharp
using UnityEngine;

public class LokTaShop : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    
    [Header("Shop Settings")]
    public int healthPotionCost = 50;
    public int shieldCost = 75;
    public float healAmount = 50f;
    
    private bool playerInRange = false;
    
    void Start()
    {
        // Make sure shop is hidden at start
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if player entered
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowShop();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        // Check if player left
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideShop();
        }
    }
    
    void ShowShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            Debug.Log("Lok Ta: Chau! Welcome to my shop!");
        }
    }
    
    void HideShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Debug.Log("Lok Ta: Come back anytime!");
        }
    }
    
    // These will be called by UI buttons
    public void BuyHealthPotion()
    {
        Debug.Log("Purchased Health Potion for " + healthPotionCost + " credits!");
        // TODO: Connect to player health system
        // TODO: Deduct credits from player
    }
    
    public void BuyShield()
    {
        Debug.Log("Purchased Shield for " + shieldCost + " credits!");
        // TODO: Activate shield on player
        // TODO: Deduct credits from player
    }
}
```

3. **Attach Script**
   - Select **LokTa_Shopkeeper**
   - Drag **LokTaShop** script onto it
   - In Inspector:
     - **Shop Panel:** Drag ShopPanel from Hierarchy

4. **Connect Buttons**
   - Select **BuyHealthButton**
   - Scroll to **Button** component
   - Click **+** under **OnClick()**
   - Drag **LokTa_Shopkeeper** into the object field
   - Dropdown: **LokTaShop → BuyHealthPotion()**
   
   - Repeat for **BuyShieldButton** → **BuyShield()**

### Step 5.6: Test Shop System

1. **Create Test Player**
   - Hierarchy → 3D Object → **Capsule**
   - Name: **"TestPlayer"**
   - Position: (0, 1, 0)
   - Add Component → **Character Controller**
   - Tag: **Player** (Important!)

2. **Add Simple Movement** (temporary for testing)
   - Create script: **"TestPlayerMovement"**

```csharp
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 move = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        controller.Move(move);
    }
}
```

3. **Test**
   - Press Play
   - Use WASD to move toward Lok Ta
   - Shop UI should appear when close
   - Click buttons to test purchases (check Console)

---

## 🔧 PART 6: INTEGRATION & POLISH (Week 3, Final Days)

### Step 6.1: Connect Systems

1. **Wave → Shop Integration**
   - In WaveManager script, find `WaveComplete()` function
   - Add this line before `Invoke("StartNextWave"...`:
   ```csharp
   // Enable shop during break
   GameObject.Find("LokTa_Shopkeeper").GetComponent<LokTaShop>().ShowShop();
   ```

2. **Enemy Death → Wave Tracking**
   - Already handled in WaveManager's `WatchEnemy()` coroutine

### Step 6.2: Create Prefabs for Team

1. **Package Your Work**
   - Select **Enemy_Malware** prefab
   - Right-click → Export Package
   - Include dependencies
   - Save as `An_EnemySystem.unitypackage`

2. **Package Wave System**
   - Select WaveManager, SpawnPoints
   - Export Package → `An_WaveSystem.unitypackage`

3. **Package Shop**
   - Select LokTa, ShopUI
   - Export Package → `An_ShopSystem.unitypackage`

### Step 6.3: Push to GitHub

**IMPORTANT: Only push YOUR folder!**

1. **Check Your Changes**
   ```
   Only these should be modified:
   - Assets/An_Work/ (all your files)
   - Assets/Scenes/Scene_AI_Test.unity (your scene)
   ```

2. **Commit & Push**
   - Open GitHub Desktop (or use terminal)
   - Commit message: "Added Enemy AI, Wave System, and Shop - An"
   - Push to repository

---

## ✅ FINAL CHECKLIST

Before declaring your work complete, verify:

- [ ] Enemy walks toward Data Core using NavMesh
- [ ] Enemy attacks Core when in range
- [ ] Core health decreases from attacks
- [ ] Waves spawn correctly (5, 7, 9, 11, 13 enemies)
- [ ] Wave count displayed in Console
- [ ] Lok Ta appears at correct location
- [ ] Shop UI shows when player approaches
- [ ] Shop UI hides when player leaves
- [ ] Both shop buttons respond to clicks
- [ ] All prefabs created and saved
- [ ] Work pushed to GitHub

---

## 🆘 TROUBLESHOOTING

### Problem: Enemies don't move
**Solution:**
- Check if NavMesh is baked (Window → AI → Navigation → Bake tab → Click Bake)
- Check if Ground has "Navigation Static" checked
- Check if enemy has NavMeshAgent component

### Problem: Enemies spin in circles
**Solution:**
- NavMesh Agent → Stopping Distance should be 2 or higher
- Make sure Core has a collider

### Problem: Shop UI doesn't show
**Solution:**
- Check if player has "Player" tag
- Check if LokTa has Sphere Collider with "Is Trigger" checked
- Check if ShopPanel is assigned in LokTaShop script

### Problem: Waves don't spawn
**Solution:**
- Check if all 4 spawn points are assigned in WaveManager
- Check if Enemy Prefab is assigned
- Look for errors in Console

### Problem: Git conflicts
**Solution:**
- Only work in `Scene_AI_Test` - never open other scenes
- Only modify files in `Assets/An_Work/`
- Pull before you start working each day

---

## 📞 COMMUNICATION WITH TEAM

### Daily Updates (Send in Group Chat)
**Format:**
```
An - Day X Update:
✅ Completed: [what you finished]
🚧 Working on: [current task]
❓ Blockers: [any problems]
```

### What to Share
- After completing Enemy AI → Share screenshot of enemy walking
- After completing Waves → Share video of wave spawning
- After completing Shop → Share screenshot of shop UI

### What NOT to Do
- ❌ Don't modify Pranha's player scripts
- ❌ Don't modify Longboren's Level_Design scene
- ❌ Don't edit scripts in Assets/Scripts/ (shared folder)
- ❌ Don't push Library/ or Temp/ folders

---

## 🎯 SUCCESS CRITERIA

Your work is complete when:

1. **Enemy AI works:**
   - Enemies spawn and walk to Core
   - Enemies attack Core and reduce health
   - Enemies can be destroyed

2. **Wave System works:**
   - 5 waves spawn with increasing difficulty
   - Enemies tracked correctly
   - Wave completion detected

3. **Shop works:**
   - UI appears when player approaches Lok Ta
   - Buttons respond to clicks
   - Integration point ready for player stats

4. **Team Integration ready:**
   - Prefabs exported
   - Code pushed to GitHub
   - No conflicts with other members' work

---

**Good luck, An! You can do this! 💪**

*If you get stuck, refer to Unity documentation or ask the team leader (Kimhour).*
