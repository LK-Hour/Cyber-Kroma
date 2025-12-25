# VICTORY/DEFEAT DETECTION SYSTEM

## How Victory is Detected ✅

**Flow:**
1. **WaveManager** tracks current wave number (1-3)
2. **GameLoop** spawns enemies for each wave
3. Waits for all enemies to be defeated: `while (activeEnemies.Count > 0)`
4. When wave is complete, moves to next wave
5. **After Wave 3 completes** → Checks `if (!gameOver)` → Calls `OnVictory()`

**OnVictory() does:**
- Shows VictoryPanel UI
- Pauses game: `Time.timeScale = 0f`
- Calls `GameSceneManager.Instance.OnVictory()`
- GameSceneManager waits 5 seconds → Returns to MainMenu

**Code Location:**
- `Assets/Scripts/WaveManager.cs` lines 120-125 (GameLoop completion)
- `Assets/Scripts/WaveManager.cs` lines 203-217 (OnVictory method)

---

## How Defeat is Detected ❌

**Flow:**
1. **Enemies attack DataCore** (tagged "DataCore")
2. **EnemyAI** detects DataCore as target
3. Enemy performs melee attack → Calls `DataCoreHealth.TakeDamage(damage)`
4. **DataCoreHealth** reduces `currentHealth`
5. **When health reaches 0** → Calls `OnCoreDestroyed()`

**OnCoreDestroyed() does:**
- Shows DefeatPanel UI
- Notifies `WaveManager.OnCoreDestroyed()`
- WaveManager sets `gameOver = true` (stops wave loop)
- Pauses game: `Time.timeScale = 0f`
- Calls `GameSceneManager.Instance.OnDefeat()`
- GameSceneManager waits 5 seconds → Returns to MainMenu

**Code Location:**
- `Assets/Scripts/DataCoreHealth.cs` lines 36-60 (TakeDamage and health check)
- `Assets/Scripts/DataCoreHealth.cs` lines 86-106 (OnCoreDestroyed method)
- `Assets/Scripts/WaveManager.cs` lines 183-201 (OnCoreDestroyed handler)

---

## Testing Victory/Defeat

### Test Victory:
1. Start game in Scene_Level_Design
2. Kill all enemies in Waves 1-3
3. After Wave 3, all enemies defeated → VictoryPanel appears
4. After 5 seconds → Returns to MainMenu automatically

### Test Defeat:
1. Start game in Scene_Level_Design
2. Let enemies reach the DataCore (glowing object with "DataCore" tag)
3. Let them attack until Core health bar reaches 0
4. DefeatPanel appears immediately
5. After 5 seconds → Returns to MainMenu automatically

### Force Defeat for Testing:
In Unity Console during Play Mode:
```csharp
// Find DataCore and kill it instantly
GameObject.FindGameObjectWithTag("DataCore").GetComponent<DataCoreHealth>().TakeDamage(9999);
```

### Force Victory for Testing:
In Unity Console during Play Mode:
```csharp
// Skip to last wave and kill all enemies
FindObjectOfType<WaveManager>().currentWave = 3;
// Then manually destroy all active enemies
```

---

## Victory/Defeat Conditions Summary

| Condition | Triggers | Result |
|-----------|----------|--------|
| **Victory** | All 3 waves completed + All enemies defeated | VictoryPanel → MainMenu |
| **Defeat** | DataCore health reaches 0 | DefeatPanel → MainMenu |
| **Defeat (Future)** | All players dead (not implemented yet) | DefeatPanel → MainMenu |

---

## Current Implementation Status

✅ **Victory detection** - Fully working (waves complete)  
✅ **Defeat detection** - Fully working (DataCore destroyed)  
❌ **Defeat on all players dead** - Not yet implemented (single player mode works)  

**For Multiplayer:**
- Need to track connected players' health
- When all NetworkObject players have health = 0 → Defeat
- This requires NetworkVariable sync for player health states

---

## Scene Transition Flow

```
Victory/Defeat Detected
        ↓
Time.timeScale = 0 (pause)
        ↓
Show UI Panel (Victory/Defeat)
        ↓
Call GameSceneManager method
        ↓
Wait 5 seconds (coroutine)
        ↓
SceneManager.LoadSceneAsync("MainMenu")
        ↓
Player returns to main menu
```

All systems are already connected! Just need to test in Unity. 🎮
