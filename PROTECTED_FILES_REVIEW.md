# 🔍 Protected Files Review - NOT Yet Merged

**Date:** December 23, 2025  
**Status:** Awaiting your decision to merge or skip

---

## 📊 Summary

| Category | Boren's Branch | Pranha's Branch |
|----------|---------------|-----------------|
| **Modified Scenes** | 6 scenes | 1 scene |
| **Modified ProjectSettings** | 20 files | 0 files |
| **Deleted Files** | 0 | Multiple (dangerous!) |

---

## 🎬 1. MODIFIED UNITY SCENES

### From Boren's Branch:

#### ✅ His Own Scene (SAFE to merge):
- `Assets/Scenes/Scene_Level_Design.unity` ← **Boren's workspace**

#### ⚠️ Other Team Scenes (RISKY - needs review):
- `Assets/Scenes/Scene_Combat_Test.unity` ← **Pranha's workspace** (Why did Boren modify this?)
- `Assets/Scenes/Scene_AI_Test.unity` ← **An's workspace** (Why did Boren modify this?)
- `Assets/Scenes/Scene_Network_Core.unity` ← **Your workspace** (Duplicate path, might be his copy)
- `Assets/Scene_Network_Core.unity` ← **Your workspace** (Different path)
- `Assets/Scenes/test.unity` ← Test scene

### From Pranha's Branch:

#### ✅ His Own Scene (SAFE to merge):
- `Assets/Scenes/Scene_Combat_Test.unity` ← **Pranha's workspace**

#### ℹ️ New Scene (SAFE - already merged):
- `Assets/SciFi_Space_Soldier/Scenes/DemoScene.unity` ← Demo for his assets

---

## ⚙️ 2. MODIFIED PROJECT SETTINGS (Boren Only)

Boren modified **ALL** ProjectSettings files. This is unusual and risky.

### Critical Files Changed:
```
ProjectSettings/
├── EditorBuildSettings.asset    ← Build configuration (scenes in build)
├── TagManager.asset              ← Tags and Layers
├── InputManager.asset            ← Input configuration
├── ProjectSettings.asset         ← Main project config (platform, quality)
├── GraphicsSettings.asset        ← Graphics pipeline settings
├── QualitySettings.asset         ← Quality presets
└── ... 14 more files
```

**Why This Matters:**
- **EditorBuildSettings** = Which scenes are included in the build
- **TagManager** = If Boren added new Tags/Layers for his work
- **ProjectSettings** = Could change platform target, package name, etc.

**Risk:** If you merge these, it might overwrite your Unity configuration.

---

## 🗑️ 3. DELETED FILES (Pranha's Branch - DANGEROUS!)

### Important Files Pranha Deleted:

```
❌ .gitattributes                                    ← Git LFS configuration!
❌ Assets/Independence_Monument_1214063220_texture.glb ← Boren's model
❌ Assets/Editor/Khmer/CreateKhmerExample.cs         ← Boren's scripts
❌ Assets/Editor/Khmer/KhmerTools.cs                 ← Boren's scripts
❌ Assets/Scripts/Khmer/KhmerTextMeshPro.cs          ← Boren's scripts
❌ Assets/Scenes/test.unity                          ← Test scene
```

**What Happened:**
- Pranha's branch is **OLDER** than main
- He doesn't have Boren's work or your recent updates
- If you merge, it will **DELETE** files you just added!

---

## 🎯 RECOMMENDATIONS

### ✅ SAFE TO MERGE (Do This):

#### 1. Boren's Own Scene:
```bash
git checkout origin/boren -- Assets/Scenes/Scene_Level_Design.unity
git commit -m "MERGE: Boren's Level Design scene updates"
```

#### 2. Pranha's Own Scene:
```bash
git checkout origin/Pranha -- Assets/Scenes/Scene_Combat_Test.unity
git commit -m "MERGE: Pranha's Combat Test scene updates"
```

---

### ⚠️ REVIEW FIRST (Check Before Merging):

#### 1. Boren Modified Other Scenes - WHY?

**Option A:** Ask Boren in group chat:
> "Boren, I see you modified Scene_Combat_Test and Scene_AI_Test. What changes did you make? Should I merge them?"

**Option B:** View the differences yourself:
```bash
# See what Boren changed in Combat scene
git diff main..origin/boren -- Assets/Scenes/Scene_Combat_Test.unity
```

**Option C:** Open both versions in Unity:
```bash
# Checkout Boren's version to a temp file
git show origin/boren:Assets/Scenes/Scene_Combat_Test.unity > /tmp/boren_combat.unity
# Then compare in text editor or Unity YAML merge tool
```

---

### ❌ DO NOT MERGE (Skip These):

#### 1. Boren's ProjectSettings (Too Risky):
- **DON'T** merge unless you know exactly what changed
- These files control Unity's entire configuration
- If broken, it could crash your project

**Safe Alternative:**
- Ask Boren: "Did you add new Tags or Layers I need?"
- If yes, manually add them in Unity Editor (safer than git merge)

#### 2. Pranha's Deletions (Will Break Project):
- **NEVER** merge deleted files from an old branch
- Pranha's branch is outdated
- His deletions would remove Boren's recent work

---

## 📋 Decision Matrix

| File | Merge? | Reason |
|------|--------|--------|
| `Scene_Level_Design.unity` (Boren) | ✅ YES | His own scene |
| `Scene_Combat_Test.unity` (Pranha) | ✅ YES | His own scene |
| `Scene_Combat_Test.unity` (Boren) | ⚠️ ASK | Why did he touch Pranha's scene? |
| `Scene_AI_Test.unity` (Boren) | ⚠️ ASK | Why did he touch An's scene? |
| `Scene_Network_Core.unity` (Boren) | ⚠️ ASK | Check if this conflicts with your work |
| `ProjectSettings/*` (Boren) | ❌ NO | Too risky without review |
| Deleted files (Pranha) | ❌ NO | Branch is outdated |

---

## 🛠️ How to Safely Merge Scene Files

### Method 1: Cherry-Pick Individual Scenes (Recommended)

```bash
# Merge Boren's Level Design scene
git checkout origin/boren -- Assets/Scenes/Scene_Level_Design.unity
git add Assets/Scenes/Scene_Level_Design.unity
git commit -m "MERGE: Boren's Level Design scene"

# Merge Pranha's Combat scene  
git checkout origin/Pranha -- Assets/Scenes/Scene_Combat_Test.unity
git add Assets/Scenes/Scene_Combat_Test.unity
git commit -m "MERGE: Pranha's Combat scene"

# Push
git push origin main
```

### Method 2: Manual Review (For Conflicting Scenes)

```bash
# 1. Create a temporary branch to test
git checkout -b test-merge-scenes

# 2. Try merging one scene at a time
git checkout origin/boren -- Assets/Scenes/Scene_AI_Test.unity

# 3. Open Unity and test if it works
# 4. If good, commit. If broken, discard:
git checkout main -- Assets/Scenes/Scene_AI_Test.unity
```

---

## 🚨 Emergency: If You Accidentally Merge Everything

```bash
# Undo last merge (if not pushed yet)
git reset --hard HEAD~1

# Or restore from backup
git reset --hard backup-pre-merge-dec23

# Or revert specific file
git checkout backup-pre-merge-dec23 -- path/to/file
```

---

## 📞 Next Steps

1. **Review this document**
2. **Contact your team:**
   - Ask Boren: "Why did you modify Scene_Combat_Test and Scene_AI_Test?"
   - Ask Pranha: "Your branch is old. Can you pull main and re-push?"
3. **Make decision:**
   - Merge safe scenes (Level Design, Combat Test)
   - Skip risky ProjectSettings
   - Ignore deletions from old branch
4. **Test in Unity before pushing**

---

## 💡 Pro Tips

1. **Always communicate before merging** - Ask team why they changed files
2. **Test in Unity immediately** - Don't merge and disappear for a day
3. **One scene at a time** - Easier to debug if something breaks
4. **Keep backup branch** - You have `backup-pre-merge-dec23` just in case
5. **Push small commits** - Don't merge everything at once

---

**Status:** Waiting for your decision. What would you like to merge?
