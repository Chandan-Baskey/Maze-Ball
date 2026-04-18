# 🎮 MAZE BALL

> A 3D isometric maze puzzle game built with Unity — guide the ball through increasingly complex mazes before time runs out!

![Unity](https://img.shields.io/badge/Unity-2022.x-black?logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-blue?logo=csharp)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20PC-green)
![Levels](https://img.shields.io/badge/Levels-15+-orange)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

---

## 📱 Game Screenshots

| Main Menu | Level Select | Gameplay | Win Screen | Game Over |
|-----------|-------------|----------|------------|-----------|
| START · LEVELS · QUIT | 15 Levels Grid | 3D Isometric Maze | WIN GAME popup | GAME OVER popup |

---

## 📖 Table of Contents

- [About the Game](#-about-the-game)
- [Features](#-features)
- [Gameplay](#-gameplay)
- [Project Structure](#-project-structure)
- [Scene Overview](#-scene-overview)
- [Scripts Reference](#-scripts-reference)
- [Level Design](#-level-design)
- [UI System](#-ui-system)
- [How to Play](#-how-to-play)
- [Installation & Setup](#-installation--setup)
- [Build Instructions](#-build-instructions)
- [Dependencies](#-dependencies)
- [Contributing](#-contributing)

---

## 🕹 About the Game

**Maze Ball** is a 3D isometric maze puzzle game for mobile (Android/iOS) and PC. Players tilt or swipe to roll a ball through a maze, reaching the exit zone before the countdown timer hits zero. The game features 15 hand-crafted levels with progressively tighter time limits and more complex maze layouts.

The game uses Unity's **Universal Render Pipeline (URP)** for polished visuals with post-processing effects, dynamic lighting, and smooth physics.

---

## Game Main Menu

![image alt](https://github.com/Chandan-Baskey/Maze-Ball/blob/9e6c870df03ecf8766b614f6a0dc4937da6b6b64/main-menu.jpg)


## ✨ Features

- 🌀 **15 unique maze levels** with increasing difficulty
- ⏱ **Countdown timer** — race against the clock each level
- 🏆 **Win / Game Over** screens with Home, Retry, and Next Level buttons
- 🗺 **Level Select screen** — jump to any unlocked level
- 🎨 **3D isometric art style** with URP post-processing
- 📱 **Mobile-first UI** — designed for portrait orientation
- 🔊 Audio Listener on Main Camera for future sound integration
- 🔄 **Scene-based architecture** — each level is its own Unity scene
- 🧱 Prefab-driven design for maze walls, ground, player, and UI
- 🌍 NavMesh-ready configuration (for future AI/enemy expansions)

---

## 🎮 Gameplay

### Objective
Roll the ball from its **starting position** to the **Win Ground** zone at the end of the maze — before the timer reaches zero.

### Controls
| Platform | Control |
|----------|---------|
| Mobile | Tilt device / Swipe |
| PC | Arrow Keys / WASD |

### Timer
Each level starts with a set countdown (e.g., **25 seconds** on Level 1). If time runs out before reaching the exit, it's **Game Over**.

### Win Condition
The ball must reach the **Win Ground** trigger zone. On contact, `GameManager.WinGame()` is called, the player stops, the ball disappears, and the **WIN GAME** popup appears with three options:
- 🏠 **Home** → Main Menu  
- 🔄 **Restart** → Replay current level  
- ▶️ **Next** → Load next level  

### Lose Condition
When `timeLeft <= 0` and the win condition hasn't been met, `GameManager.GameOver()` fires:
- **GAME OVER** popup appears  
- Player controller is disabled  
- Ball is hidden  
- Two options: 🏠 Home or 🔄 Restart

---

## 📁 Project Structure

```
MazeBall/
├── Assets/
│   ├── Scenes/
│   │   ├── 0-MainManu.unity          # Scene index 0 — Main Menu
│   │   ├── 01-LevelS.unity           # Scene index 1 — Level Select
│   │   ├── LvL-1.unity               # Scene index 2 — Level 1
│   │   └── ... (LvL-2 through LvL-15)
│   │
│   ├── Scripts/
│   │   ├── GameManager.cs            # Core game logic (timer, win/lose)
│   │   ├── LvLMenu.cs                # Level select navigation
│   │   ├── MainMenu.cs               # Main menu navigation
│   │   └── PlayerController.cs       # Ball/player movement (referenced)
│   │
│   ├── Prefabs/
│   │   ├── GameManager prefab        # guid: f57e7b9b5180a2e4ba34ede149e73000
│   │   ├── Ground / Ground 2 prefab  # guid: 869390d7521a44a449f4d4e14aab3117
│   │   ├── Win Ground prefab         # guid: 7e68d24e865857346bf60b83f107566b
│   │   ├── Canvas (UI) prefab        # guid: 1b2e3147e99478a48baa33f36e4d2ac1
│   │   └── GlobalVolume prefab       # guid: 6f1062a3e60bfa842b9885dea92200d3
│   │
│   ├── Materials/
│   │   ├── Ball material             # guid: e0f22696a9fbc9b41a279dc4cd61fb4b
│   │   ├── Wall material             # guid: f51a0b95663958b4287c26346de2eb4c
│   │   └── Ground material           # guid: 70b5af82b1e64c145be36244e6cc2b63
│   │
│   └── Fonts/
│       ├── Custom bold font          # guid: cbb8c5cd2b31d2b44ad3b29cd20b2a0a
│       └── Title font                # guid: a577791e9f3863d4e9506d710d62cff3
│
└── README.md
```

---

## 🎬 Scene Overview

### Scene 0 — `0-MainManu.unity` (Main Menu)

The entry point of the game. Contains a Canvas with:
- **MAZE BALL** logo / title text
- **START** button → loads Scene 2 (Level 1)
- **LEVELS** button → loads Scene 1 (Level Select)
- **QUIT** button → calls `Application.Quit()`

Attached script: `MainMenu.cs` on the Canvas GameObject.

### Scene 1 — `01-LevelS.unity` (Level Select)

A scrollable UI with 15 level buttons (LvL1 through LvL15), each loading its respective scene index (2–16). Also includes:
- **PlayB** → Start from Level 1 (`StartGame()`)
- **HomeB** → Return to Main Menu (`BackMainMenu()`)

Attached script: `LvLMenu.cs` on the Canvas GameObject.

### Scenes 2–16 — Level Scenes (`LvL-1.unity` through `LvL-15.unity`)

Each level scene contains:

| Object | Description |
|--------|-------------|
| `Main Camera` | Positioned at `(0, 15.57, -10.7)` with a 50.158° tilt for isometric view; URP camera with post-processing |
| `Directional Light` | Intensity 2, color temperature 5000K, angled at (50°, -30°, 0°) |
| `Ball` | Sphere with Rigidbody, SphereCollider (radius 0.5), custom material, and `BallController` script |
| `Ground 2` | Maze floor prefab instance, tagged `"Ground"`, with custom material |
| `Win Ground` | Trigger zone prefab that fires the win condition |
| `GameManager` | Invisible manager object running `GameManager.cs` |
| `Canvas` | In-game HUD prefab (timer display, win/lose overlays) |
| `Global Volume` | URP post-processing volume for bloom, color grading, etc. |
| `EventSystem` | Unity input event system |
| Maze Walls (`Cube1`, `Cube1 (2)`, etc.) | Scaled cube GameObjects forming the maze layout, parented to Ground 2 |

---

## 📜 Scripts Reference

### `GameManager.cs`

The central controller for each level scene.

```csharp
public class GameManager : MonoBehaviour
```

**Public Fields:**

| Field | Type | Description |
|-------|------|-------------|
| `timeLeft` | `float` | Countdown time in seconds (default: 25) |
| `winText` | `GameObject` | The WIN GAME UI panel |
| `gameOverText` | `GameObject` | The GAME OVER UI panel |
| `ball` | `GameObject` | The ball object (hidden on end) |
| `player` | `PlayerController` | Player script (disabled on end) |
| `timerText` | `TextMeshProUGUI` | TMP timer display |
| `timeText` | `GameObject` | Timer display container |
| `currentScene` | `int` | Index of this scene (for restart) |
| `nextScene` | `int` | Index of the next scene |

**Key Methods:**

```csharp
void Update()
// Decrements timer each frame; triggers GameOver when time hits 0

public void WinGame()
// Activates winText, disables player, hides ball

public void GameOver()
// Activates gameOverText, disables player, hides ball

public void RestartGame()   // SceneManager.LoadScene(currentScene)
public void NextLevel()     // SceneManager.LoadScene(nextScene)
public void Back()          // SceneManager.LoadScene(0) — Main Menu
public void LevelMenu()     // SceneManager.LoadScene(1) — Level Select
```

---

### `LvLMenu.cs`

Handles all navigation from the Level Select screen.

```csharp
public class LvLMenu : MonoBehaviour
```

**Methods:**

| Method | Loads Scene |
|--------|-------------|
| `StartGame()` | 2 (Level 1) |
| `BackMainMenu()` | 0 (Main Menu) |
| `Level1()` – `Level15()` | 2 – 16 |
| `Level16()` – `Level20()` | 17 – 21 (reserved for future levels) |

---

### `MainMenu.cs`

Handles navigation from the Main Menu screen.

```csharp
public class MainMenu : MonoBehaviour
```

**Methods:**

| Method | Action |
|--------|--------|
| `StartGame()` | Loads Scene 2 (Level 1) |
| `LevelMenu()` | Loads Scene 1 (Level Select) |
| `QuitGame()` | `Application.Quit()` |

---

### `PlayerController.cs` *(referenced)*

Controls ball movement. Referenced by `GameManager` as `public PlayerController player`. Disabled when the game ends to freeze ball input.

---

## 🗺 Level Design

Maze walls are Unity default **Cube** primitives, scaled and rotated to form corridors. Each wall object has:
- `MeshFilter` (Cube mesh — fileID `10202`)
- `MeshRenderer` with a shared wall material
- `BoxCollider` for physics

The maze geometry is **parented to the Ground 2 prefab root** so the entire maze moves as one unit.

**Camera setup** for isometric 3D view:
```
Position:  (0, 15.57, -10.7)
Rotation:  X = 50.158°
FOV:       60°
Near clip: 0.3 | Far clip: 1000
```

**Ball physics settings:**
```
Mass: 1
Drag: 0 (air resistance)
Angular Drag: 0.05
Collision Detection: Discrete
Uses Gravity: true
```

**BallController bounciness settings** (via custom MonoBehaviour):
```
materialBounciness: 1
bounceCombine: 2 (Average)
bounceDamping: 1
maxBounceSpeed: 1
groundTag: "Ground"
minLinearDrag: 0.2
minAngularDrag: 0.05
```

---

## 🖼 UI System

The game uses **Unity UI (uGUI)** with a Canvas set to **Screen Space - Overlay** mode and scaled at `1920×1080` reference resolution.

### In-Game HUD (Canvas Prefab)
- **Timer Text** — `TextMeshProUGUI` component updated every frame
- **Win Panel** — hidden by default; shown on `WinGame()`; contains Home, Restart, Next buttons
- **Game Over Panel** — hidden by default; shown on `GameOver()`; contains Home, Restart buttons

### Level Select Grid Layout
15 level buttons arranged in a 3-column grid:

```
Row 1:  [ 1 ] [ 2 ] [ 3 ]
Row 2:  [ 4 ] [ 5 ] [ 6 ]
Row 3:  [ 7 ] [ 8 ] [ 9 ]
Row 4:  [10 ] [11 ] [12 ]
Row 5:  [13 ] [14 ] [15 ]
```

Each button uses a gold coin sprite with a bold numeric label and calls `LvLMenu.Level{N}()` on click.

---

## 🕹 How to Play

1. Launch the game — you'll see the **Main Menu**
2. Tap **START** to play from Level 1, or **LEVELS** to pick a specific level
3. In the level, **tilt your device** (mobile) or use **Arrow Keys / WASD** (PC) to roll the ball
4. Navigate through the maze and reach the **exit zone** before the timer expires
5. On winning: choose **Next** to advance, **Restart** to replay, or **Home** to return
6. On game over: choose **Restart** to try again or **Home** to go back to the menu

---

## 🛠 Installation & Setup

### Requirements

| Tool | Version |
|------|---------|
| Unity Editor | 2022.x LTS or later |
| Universal Render Pipeline (URP) | Included via Package Manager |
| TextMeshPro | Included via Package Manager |
| .NET | Standard 2.1 |
| Platform Modules | Android / iOS / Standalone |

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/yourusername/maze-ball.git

# 2. Open Unity Hub and click "Add project from disk"
#    Navigate to the cloned folder and open it

# 3. Unity will import all assets automatically (may take a few minutes)

# 4. Open File > Build Settings and verify scene order:
#    Index 0  → 0-MainManu
#    Index 1  → 01-LevelS
#    Index 2  → LvL-1
#    ...
#    Index 16 → LvL-15
```

> ⚠️ **Important:** Scene build indices must match exactly. `LvLMenu.cs` and `GameManager.cs` use hardcoded scene indices.

---

## 📦 Build Instructions

### Android

1. Go to `File > Build Settings`
2. Select **Android** and click **Switch Platform**
3. Under `Player Settings`:
   - Set **Company Name** and **Product Name**
   - Set **Minimum API Level** to Android 6.0 (API 23) or higher
   - Enable **Auto Graphics API** or set to **OpenGLES3**
4. Click **Build** and choose an output folder

### iOS

1. Switch platform to **iOS** in Build Settings
2. Set your **Bundle Identifier** in Player Settings
3. Click **Build** to generate an Xcode project
4. Open the `.xcodeproj` in Xcode and deploy to device

### PC (Windows / Mac)

1. Switch platform to **Standalone**
2. Select your target OS
3. Click **Build and Run**

---

## 📋 Scene Build Index Reference

```
Build Index | Scene File          | Description
------------|---------------------|--------------------
     0      | 0-MainManu.unity    | Main Menu
     1      | 01-LevelS.unity     | Level Select
     2      | LvL-1.unity         | Level 1  (25s timer, nextScene=3)
     3      | LvL-2.unity         | Level 2
     4      | LvL-3.unity         | Level 3
     5      | LvL-4.unity         | Level 4
     6      | LvL-5.unity         | Level 5
     7      | LvL-6.unity         | Level 6
     8      | LvL-7.unity         | Level 7
     9      | LvL-8.unity         | Level 8
    10      | LvL-9.unity         | Level 9
    11      | LvL-10.unity        | Level 10
    12      | LvL-11.unity        | Level 11
    13      | LvL-12.unity        | Level 12
    14      | LvL-13.unity        | Level 13
    15      | LvL-14.unity        | Level 14
    16      | LvL-15.unity        | Level 15
```

---

## 📦 Dependencies

| Package | Purpose |
|---------|---------|
| Universal Render Pipeline (URP) | Rendering, post-processing, lighting |
| TextMeshPro | High-quality timer and UI text |
| Unity UI (uGUI) | Buttons, canvas, panels |
| Unity Physics | Ball Rigidbody, collider, bounce |

All packages are managed via the **Unity Package Manager** and are included in the project manifest.

---

## 🗂 Key Prefab GUIDs (for reference)

| Prefab | GUID |
|--------|------|
| GameManager | `f57e7b9b5180a2e4ba34ede149e73000` |
| Ground / Maze | `869390d7521a44a449f4d4e14aab3117` |
| Win Ground trigger | `7e68d24e865857346bf60b83f107566b` |
| UI Canvas | `1b2e3147e99478a48baa33f36e4d2ac1` |
| Global Volume (URP) | `6f1062a3e60bfa842b9885dea92200d3` |

---

## 🤝 Contributing

1. Fork this repository
2. Create your feature branch: `git checkout -b feature/new-level`
3. Add your level scene and register it in Build Settings
4. Update `LvLMenu.cs` to include the new `Level{N}()` method
5. Commit your changes: `git commit -m "Add Level 16"`
6. Push to the branch: `git push origin feature/new-level`
7. Open a Pull Request

### Adding a New Level — Checklist

- [ ] Duplicate an existing level scene
- [ ] Design the new maze using Cube primitives parented to Ground prefab
- [ ] Set `currentScene` and `nextScene` in the GameManager prefab instance
- [ ] Add the scene to **Build Settings** at the correct index
- [ ] Add a `Level{N}()` method in `LvLMenu.cs`
- [ ] Add a button in `01-LevelS.unity` wired to the new method

---

## 👤 Author

**Your Name**  
📧 im.chandanbaskey@email.com  
🌐 [https://github.com/chandan-Baskey](https://github.com/Chandan-Baskey)

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<div align="center">
  <strong>Made with ❤️ in Unity</strong><br/>
  <em>Roll the ball. Beat the maze. Beat the clock.</em>
</div>
