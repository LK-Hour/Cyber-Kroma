# 🎮 Scene_Combat_Test Setup Guide
## Complete Step-by-Step Instructions for Pranha's Combat Scene

This guide will help you properly configure all the missing references in Scene_Combat_Test.unity to make the combat system work correctly.

---

## 📋 **Overview**
Pranha's combat scene includes:
- Player character with movement, shooting, and health systems
- Mobile controls (joystick + touch look + shoot button)
- Space Soldier 3D character with animations
- Combat testing environment

**⚠️ Important:** You must assign references manually because scene files don't automatically link to assets when merged from Git branches.

---

## 🎯 **STEP 1: Setup the Player GameObject**

### 1.1 Find the Player
1. Open **Scene_Combat_Test** (double-click in Project → Assets/Scenes/)
2. In the **Hierarchy** window, find the GameObject named **"Player"**
   - **Note:** The Player is actually the **Space_Soldier_A prefab** instance
3. Click on it to select it

### 1.2 Verify Components
The Player is a **3D soldier character model** with these components already built-in:
- ✅ Transform
- ✅ Animator (already on the prefab)
- ✅ Character Controller
- ✅ Character Movement (Script)
- ✅ Character Shooting (Script)
- ✅ Character Health (Script)

**Important:** The scripts are added AT THE ROOT of the Player GameObject, not on a child.

If any script says **"Missing (Mono Script)"**, you need to restart Unity first.

---

## 🎬 **STEP 2: Assign Animator Controller**

### 2.1 Locate Animator Component
1. With **Player** selected (the Space_Soldier_A root), scroll in Inspector to find **Animator** component
   - The Animator is **directly on the Player GameObject** (not on a child)
2. Look for the **Controller** field (it's probably empty or says "None")

### 2.2 Assign the Controller
1. In the **Project** window, navigate to:
   ```
   Assets → SciFi_Space_Soldier → Resources → Space_Soldier → Controller&Masks
   ```
2. Find the file: **`Soldier_Controller.controller`**
3. **Drag and drop** it into the **Controller** field of the Animator component on the Player

✅ **Result:** The "Animator is not playing an AnimatorController" warnings will stop.

---

## 🕹️ **STEP 3: Assign CharacterMovement References**

With **Player** still selected, find the **Character Movement (Script)** component in Inspector.

### 3.1 Mobile Controls Section
| Field Name | What to Assign | How to Find It |
|------------|--------Need to ADD joystick | See below - you need to add a joystick from Assets/Joystick Pack/Prefabs |
| **Touch Field** | TouchZone GameObject | Hierarchy → Canvas → Find "TouchZone" → Drag to field |

**To Add Joystick:**
1. Go to Project: **Assets/Joystick Pack/Prefabs**
2. Drag **Fixed Joystick** (or Dynamic Joystick) into the **Canvas** in Hierarchy
3. Position it in the bottom-left corner of the screen
4. Drag this joystick to the **Move Joystick** field in CharacterMovement script

### 3.2 Camera Settings Section
| Field Name | What to Assign | How to Find It |
|------------|----------------|----------------|
| **Player Camera** | Main Camera in scene | Hierarchy → Find "Main Camera" (it's a separate GameObject, NOT a child of Player)
| **Player Camera** | Main Camera | Hierarchy → Expand "Player" → Find "Main Camera" child → Drag to field |

**Settings (leave as default):**
- Look Sensitivity: `0.2`
- Look X Limit: `90`
- Camera Smoothness: `15`

### 3.3 Movement Settings (default values)
- Walk Speed: `3`
- Run Speed: `6`
- Gravity: `25`

### 3.4 Roll Settings (default values)
- Roll Speed: `10`Click the ⊙ button → Select "Animator" from the **same Player GameObject** |

**💡 Tip:** In this scene, the Animator is **directly on the Player GameObject** (the Space_Soldier_A root), NOT on a child
### 3.5 Animation Section
| Field Name | What to Assign | How to Find It |
|------------|----------------|----------------|
| **Animator** | The Animator component | Expand "Player" → Find the child object with the 3D model → Drag the GameObject with Animator |

**💡 Tip:** The Animator is usually on a child GameObject (the 3D soldier model), not on the root Player GameObject.

---

## 🔫 **STEP 4: Assign CharacterShooting References**

Find the **Character Shooting (Script)** component on the Player.

### 4.1 Setup Section
| Field Name | What to Assign | How to Find It |
|------------|----------------|----------------|
| **Player Camera** | Main Camera | Hierarchy → Find "Main Camera" (separate GameObject in scene root) |
| **Animator** | Animator component | Click ⊙ button → Select "Animator" from **same Player GameObject** |
| **Fire Point** | Muzzle/gun tip position | **Look inside Player** → Find "ShootFX" child object → Drag to field |
| **Bullet Trail Prefab** | Line Renderer prefab | Leave empty for now (optional visual effect) |

**💡 Note:** The Space_Soldier_A prefab already has a **"ShootFX"** child GameObject - use that as the Fire Point!

### 4.2 Gun Stats (default values)
- Damage: `10`
- Range: `100`
- Fire Rate: `0.15`
- Max AmUsing the ShootFX as Fire Point
The Space_Soldier_A prefab already has a "ShootFX" child:
1. In Hierarchy, expand **Player**
2. Find the **"ShootFX"** child GameObject
3. Drag **ShootFX** to the **Fire Point** field in CharacterShooting script
4. This is already positioned at the gun barrel
3. Position it at the gun's barrel/muzzle (use Move tool)
4. Drag **FirePoint** to the Character Shooting script's Fire Point field

---

## ❤️ **STEP 5: Assign CharacterHealth References**

Find the **Character Health (Script)** component on the Player.

### 5.1 Stats Section (default values)
- Max Health: `100`
- Respawn Delay: `5`

### 5.2 References Section
| Field Name | What to Assign | How to Find It |
|------------|----------------|----------------|
| **Respawn Point** | SpawnPoint GameObject | Hierarchy → Find **"SpawnPoint"** (already exists in scene) → Drag to field |
| **Animator** | Animator component | Click the ⊙ button → select "Animator" from the same GameObject |
| **Movement Script** | CharacterMovement | Click the ⊙ button → select "CharacterMovement" from the same GameObject |
| **Shooting Script** | CharacterShooting | Click the ⊙ button → select "CharacterShooting" from the same GameObject |
| **Character Controller** | CharacterController | Click the ⊙ button → select "CharacterController" from the same GameObject |

**💡 Tip:** The scene already has a "SpawnPoint" GameObject - use that instead of creating a new one!

---

## 📱 **STEP 6: Setup Mobile UI Controls**

### 6.1 Find the Canvas
1. In **Hierarchy**, find the **Canvas** GameObject
2. It currently contains these UI elements:
   - **TouchZone** (for camera rotation/touch look)
   - **Btn_Shoot** (shoot button)
   - **Btn_Reload** (reload button)
   - **Btn_Roll** (roll/dodge button)
   - **Crosshair** (center crosshair image)
   - **Tex**Btn_Shoot** in Hierarchy (under Canvas)
2. In **Inspector**, find the **Event Trigger** component
3. You need to add two events:

**Add Pointer Down Event:**
1. Click **Add New Event Type** → Select **PointerDown**
2. Click **+ (plus)** under PointerDown to add callback
3. Drag the **Player** GameObject to the object field
4. Click dropdown (No Function) → **CharacterShooting** → **StartFiring()**

**Add Pointer Up Event:**
1. Click **Add New Event Type** → Select **PointerUp**
2. Click **+ (plus)** under PointerUp to add callback
3. Drag the **Player** GameObject to the object field
4. Click dropdown → **CharacterShooting** → **StopFiring()**

✅ **Result:** Now the Btn_Shoot
1. Add another event (or switch event type to PointerUp)
2. Drag the **Player** GameObject to the object field
3. Click dropdown → **CharacterShooting** → **StopFiring()**

✅ **Result:** Now the shoot buttois:
```
Player (Space_Soldier_A prefab root)
├── SK_Soldier_Helmet (child mesh)
├── SK_Soldier_Legs (child mesh)
├── SK_Soldier_Torso (child mesh)
├── Root (skeleton bones hierarchy)
├── ShootFX (use as Fire Point)
└── Animator (component on the Player root itself)
```

**Important Notes:**
- The Player **IS** the Space_Soldier_A prefab (not a capsule with soldier as child)
- The Animator is on the **Player root**, not on a child
- All scripts attach to the **Player root**
- Main Camera is a **separate GameObject** in the scene, NOT a child of Player

### 7.2 The Setup is Already There!
The Space_Soldier_A prefab is already complete with:
- ✅ 3D model meshes (Helmet, Legs, Torso)
- ✅ Skeleton/bones for animation
- ✅ Animator component
- ✅ ShootFX position marker

You just need to **add the scripts and assign references**!
### 7.2 Assign the 3D Model (if missing)
If there's no soldier model under Player:
1. Navigate in Project: **Assets → SciFi_Space_Soldier → Resources → Space_Soldier → Prefabs**
2. Find **Space_Soldier_A**, **B**, or **C**
3. Drag one into the Hierarchy **as a child of Player**
4. Position it at `(0, 0, 0)` relative to Player
5. Now assign this child's Animator to all the script fields

---

## 🧪 **STEP 8: Testing**

### 8.1 Test in Unity Editor
1. Click **Play** button
2. **Check console** - no more errors should appear
3. Test controls:
   - **W/A/S/D** or **Joystick**: Move player
   - **Mouse** or **Touch drag**: Look around
   - **Left Click** or **Shoot Button**: Fire weapon
   - **Press K**: Test death/respawn system

### 8.2 What Should Work
✅ Player moves smoothly
✅ Camera rotates with mouse/touch
✅ Animations play (Idle, Walk, Run)
✅ Shooting creates raycast hits
✅ Health system responds to damage (test with K key)

### 8.3 Common Issues

**Issue:** "Animator is not playing an AnimatorController"
- **Fix:** Assign Soldier_Controller.controller to Animator component

**Issue:** "NullReferenceException" in CharacterMovement
- **Fix:** Assign moveJoystick and touchField from Canvas

**Issue:** Camera doesn't move
- **Fix:** Assign playerCamera field in CharacterMovement

**Issue:** Character doesn't animate
- **Fix:** Assign Soldier_Controller.controller to the Animator on the Player root

**Issue:** Shooting doesn't work
- **Fix:** Assign Player Camera and Fire Point in CharacterShooting

---

## 📝 **Quick Reference Checklist**

Print this and check off as you go:

### Player GameObject
- [ ] Character Controller coFixed Joystick (ADD from Joystick Pack/Prefabs)
- [ ] Touch Field = Canvas/TouchZone
- [ ] Player Camera = Main Camera (separate GameObject in scene root)
- [ ] Animator = Player's own Animator component (on root

### Animator Setup
- [ ] Animator component exists (on 3D model child)
- [ ] Controller field = Soldier_Controller.controller
Main Camera (separate GameObject)
- [ ] Animator = Player's own Animator component
- [ ] Fire Point = Player/ShootFX (already exists in prefab
- [ ] Touch Field = Canvas/TouchField
- [ ] Player Camera = Player/Main Camera
- [ ] Animator = PlayeSpawnPoint GameObject (already exists in scene)
- [ ] Animator = Player's own Animator component
- [ ] Movement Script = CharacterMovement on same GameObject
- [ ] Shooting Script = CharacterShooting on same GameObject
- [ ] Character Controller = CharacterController on same GameObject

### Mobile UI
- [ ] Joystick = ADD Fixed Joystick to Canvas from Assets/Joystick Pack/Prefabs
- [ ] TouchZone exists in Canvas ✅
- [ ] Btn_Shoot exists in Canvas ✅
- [ ] Btn_Shoot has Event Trigger component
- [ ] PointerDown event → Player.CharacterShooting.StartFiring()
- [ ] PointerUp eventracterShooting on same GameObject
- [ ] Character Controller = CharacterController on same GameObject

### Mobile UI
- [ ] Joystick exists in Canvas
- [ ] TouchField exists in Canvas
- [ ] ShootButton exists in Canvas
- [ ] ShootButton PointerDown → Player.CharacterShooting.StartFiring()
- [ ] ShootButton PointerUp → Player.CharacterShooting.StopFiring()

---

## 🚀 **After Setup**

Once everything is assigned:

1. **Save the scene:** Ctrl+S or File → Save
2. **Save the project:** Ctrl+Shift+S or File → Save Project
3. **Test thoroughly** in Play mode
4. **Commit to Git:**
   ```bash
   git add Assets/Scenes/Scene_Combat_Test.unity
   git commit -m "CONFIG: Setup all references for Pranha's combat scene"
   git push origin main
   ```

---

## 🆘 **Need Help?**

If you get stuck:
1. Check the **Console** window for specific error messages
2. Verify each field is assigned (not "None")
3. Make sure you're assigning the correct type (Camera to Camera field, etc.)
4. Restart Unity if scripts still show as "Missing"
Player **IS** the Space_Soldier_A prefab itself. The Animator is on the Player root GameObject, and Main Camera is a separate object in the scene
**Remember:** The soldier model must be a **child** of the Player GameObject, and the Animator component lives on that child, not on the root Player!

---

**Good luck! 🎮 Once this is setup, Pranha's combat system will be fully functional!**
