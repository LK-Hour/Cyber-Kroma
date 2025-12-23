# 🔀 Safe Merge Guide for Unity Team

## Problem
You want to merge teammates' work but avoid overwriting YOUR scenes/scripts.

## ⚠️ Unity Conflict Zones (DON'T MERGE BLINDLY)
- **Scenes** (`*.unity`) - Each person should work in their own scene
- **ProjectSettings/** - Unity config files (can cause crashes if overwritten)
- **Prefabs** - If you both edited the same prefab, conflict!

## ✅ Safe to Merge (No Conflicts)
- **New scripts** they added (`Assets/Scripts/NewScript.cs`)
- **New prefabs** they created
- **New assets** (models, textures, sounds)
- **New scenes** (their own test scenes)

---

## 📋 Step-by-Step: Merge Boren's Work Safely

### 1. **Backup Your Current Work**
```bash
# Create a backup branch (just in case)
git branch backup-before-merge
```

### 2. **See What Boren Changed**
```bash
git diff --name-status main..origin/boren
```

**Look for:**
- `A` (Added) - New files = **SAFE** ✅
- `M` (Modified) - Changed files = **REVIEW NEEDED** ⚠️
- `D` (Deleted) - Removed files = **DANGEROUS** ❌

### 3. **Option A: Selective File Checkout (Safest)**

Only grab the NEW files Boren added:

```bash
# Checkout ONLY specific files from Boren's branch
git checkout origin/boren -- Assets/Materials/
git checkout origin/boren -- "Assets/TextMesh Pro/Fonts/KhmerOS_Battambang.ttf"
git checkout origin/boren -- Assets/Independence_Monument_1214063220_texture.glb

# Commit these additions
git commit -m "ADD: Boren's materials and font assets"
```

### 4. **Option B: Merge with Manual Review (More Complete)**

```bash
# Start merge but DON'T commit automatically
git merge origin/boren --no-commit --no-ff
```

**What happens:**
- Git stages ALL changes from Boren's branch
- You can review/modify BEFORE committing

**Check what's staged:**
```bash
git status
```

**Undo changes to files you want to KEEP YOUR VERSION:**
```bash
# Example: Keep YOUR version of Scene_Network_Core
git reset HEAD Assets/Scene_Network_Core.unity
git checkout -- Assets/Scene_Network_Core.unity

# Example: Keep YOUR ProjectSettings
git reset HEAD ProjectSettings/
git checkout -- ProjectSettings/
```

**Commit the rest:**
```bash
git commit -m "MERGE: Boren's assets (kept my scenes intact)"
```

### 5. **If Conflicts Appear (Unity Scenes)**

Unity scenes are YAML files - conflicts look like:
```yaml
<<<<<<< HEAD
  m_LocalPosition: {x: 0, y: 1, z: 0}
=======
  m_LocalPosition: {x: 5, y: 1, z: 0}
>>>>>>> origin/boren
```

**DON'T manually edit Unity files!**

Instead:
```bash
# Keep YOUR version
git checkout --ours Assets/Scenes/YourScene.unity

# OR keep THEIR version
git checkout --theirs Assets/Scenes/TheirScene.unity

# Then mark as resolved
git add Assets/Scenes/YourScene.unity
```

---

## 📋 Step-by-Step: Merge Pranha's Work

**Warning:** Pranha's branch shows `D` (deletions) of your files. This is dangerous!

### Safe Approach:
```bash
# 1. Check what Pranha added (not deleted)
git diff --name-status main..origin/Pranha | grep "^A"

# 2. If there are NEW files you want, cherry-pick them:
git checkout origin/Pranha -- Assets/Scripts/PlayerController.cs

# 3. Commit
git commit -m "ADD: Pranha's player controller"
```

**DON'T do a full merge if you see many `D` (deletions)** - it will delete your files!

---

## 🎯 Best Practice Workflow (Moving Forward)

### For Team Members:
1. **Pull main before starting work:**
   ```bash
   git pull origin main
   ```

2. **Create a feature branch:**
   ```bash
   git checkout -b feature/player-movement
   ```

3. **Work ONLY in your assigned scene:**
   - Kimhour: `Scene_Network_Core.unity`
   - Pranha: `Scene_Combat_Test.unity`
   - An: `Scene_AI_Test.unity`
   - Boren: `Scene_Level_Design.unity`

4. **When done, push your branch:**
   ```bash
   git add .
   git commit -m "ADD: Player movement"
   git push origin feature/player-movement
   ```

5. **Leader (You) reviews and merges selectively**

### For You (Leader):
1. **Pull their branch**
2. **Review with `git diff`**
3. **Selectively checkout files**
4. **Test in Unity before pushing to main**

---

## 🆘 Emergency: "I Merged and Everything Broke!"

```bash
# Option 1: Undo the merge (if you haven't pushed yet)
git reset --hard HEAD~1

# Option 2: Restore from backup
git reset --hard backup-before-merge

# Option 3: If you already pushed, revert the merge
git revert -m 1 HEAD
git push origin main
```

---

## ✅ Checklist Before Merging

- [ ] Created backup branch
- [ ] Reviewed `git diff --name-status`
- [ ] Identified NEW files (A) vs MODIFIED files (M)
- [ ] Tested in Unity Editor after merge
- [ ] No console errors in Unity
- [ ] Scenes still open without errors
- [ ] Committed with clear message

---

## 💡 Pro Tips

1. **Don't merge ProjectSettings/** unless necessary - it can break Unity config
2. **Scene conflicts are the worst** - avoid by sticking to separate scenes
3. **Prefabs are safer** - Unity handles prefab merging better than scenes
4. **Test immediately** after merge - don't merge Friday night before deadline!
5. **Communicate** - Tell team before big merges: "I'm merging everyone's work now"

---

## Quick Reference Commands

```bash
# See remote branches
git branch -r

# See what they changed
git diff --name-status main..origin/branch-name

# Merge without auto-commit
git merge origin/branch-name --no-commit --no-ff

# Undo staged file (keep your version)
git reset HEAD file.unity
git checkout -- file.unity

# Grab only one file from their branch
git checkout origin/branch-name -- path/to/file

# Abort merge if things go wrong
git merge --abort

# Create backup
git branch backup-now
```

---

Good luck! Take it slow, one branch at a time. 🎮
