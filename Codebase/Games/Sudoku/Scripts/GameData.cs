//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-28
//	Project: 10TinyBuilds-Sudoku
//  Description: GameData.cs
//============================================================================

// This script defines the data structures used for saving game state and high scores in a Sudoku game.

// Ignore Spelling: Sudoku

using System.Collections.Generic;

// ✰ Think of this as a save file blueprint. It holds everything we need to remember about a game in progress. ✰
[System.Serializable]
public class GameData
{
    public int[,] board;
    public int[,] solvedBoard;
    public int[,] initialPuzzle;
    public bool[,] mistakesMade;
    public int score;
    public int difficulty;
}

// ✰ This blueprint is for a single entry in our high score list. ✰
[System.Serializable]
public class HighScore
{
    public int score;
    public string status; // ✰ So we know if the player won or gave up. ✰
    public string date;
}

// ✰ And this holds the entire list of our top scores! ✰
[System.Serializable]
public class HighScoresData
{
    public List<HighScore> highScores = new List<HighScore>();
}
