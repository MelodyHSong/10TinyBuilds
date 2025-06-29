//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-28
//	Project: 10TinyBuilds-Sudoku
//  Description: GameData.cs
//============================================================================

// This script defines the data structures used for saving game state and high scores in a Sudoku game.

// Ignore Spelling: Sudoku

using System.Collections.Generic;

// * A class to hold all the data for a saved game state *
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

// * A class for a single high score entry *
[System.Serializable]
public class HighScore
{
    public int score;
    public string status; // * "Completed" or "Gave Up" *
    public string date;
}

// * A class to hold a list of all high scores *
[System.Serializable]
public class HighScoresData
{
    public List<HighScore> highScores = new List<HighScore>();
}
