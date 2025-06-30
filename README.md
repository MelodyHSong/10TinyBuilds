# 10TinyBuilds
Repository for my 10 tiny games challenge. 



# Change Log - Sudoku Tiny Build

Version InDev 0.0.2 - 2025-06-29

- Initial Major Version: This marks the first complete, feature-rich version of the game.

- Main Menu Scene: Created a dedicated main menu scene (MainMenu) to act as the game's hub.

- Game Scene: Gameplay is now contained within its own scene (GameScene).

- Difficulty Selection: Added an interface and logic for choosing Easy, Medium, or Hard difficulty, which affects how many numbers are removed from the puzzle.

- Save/Load System: Implemented a robust save/load system using JSON serialization. The game automatically saves on quit and can be continued from the main menu.

- Persistent Audio Manager: Created a singleton AudioManager that persists between scenes to handle background music and sound effects.

- Settings Menu: Added a settings panel where the player can mute/unmute background music and sound effects. Preferences are saved using PlayerPrefs.

- High Score Tracker: The game now saves the last 10 scores (both completed and "gave up") to a file, which can be viewed from the main menu.

- Enhanced Win State: The win screen can be closed to allow the player to view their completed puzzle. The "Solve" button is now hidden and replaced with a "Results" button after a win.

- Improved User Feedback: Incorrectly answered cells now turn red and remain on the grid, rather than being cleared immediately. This allows the player to see and correct their mistakes.
