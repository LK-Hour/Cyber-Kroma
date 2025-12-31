# Restore Environment Assets Guide

## Missing Asset Pack Identified

Your "Game Scene.unity" uses a **Low Poly City/Environment Pack** with:
- 🏢 Buildings (residential, commercial, skyscrapers)
- 🚗 Vehicles (cars, trucks, buses, taxis, police, ambulance)
- 🛣️ Roads (lanes, intersections, corners, concrete tiles)
- 🌳 Nature (trees, bushes, grass, rocks)
- 🚦 Props (street lights, traffic signals, benches, billboards, hydrants)

## Where to Get It

### Option 1: Unity Asset Store (Recommended)

**Likely Asset Pack:** "Low Poly City Pack" or "Simple City Pack"

**Popular matches:**
1. **"Simple City - Cartoon Assets"** by CityGen Studio
2. **"Low Poly Street Pack"** by Broken Vector
3. **"Cartoon City Builder"** by Quaternius
4. **"City Package"** by Vertex Studio

**Steps:**
1. Open Unity Hub
2. Window → Asset Store
3. Search: `low poly city pack`
4. Look for packs with Buildings, Vehicles, Roads
5. Download FREE or purchase
6. Import to project

### Option 2: Check Your Downloads

If you previously downloaded this pack:

```bash
# Unity Asset Store cache location
~/.local/share/unity3d/Asset Store-5.x/

# Search for the package
find ~/.local/share/unity3d/Asset\ Store-5.x/ -name "*City*" -o -name "*Building*" -o -name "*Vehicle*"
```

### Option 3: From Backup

If you have a Git backup from before deletion:

```bash
cd "/home/hour/Documents/CADT/Game Developement/Final Project/Cyber Kroma"

# Check git history for deleted files
git log --all --full-history --diff-filter=D -- "Assets/**/*Building*.prefab"

# Find when environment assets were deleted
git log --all --oneline --graph -- Assets/
```

## Manual Import Steps

1. **Download the asset pack** (Unity Asset Store or file)

2. **In Unity Editor:**
   - Assets → Import Package → Custom Package
   - Select downloaded `.unitypackage` file
   - Check: ✓ Buildings, ✓ Vehicles, ✓ Roads, ✓ Nature, ✓ Props
   - Click "Import"

3. **Verify Import:**
   ```
   Assets/
     └── [PackName]/
         ├── Prefabs/
         │   ├── Buildings/
         │   ├── Vehicles/
         │   ├── Roads/
         │   └── Props/
         └── Materials/
   ```

4. **Open Game Scene:**
   - Assets/Scenes/Game Scene.unity
   - Missing prefabs should auto-reconnect if GUIDs match

## Missing Prefab GUIDs Reference

**Most Critical (160 instances):**
- `16da29ce0a2fad04f835dcfdf665d2bc` - **Virus** (Your enemy prefab!)

**Environment (Top 10):**
- `b8ee657c6800ab849bb30b4375d02323` - Props_Street Light (127 instances)
- `2509cc5763ca17f4598bf61fd585a93e` - Natures_Fir Tree (53 instances)
- `4f32dfddb033d3e40b5841bc53cc1689` - Road Lane_01 (55 instances)
- `b82e76f4092b7d24aaeecddf36fe8d3b` - Natures_Big Tree (49 instances)
- `d4cfcf659200d7548b90a5580c3ab18f` - Props_Traffic Signal_big (44 instances)
- `31bbda51eeab02f44a8e92a7da2f5ff8` - Natures_Bush_01 (44 instances)
- `0ee6d569ec3227e40888e8151b331e0f` - Natures_Pot Bush_big (32 instances)
- `800befcc62026a34693972077c91698d` - Natures_Pot Bush_small (32 instances)
- `2d5fb229ae2945742b900a405a5e7a0b` - Natures_House Floor (30 instances)
- `dac1a631b172e66469fb09e9987bcf57` - Natures_Rock_Big (28 instances)

## Alternative: Use Scene_Level_Design

If you can't restore the assets, **use Scene_Level_Design.unity instead**:

**Advantages:**
- ✅ All prefabs working
- ✅ Configured WaveManager
- ✅ Enemy AI setup
- ✅ Shop, DataCore, spawning system

**Switch to it:**
1. File → Open Scene → Scene_Level_Design.unity
2. Build Settings → Remove "Game Scene.unity"
3. Continue development there

## Quick Search Commands

**Find asset name from GUID:**
```bash
# In Unity project
grep -r "16da29ce0a2fad04f835dcfdf665d2bc" Assets/**/*.meta
```

**Check if assets exist in project:**
```bash
find Assets/ -name "*.prefab" | grep -i building
find Assets/ -name "*.prefab" | grep -i vehicle
find Assets/ -name "*.prefab" | grep -i road
```

---

**Next Steps:**
1. Search Unity Asset Store for "Low Poly City Pack"
2. Download FREE version to test
3. Import and verify GUIDs match
4. If not, manually rebuild scene in Scene_Level_Design.unity
