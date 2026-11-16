# match_2_blast

A Unity 2D match-2 / collapse puzzle prototype based on mobile tap-and-blast mechanics. The core loop centers on tapping adjacent clusters of matching color blocks on a grid to clear them, prompting the remaining blocks above to cascade down while fresh tiles spawn to refill columns. Each level requires clearing specified numbers of colored blocks (quests) within a restricted move count while accumulating score and coin rewards; clearing larger groups synthesizes special booster items (vertical/horizontal rockets, area bombs, and color bombs) capable of triggering cascading clearing effects. This repository is a personal prototype/demo project, not a maintained product, with the last substantial work committed in 2022.

## Gameplay

- **Grid Initialization & Refill**: Manages a 2D column-based grid array, spawning block prefabs from above and refilling empty column slots (`Assets/Scripts/Gameplay/GameGrid.cs`).
- **Cluster Matching & Cascading**: Uses breadth-first search / queue traversal to find connected tiles of the same color, destroying them and cascading remaining blocks downward at configured speed (`Assets/Scripts/Gameplay/Square.cs`, `Assets/Scripts/Gameplay/GameGrid.cs`).
- **Booster Generation**: Spawns directional rockets when clearing 5–7 blocks, area bombs when clearing 8–9 blocks, and color bombs when clearing 10+ blocks (`Assets/Scripts/Gameplay/Square.cs`).
- **Booster Activation & Combos**: Executes horizontal/vertical line clears for rockets, 3x3 explosive radius for bombs, all matching color clears for color bombs, and chains neighboring activated boosters (`Assets/Scripts/Gameplay/Square.cs`, `Assets/Scripts/Gameplay/BlackBomb.cs`).
- **Board Shuffling**: Scans the board for remaining valid moves and shuffles the grid when no adjacent matching blocks exist to prevent deadlocks (`Assets/Scripts/Gameplay/GameGrid.cs`, `Assets/Scripts/Shuffler.cs`).
- **Objectives & Turn Management**: Tracks move counts, decrements quest targets upon tile destruction, updates the level score progress bar, and triggers win/lose screens (`Assets/Scripts/Managers/GameManager.cs`, `Assets/Scripts/Classes/Quest.cs`).
- **ScriptableObject Level Configuration**: Configures grid dimensions, allowed moves, quest targets, active color types, and score multipliers across level assets (`Assets/Scripts/ScriptableObjects/LevelSO.cs`, `Assets/Scripts/ScriptableObjects/LevelListSO.cs`).
- **Player Input**: Raycasts 2D mouse clicks from the screen to detect clicked block colliders and activate interactions (`Assets/Scripts/Controls/Mouse.cs`, `Assets/Scripts/Interfaces/IPlayerControls.cs`, `Assets/Scripts/Gameplay/GameLogic.cs`).
- **Data Persistence**: Saves and loads player health, coins, and level progress to local persistent storage in JSON format with XOR obfuscation (`Assets/Scripts/DataPersistence/DataPersistenceManager.cs`, `Assets/Scripts/DataPersistence/FileDataHandler.cs`, `Assets/Scripts/DataPersistence/Data/GameData.cs`, `Assets/Scripts/DataPersistence/MyGameDatas.cs`).
- **Audio Control**: Plays background music and UI/game sound effects with random pitch variation, persisting volume preferences to PlayerPrefs (`Assets/Scripts/Managers/AudioManager.cs`, `Assets/Scripts/Managers/SettingManager.cs`).
- **Adaptive Camera**: Dynamically adjusts camera orthographic size and position offsets to center the game board based on grid width and height (`Assets/Scripts/Controllers/CameraController.cs`).
- **User Interface**: Displays current moves, quest targets, score progress bar, settings modal, and end-of-game victory/defeat dialogs (`Assets/Scripts/Managers/UIManager.cs`, `Assets/Scripts/UI/EndGamePanel.cs`, `Assets/Scripts/UI/QuestPanel.cs`, `Assets/Scripts/Controllers/MainMenuController.cs`).

## Tech stack

- **Game Engine**: Unity 2021.3.8f1 (defined in `ProjectSettings/ProjectVersion.txt`)
- **Language**: C# (.NET / Unity Scripting Runtime)
- **Target Platform**: Desktop / Mobile 2D
- **Notable Packages** (from `Packages/manifest.json`):
  - `com.unity.2d.sprite`: 1.0.0 (2D Sprite renderer support)
  - `com.unity.2d.tilemap`: 1.0.0 (2D Tilemap module)
  - `com.unity.textmeshpro`: 3.0.6 (UI text rendering)
  - `com.unity.ugui`: 1.0.0 (Unity UI system)
  - `com.unity.timeline`: 1.6.4 (Sequencing and animation timelines)
  - `com.unity.ads`: 3.7.5 (Unity Ads integration)
  - `com.unity.test-framework`: 1.1.31 (Unity Test Framework package)

## Project structure

| Path | Description | Type |
| --- | --- | --- |
| `Assets/Scripts/Classes` | Data classes such as `Quest.cs` | Own code |
| `Assets/Scripts/Controllers` | Camera setup and main menu controllers | Own code |
| `Assets/Scripts/Controls` | Mouse input detection and raycasting | Own code |
| `Assets/Scripts/DataPersistence` | JSON-based save/load system, data handlers, and model definitions | Own code |
| `Assets/Scripts/Enums` | Enumerations for PlayerPrefs keys | Own code |
| `Assets/Scripts/Exceptions` | Custom exceptions (`InvalidBlockException`, `InvalidGridException`) | Own code |
| `Assets/Scripts/Gameplay` | Core grid mechanics, cascading logic, and square/bomb block logic | Own code |
| `Assets/Scripts/Interfaces` | Interfaces for blocks and player controls | Own code |
| `Assets/Scripts/Managers` | Audio, game state, assets, UI, and settings managers | Own code |
| `Assets/Scripts/ScriptableObjects` | Level and level list ScriptableObject definitions | Own code |
| `Assets/Scripts/UI` | Quest HUD and end-game victory/defeat panel scripts | Own code |
| `Assets/Scenes` | Main game scenes (`MainMenu.unity`, `Game.unity`) | Own data / assets |
| `Assets/Scriptable Objects` | Level asset definitions (`level 0.asset` to `level 5.asset`) and level list | Own data |
| `Assets/Prefabs` | Game grid, blocks, bombs, audio manager, and UI prefabs | Own data / assets |
| `Assets/Sprites` | Cube sprites, rocket, bomb, and booster visual assets | Own data / assets |
| `Assets/Sounds` | Background music track (`Chunky_Monkey.wav`) and game sound effects | Third-party / audio data |
| `Assets/Materials` | Sprite and VFX materials | Own data / assets |
| `Assets/Meshes` | Quad meshes for rendering | Own data / assets |
| `Assets/Fonts` | Font assets used across UI | Third-party / font data |
| `Assets/Animation` | Level complete animations | Own data / assets |
| `Assets/Packages/Candy UI with Animation Free - Cyko` | Vendored candy-themed UI assets, animations, and documentation | Third-party assets |
| `Assets/Packages/Easter_UI` | Vendored Easter UI sprite package | Third-party assets |
| `Assets/Packages/GUI Mobile Hyper-Casual` | Vendored mobile hyper-casual GUI components and PSDs | Third-party assets |
| `Assets/Packages/TextMesh Pro` | Vendored TextMesh Pro essential resources, shaders, and fonts | Third-party package assets |
| `Assets/Packages/ToonExplosion v1.0` | Vendored cartoon explosion sprite effects and animator | Third-party assets |
| `Packages/` | Unity package manifest (`manifest.json`) and lockfile | Configuration |
| `ProjectSettings/` | Unity project configuration files including `EditorBuildSettings.asset` and `ProjectVersion.txt` | Configuration |

## Running / building

1. Install **Unity 2021.3.8f1** (or compatible 2021.3 LTS version) via Unity Hub.
2. In Unity Hub, click **Add project from disk** and choose the root directory of this repository (`match_2_blast`).
3. Open the project in Unity Editor.
4. Open the entry scene: `Assets/Scenes/MainMenu.unity` (configured as index `0` in `ProjectSettings/EditorBuildSettings.asset`).
5. Press the **Play** button in the editor toolbar to run the game in Editor Play Mode.
6. To create a standalone build, navigate to `File > Build Settings...`, confirm that `Assets/Scenes/MainMenu.unity` (index 0) and `Assets/Scenes/Game.unity` (index 1) are enabled, select the target platform (e.g. PC, Mac & Linux Standalone or Android), and click **Build**.

*Note: The project was documented statically from source; it was not built or run in this environment.*

## Repository notes

- **Tests**: None implemented. Although `com.unity.test-framework` is declared in `Packages/manifest.json`, there are no automated unit or playmode test files in `Assets/`.
- **CI/CD**: None configured. The repository does not contain GitHub Actions workflows or automated build pipelines.
- **Commit History**: The git history contains a single commit (`e2b110dc8811f127fd6308e50c9aed2cf9989618`) made on 2022-11-09 with message `.`.
- **Project Scope**: This repository is a learning/portfolio prototype developed to explore Unity 2D match-2 blast mechanics, grid cascading logic, ScriptableObject-based level progression, and basic JSON save persistence.

## Third-party assets

- `Assets/Packages/Candy UI with Animation Free - Cyko`: Mobile candy UI elements, buttons, and animations. Licence file: Not present in repository.
- `Assets/Packages/Easter_UI`: Easter-themed UI sprite kit. Licence file: Not present in repository.
- `Assets/Packages/GUI Mobile Hyper-Casual`: Mobile casual UI graphics, buttons, icons, and PSD source files. Licence file: Not present in repository.
- `Assets/Packages/TextMesh Pro`: Unity TextMesh Pro shaders, essential font assets, and emoji graphics. Licence file: Attribution file present (`Assets/Packages/TextMesh Pro/Sprites/EmojiOne Attribution.txt`), full licence text not present.
- `Assets/Packages/ToonExplosion v1.0`: Cartoon explosion VFX texture sheets and animation controllers. Licence file: Not present in repository.
