This is a perfect setup. With **4 people** (Leader + 3 Members), you can work very fast if you divide the work by **System**.

To work simultaneously without breaking the project, you must follow the **"One Person, One Scene"** rule until the very end.

Here is your **Master Plan** for the next 3 weeks.

---

### 🛑 BEFORE YOU START (The Leader's Job - 1 Hour)
**Leader (Kimhour):** You must do this first.
1.  Create the **Unity Project (3D URP)**.
2.  Set up **GitHub Repository** with a `.gitignore` for Unity.
3.  Invite Pranha, An, and Longboren to the Repo.
4.  **Create 4 Empty Scenes** in the `Scenes` folder:
    *   `Scene_Network_Core` (Leader's Scene)
    *   `Scene_Combat_Test` (Pranha's Scene)
    *   `Scene_AI_Test` (An's Scene)
    *   `Scene_Level_Design` (Longboren's Scene)
5.  Push to GitHub. Now everyone pulls and opens *their specific scene*.

---

### 👤 MEMBER 1: PRANHA
**Role:** The Combat Engineer (Player & Mechanics)
**Goal:** Make the player move, shoot, and get hit by "Viruses" on a mobile screen.
**Working Scene:** `Scene_Combat_Test`

#### Step 1: Assets & Visuals
*   **Download:** Go to **Mixamo.com**. Download a "Soldier/Sci-fi" character. Download animations: *Idle, Run_Forward, Run_Backward, Strafe_Left, Strafe_Right*.
*   **Setup:** Import into Unity. Create an "Animator Controller" and use a **Blend Tree** so the character switches from Idle to Run smoothly based on speed.

#### Step 2: Coding (The Player Controller)
*   **Input:** Install "Input System" package. Create an "Input Action Asset" with a **Left Stick** (Move) and **Right Touch Area** (Look).
*   **Script (`PlayerController.cs`):**
    *   Use `CharacterController.Move()` linked to the Left Stick.
    *   **Crucial:** Mobile screens are different sizes. Use `CanvasScaler` for your UI joysticks.

#### Step 3: The "Phishing" Attack (Specific GDD Feature)
*   **The Pop-Up Virus:**
    *   Create a Canvas. Add a Panel called `VirusPopup`. Add a button "X" to close it. Set the Panel to `SetActive(false)`.
    *   Write a function: `public void InfectPlayer() { VirusPopup.SetActive(true); }`.
*   **Shooting:**
    *   Create a simple Raycast script. When the "Fire" button is pressed -> Shoot a ray forward. If it hits an enemy, deal damage.

---

### 👤 MEMBER 2: AN
**Role:** The Logic Master (AI, Spawning & Shop)
**Goal:** Make enemies chase the core, make waves spawn, and build "Lok Ta".
**Working Scene:** `Scene_AI_Test`

#### Step 1: Enemy Assets & Navigation
*   **Asset:** Download a "Zombie" or "Mutant" from Mixamo to act as the "Malware."
*   **NavMesh:** Create a simple floor plane. Go to **Window > AI > Navigation**. Click **Bake**.
*   **Script (`EnemyAI.cs`):**
    *   Add `NavMeshAgent` to the enemy.
    *   Create a cube called "DataCore".
    *   Code: `agent.SetDestination(DataCore.transform.position);`

#### Step 2: The Wave Spawner
*   **Logic:** You don't want enemies appearing out of nowhere.
*   **Script (`WaveManager.cs`):**
    *   Create a List of Transform points (SpawnPoints).
    *   Use a Coroutine: `IEnumerator SpawnWave()`.
    *   Spawn 5 enemies, wait 5 seconds, spawn 5 more.
    *   **Win Condition:** Track `EnemiesAlive`. If 0, trigger the Shop.

#### Step 3: The "Lok Ta" Shop (Specific GDD Feature)
*   **Asset:** Find a "Old Man" or "Wizard" model (Free Asset Store). Tint him Blue/Holographic using a Material with low Opacity.
*   **Logic:**
    *   Create a Trigger Zone (Sphere Collider) around him.
    *   `OnTriggerEnter`: Show the Shop UI Buttons (Heal, Shield).
    *   `OnTriggerExit`: Hide the Shop UI.

---

### 👤 MEMBER 3: LONGBOREN
**Role:** The World Builder (Level Design, Lighting, UI)
**Goal:** Create "Cyber Phnom Penh" and the Atmosphere.
**Working Scene:** `Scene_Level_Design`

#### Step 1: The Landmarks (Art)
*   **Independence Monument:**
    *   Do **NOT** try to find a model online. You won't find it.
    *   Install **ProBuilder** (Package Manager).
    *   Build it using Shapes: Stack 5 "Pyramids" on top of each other. Add "Naga" heads using simple Cubes at the corners.
    *   **Texture:** Create a Material called `Neon_Glow`. Check "Emission" and pick a Hot Pink or Electric Blue color. Apply it to the edges.
*   **Wat Phnom:**
    *   Create a rounded hill using ProBuilder. Put a glowing cylinder (Stupa) on top.

#### Step 2: Lighting & Atmosphere
*   **Skybox:** Set the Skybox Material to **Black**.
*   **Post-Processing:**
    *   Add a "Volume" to the scene. Add **Bloom**.
    *   *Result:* Anything with an "Emission" material will now glow like a neon light. This is how you get the Cyberpunk look cheaply.

#### Step 3: The UI
*   **Font:** Download "Battambang" or "Siemreap" from Google Fonts. Import into Unity -> Right Click -> **Create TextMeshPro Font Asset**.
*   **HUD:** Create the Health Bar, Ammo Counter, and Wave Counter. Make them distinct and "Techy" (Green/Cyan colors).

---

### 👤 LEADER: KIMHOUR
**Role:** The Integrator (Network & Build)
**Goal:** Connect everyone's work and make sure it runs on Android.
**Working Scene:** `Scene_Network_Core`

#### Step 1: Netcode for GameObjects (NGO)
*   **Setup:** Install NGO package. Create the `NetworkManager` object.
*   **Transport:** Select `UnityTransport`.
*   **Relay:** Go to [Unity Gaming Services dashboard](https://dashboard.unity3d.com/). Create a Project. Turn on "Relay" (It's free).
*   **Script:** Write a `LobbyManager.cs`.
    *   Function: `CreateGame()` (Starts Host).
    *   Function: `JoinGame(code)` (Joins Client).

#### Step 2: The Player Prefab Networking
*   **Wait for Pranha:** Once Pranha finishes the Player Movement (Step 2), you take his Prefab.
*   **Add Network Components:**
    *   `NetworkObject`: So the server knows it exists.
    *   `NetworkTransform`: To sync position X,Y,Z to other players.
    *   `NetworkAnimator`: To sync the running animation.

#### Step 3: The Android Build
*   **Settings:** Switch Platform to Android.
*   **Minimum API:** Set to Android 7.0 (Nougat) as per GDD.
*   **Architecture:** Set to ARM64.
*   **Test Build:** Try to build an empty scene to your phone *now* to ensure your Java/JDK tools are working. Do not wait until Dec 23.

---

### 🔗 HOW TO MERGE (The Final Workflow)

**Every Friday (or every 3 days):**

1.  **Pranha** makes his Player into a **Prefab** and pushes to Git.
2.  **An** makes his Enemy into a **Prefab** and pushes to Git.
3.  **Longboren** saves his Map Scene and pushes.
4.  **Kimhour (Leader)** pulls everyone's changes.
    *   Open Longboren's `Scene_Level_Design`.
    *   Drag Pranha's `PlayerPrefab` into the NetworkManager.
    *   Drag An's `EnemySpawner` into the map.
    *   **Build the APK.**
    *   Send the APK to the group chat for testing.

**Conflict Warning:**
If Pranha changes the *Scene* that Longboren is working on, you will lose work.
*   **Rule:** Pranha only touches the *Player Prefab*. An only touches the *Enemy Prefab*. Longboren only touches the *Map Scene*.

Go! You have 3 weeks. Start with the Setup tonight.