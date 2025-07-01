# Change Log - Sudoku Tiny Build

# Version InDev 0.0.5 2025-07-01

Bug Fix (Save/Load): Refactored the save data structure to use single-dimensional arrays instead of multi-dimensional arrays, as Unity's JsonUtility does not support them. This fixes a critical bug where loading a saved game would cause a NullReferenceException and fail to generate the grid.

Bug Fix (Audio System): Added a boolean flag isGenerating to SudokuGameManager to prevent OnCellValueChanged from firing during initial puzzle generation and loading, which was causing unwanted sound effects to play.

---

# Version InDev 0.0.4 - 2025-06-30

Bug Fix (Audio System): Refactored the AudioManager to get its AudioSource components via code in the Awake function. This prevents a race condition during scene loading that was causing audio source references to be lost, which in turn fixes a bug where grid generation would fail when loading the GameScene from the MainMenu

---

# Version InDev 0.0.3 - 2025-06-30

Bug Fix (Audio System): Replaced mute toggles with volume sliders for BGM and SFX to provide more granular control and fix persistent mute bugs. Refactored AudioManager to correctly handle Play() and Stop() calls based on volume.

UI Functionality: Added functions to correctly open and close the "New Game" difficulty selection panel from the main menu.

UI Functionality: Added click sounds to all buttons in the main menu for better user feedback.

---

# Version InDev 0.0.2 - 2025-06-29

Initial Major Version: This marks the first complete, feature-rich version of the game.

Main Menu Scene: Created a dedicated main menu scene (MainMenu) to act as the game's hub.

Game Scene: Gameplay is now contained within its own scene (GameScene).

Difficulty Selection: Added an interface and logic for choosing Easy, Medium, or Hard difficulty.

Save/Load System: Implemented a robust save/load system using JSON serialization.

Persistent Audio Manager: Created a singleton AudioManager that persists between scenes.

Settings Menu: Added a settings panel where the player can control audio.

High Score Tracker: The game now saves the last 10 scores to a file.

Enhanced Win State: The win screen can be closed to allow the player to view their completed puzzle.

Improved User Feedback: Incorrectly answered cells now turn red and remain on the grid.
