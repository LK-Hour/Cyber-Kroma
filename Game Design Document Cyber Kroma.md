# GAME DESIGN DOCUMENT: CYBER KROMA

**Game Name:** Cyber Kroma: The Scam Hunter  
**Team Name:** KromaCode  
**Date:** November 24, 2025  
**Engine:** Unity 2022.3.47f1 (LTS)  
**Platform:** Android (Mobile)

---

## 1. Game Overview

- **Genre:** 3D Multiplayer First-Person Shooter (Co-op Survivor)
- **Target Audience:** Cambodian teenagers and students (Mobile users)
- **Game Goal:** Cooperate with up to 4 players to survive 5 waves of malware attacks, protect the "Data Core," and rescue the "New User" NPC
- **Core Concept:** A digital defense game set in a neon "Cyber Phnom Penh" where players use "Digital Literacy" skills (Fact-Checking, Firewalls) to defeat enemies representing real-world scams (Phishing, DeepFakes)

---

## 2. Story & Setting

- **Setting:** The **"Khmer Digital Network"** — a futuristic, neon-lit virtual city
- **Visual Style:** Cyberpunk Phnom Penh. Iconic landmarks like the **Independence Monument** and **Wat Phnom** are constructed from glowing blue circuits and floating binary code
- **Story:** In 2025, a massive wave of "Dark Data" attacks the Cambodian internet. Players are elite members of **Cyber Kroma**, a digital defense squad. Their mission is to purge the infection and teach the "New User" (a confused civilian NPC) how to stay safe online

---

## 3. Core Gameplay

### Player Mechanics (The Agents)

**Mobile Controls:**
- **Left Virtual Joystick:** Movement (WASD equivalent)
- **Touch Screen Swipe:** Aiming (Camera Look)
- **Shoot Button:** Shoot "Report Blaster"
- **Skill Button:** Skill (e.g. 2FA Shield)
- **Additional Button:** Interact (Revive/Shop)

**Customization:**
- Players can select their **Headgear** (Kroma Scarf, Apsara Visor) in the lobby
- *Network Sync:* Visuals are synchronized so teammates see unique avatars

### Interactions & NPC (AI Feature)

**"Lok Ta Digital" (AI Shopkeeper):**
- Between waves, a holographic Khmer Elder appears
- **Logic:** Uses game data to recommend items (e.g., *If player HP is low -> "Chau, buy an Antivirus Potion!"*)
- **AI Asset:** Voice lines generated via AI Text-to-Speech; Dialogue variations generated via LLM

### Enemies (Educational Metaphors)

1. **The Phisher (Ranged):** Shoots "Fake Link" projectiles. **Effect:** If hit, the mobile screen is covered in a "Pop-up Ad" UI overlay. Player must tap "X" to close it

2. **The Ghost Account (Stealth):** Invisible enemies that ignore players and attack the Data Core. Visible only via the "Scanner" class

3. **The DeepFake (Boss):** A powerful AI that copies the mesh and name of a teammate. Players must communicate to identify the impostor

### Win/Lose Conditions

- **Win:** Defeat the Wave 5 Boss and keep Data Core health > 0%
- **Lose:** Data Core reaches 0% or all players are "Disconnected" (HP 0)

---

## 4. Gameplay Flow

1. **Main Menu:** Start Game, Customization Lobby, Settings (Language: Khmer/English)
2. **Lobby:** Host/Join via LAN or IP. Select Class (Firewall, Debugger, Scanner)
3. **Gameplay Loop:**
   - **Combat Phase (2 mins):** Defend Core from Waves
   - **Shop Phase (30s):** Interact with NPC "Lok Ta" to upgrade stats
   - **Educational Popup:** A "Scam Alert" card explains the enemy just fought (e.g., *Telegram Phishing Tactics*)
4. **End Screen:** Scoreboard + Digital Safety Rating

---

## 5. Art Direction & UI

- **Style:** Low-Poly Synthwave
- **Why:** Best for mobile performance. Uses simple shapes with high-contrast colors (Cyan, Magenta, Gold)

**UI Design:**
- **Language:** Supports Khmer Unicode (using TextMeshPro) for all menus and tips
- **Mobile Layout:** Large, thumb-friendly buttons. Semi-transparent HUD to maximize visibility
- **Glitch Effects:** UI flickers when the player takes damage

---

## 6. Audio

- **Music:** Upbeat Synthwave mixed with traditional Khmer instruments (Tro/Roneat)
- **Voice Over:** AI-Generated Khmer voice lines for the NPC and system alerts (e.g., *"Som Proyat!"*)
- **SFX:** Digital "Zaps," "Modem Dial-up" noises for enemy spawns

---

## 7. Technical Requirements & Graphics Optimization

### Core Technologies
- **Engine:** Unity 2022.3.47f1 (LTS) with Android Build Support
- **Pipeline:** Universal Render Pipeline (URP) - Required for performant mobile glow effects
- **Networking:** Netcode for GameObjects (NGO)
- **Target Platform:** Android (Minimum API Level 24 / Android 7.0 Nougat)
- **Input Handling:** Unity New Input System Package (handling Multi-touch)

### On-Screen Controls
- **Left Stick:** Virtual Joystick for Player Movement (Vector2)
- **Right Region:** Touch & Drag for Camera Rotation (Delta)
- **Action Buttons:** UI Buttons mapped to functions (Shoot, Jump, Skill)

### Graphics Optimization
*To ensure the game runs on Android while looking good:*

| Optimization | Setting | Reason |
|--------------|---------|--------|
| **Mesh Detail** | Low-Poly (< 5000 tris per character) | Reduces draw calls |
| **Textures** | Max 1024x1024, compressed (ASTC) | Saves VRAM |
| **Lighting** | Baked lightmaps + 1 real-time light | Mobile GPUs struggle with real-time shadows |
| **Post-Processing** | Bloom only (no motion blur) | Bloom is cheap; blur tanks FPS |
| **Particles** | Max 100 particles on-screen | Overdraw kills mobile performance |
| **Target FPS** | 30 FPS (stable) or 60 FPS (high-end) | Battery life vs smoothness trade-off |

---

## 8. Scope & Team Roles

**Timeline:** 5 Weeks (Submission Dec 23, 2025)

| Role | Member | Responsibilities |
|------|--------|------------------|
| **Leader/Network Engineer** | Kimhour | Netcode setup, build management, Git coordination |
| **Combat Engineer** | Pranha | Player controller, shooting mechanics, mobile input |
| **AI/Logic Master** | An | Enemy AI, wave spawner, NPC shop logic |
| **Level Designer/Artist** | Longboren | Environment design, UI/UX, Khmer localization |

---

## 9. Educational Integration

### Scam Types Covered
1. **Phishing** - Email/message scams
2. **Ghost Accounts** - Fake social media profiles
3. **DeepFakes** - AI-generated fake content

### Learning Outcomes
- Players learn to identify common digital scams
- Understand basic cybersecurity concepts (2FA, verification)
- Practice safe online communication

---

## 10. Success Metrics

- **Engagement:** Average session length > 15 minutes
- **Education:** 80% of players can identify scam types after playing
- **Performance:** Maintains 30+ FPS on mid-range Android devices
- **Localization:** UI fully functional in Khmer and English

---

## 11. Future Expansion (Post-Launch)

- Additional enemy types (Ransomware, Bot Networks)
- New maps (Cyber Angkor Wat, Digital Mekong)
- Seasonal events with real-world scam awareness campaigns
- Integration with Cambodian digital literacy programs

---

**Document Version:** 1.0  
**Last Updated:** December 23, 2025  
**Status:** Final Submission
