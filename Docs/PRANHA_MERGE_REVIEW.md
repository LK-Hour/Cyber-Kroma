# ⚠️ PRANHA'S MERGE REVIEW - DANGER ZONES DETECTED

## 🚨 CRITICAL ISSUES FOUND

Pranha's branch (`origin/Pranha`) has **DELETED files from other team members**:

### Files Pranha Deleted (DO NOT MERGE):
- ❌ `Assets/Assets/Independence_Monument_1214063220_texture.glb` (Boren's work!)
- ❌ `Assets/Editor/Khmer/` (Boren's Khmer tools!)
- ❌ `Assets/Plugins/README_HarfBuzz.md` (Boren's documentation)
- ❌ `.gitattributes` (Project config)

### Why This Happened:
Pranha likely pulled an old version of `main` and overwrote newer files.

---

## ✅ SAFE MERGE PLAN

### Step 1: Backup Current State
```bash
cd "/home/hour/Documents/CADT/Game Developement/Final Project/Cyber Kroma"

# Create backup branch
git branch backup-before-pranha-merge
git add -A
git stash
```

### Step 2: Cherry-Pick ONLY Pranha's Good Work

We'll manually extract ONLY his new additions without the deletions:

```bash
# Fetch latest
git fetch origin

# Check out ONLY the files Pranha actually worked on
# (His scene and his scripts)
git checkout origin/Pranha -- Assets/Scenes/Scene_Combat_Test.unity
git checkout origin/Pranha -- Assets/Scripts/CharacterMovement.cs
git checkout origin/Pranha -- Assets/Scripts/CharacterShooting.cs  
git checkout origin/Pranha -- Assets/Scripts/TouchLook.cs

# Get his Space Soldier assets (NEW additions)
git checkout origin/Pranha -- Assets/SciFi_Space_Soldier/

# Check status
git status
```

### Step 3: Review What You Got
```bash
# See what changed
git diff --cached

# If it looks good:
git commit -m "MERGE: Pranha's Scene_Combat_Test with player controls

- Added mobile touch shooting button
- Added CharacterMovement script
- Added Space Soldier 3D model and animations
- Updated Scene_Combat_Test with player prefab"
```

### Step 4: Push to Main
```bash
git push origin main
```

---

## 🔒 PROTECTED FILES (DON'T LET PRANHA OVERWRITE)

Keep these from being deleted:

1. **Boren's Work:**
   - `Assets/Editor/Khmer/`
   - `Assets/Independence_Monument_1214063220_texture.glb`
   - `Assets/TextMesh Pro/Fonts/Battambang*.ttf`

2. **Shared Config:**
   - `.gitattributes`
   - `Packages/manifest.json` (already modified by you for Burst fix)
   
3. **An's Work:**
   - `Assets/Scenes/Scene_AI_Test.unity` (if exists)

---

## 🎯 What Pranha Actually Contributed (Good Stuff)

### New Assets:
- ✅ `Assets/SciFi_Space_Soldier/` - Complete 3D player model with animations
- ✅ Space Soldier prefabs with LOD support
- ✅ Soldier animations (Rifle_Aiming, Walk, Run, Shoot, Reload)

### New Scripts:
- ✅ `CharacterMovement.cs` - Mobile joystick movement
- ✅ `CharacterShooting.cs` - Touch-based shooting
- ✅ `TouchLook.cs` - Camera control for mobile
- ✅ `CharacterHealth.cs` - Player HP system

### Scene Updates:
- ✅ `Scene_Combat_Test.unity` - Player setup with mobile controls

---

## 📝 RECOMMENDED ACTION

### Option 1: Safe Selective Merge (RECOMMENDED)
Run the commands in Step 2 above - cherry-pick only his work.

### Option 2: Ask Pranha to Fix His Branch
Tell Pranha:
> "Hey, your branch deleted Boren's Khmer files and Independence Monument. Can you:
> 1. Pull latest `main`
> 2. Merge it into your branch
> 3. Re-commit only YOUR new work
> 4. Push again?"

### Option 3: Manual Review in Unity
1. Create a new branch: `git checkout -b test-pranha-merge`
2. Merge: `git merge origin/Pranha`
3. Open Unity
4. Check if Boren's assets still exist
5. If broken, discard: `git merge --abort`

---

## ✅ FINAL CHECKLIST AFTER MERGE

After merging, verify:
- [ ] `Assets/Editor/Khmer/` folder exists
- [ ] Independence Monument model exists
- [ ] Pranha's Scene_Combat_Test opens without errors
- [ ] Space Soldier model is in project
- [ ] No broken prefab references
- [ ] Game builds successfully

---

## 🆘 IF SOMETHING BREAKS

```bash
# Undo the merge
git reset --hard backup-before-pranha-merge

# You're back to safety!
```

---

## 📞 NEXT STEPS

**Leader (You):** Should do one of these:

1. ✅ **Safe Path:** Run the selective checkout commands (Step 2) now
2. ⚠️ **Cautious Path:** Ask Pranha to rebase his branch first
3. ❌ **Risky Path:** Full merge (not recommended - will delete others' work)

**What do you want to do?**
