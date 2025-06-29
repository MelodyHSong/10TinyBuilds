

//============================================================================
//	Author: @MelodyHSong ✰
//	Date: 2025-06-28
//	Project: 10TinyBuilds-Sudoku
//  Description: SaveLoadManager.cs
//============================================================================

// This script manages saving and loading game data and high scores for a Sudoku game. 

using UnityEngine;
using System.IO;
using System.Collections.Generic;

//Ignore Spelling: Sudoku


public static class SaveLoadManager
{
    private static string saveDataPath = Application.persistentDataPath + "/sudokuSaveData.json";
    private static string highScoresPath = Application.persistentDataPath + "/highScores.json";

    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveDataPath, json);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(saveDataPath))
        {
            string json = File.ReadAllText(saveDataPath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return null;
    }

    public static void DeleteSavedGame()
    {
        if (File.Exists(saveDataPath))
        {
            File.Delete(saveDataPath);
        }
    }

    public static void SaveHighScore(int score, string status)
    {
        HighScoresData highScoresData = LoadHighScores();

        HighScore newHighScore = new HighScore
        {
            score = score,
            status = status,
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        highScoresData.highScores.Insert(0, newHighScore);

        // * Keep only the last 10 scores *
        if (highScoresData.highScores.Count > 10)
        {
            highScoresData.highScores.RemoveAt(10);
        }

        string json = JsonUtility.ToJson(highScoresData);
        File.WriteAllText(highScoresPath, json);
    }

    public static HighScoresData LoadHighScores()
    {
        if (File.Exists(highScoresPath))
        {
            string json = File.ReadAllText(highScoresPath);
            return JsonUtility.FromJson<HighScoresData>(json);
        }
        return new HighScoresData();
    }
}
