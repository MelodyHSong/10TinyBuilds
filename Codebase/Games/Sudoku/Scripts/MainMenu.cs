//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-28
//	Project: 10TinyBuilds-Sudoku
//  Description: MainMenu.cs
//============================================================================

// This script manages the main menu of a Sudoku game, allowing players to start a new game,
// continue a saved game, access settings, view high scores, and quit the game.

// Ignore Spelling: Sudoku
// Ignore Spelling: bgm, sfx

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public GameObject settingsPanel;
    public GameObject aboutPanel;
    public GameObject highScoresPanel;
    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Transform highScoresContainer;
    public GameObject highScoreEntryPrefab;

    void Start()
    {
        // * Check if a saved game exists and enable/disable the continue button *
        continueButton.interactable = File.Exists(Application.persistentDataPath + "/sudokuSaveData.json");

        // * Load and apply sound settings *
        bgmToggle.isOn = PlayerPrefs.GetInt("bgmMuted", 0) == 0;
        sfxToggle.isOn = PlayerPrefs.GetInt("sfxMuted", 0) == 0;
    }

    public void NewGame(int difficulty)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetInt("load_game", 0); // * 0 for false *
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("load_game", 1); // * 1 for true *
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings() { settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); }
    public void OpenAbout() { aboutPanel.SetActive(true); }
    public void CloseAbout() { aboutPanel.SetActive(false); }

    public void OpenHighScores()
    {
        highScoresPanel.SetActive(true);
        PopulateHighScores();
    }
    public void CloseHighScores() { highScoresPanel.SetActive(false); }

    public void OnBGMToggle(bool is_on)
    {
        PlayerPrefs.SetInt("bgmMuted", is_on ? 0 : 1);
        if (AudioManager.instance != null) AudioManager.instance.UpdateBGMVolume();
    }

    public void OnSFXToggle(bool is_on)
    {
        PlayerPrefs.SetInt("sfxMuted", is_on ? 0 : 1);
        if (AudioManager.instance != null) AudioManager.instance.UpdateSFXVolume();
    }

    private void PopulateHighScores()
    {
        // * Clear previous entries *
        foreach (Transform child in highScoresContainer)
        {
            Destroy(child.gameObject);
        }

        HighScoresData highScoresData = SaveLoadManager.LoadHighScores();

        if (highScoresData.highScores.Count == 0)
        {
            GameObject entry = Instantiate(highScoreEntryPrefab, highScoresContainer);
            entry.GetComponent<TMP_Text>().text = "No scores yet!";
        }
        else
        {
            foreach (HighScore score in highScoresData.highScores)
            {
                GameObject entry = Instantiate(highScoreEntryPrefab, highScoresContainer);
                entry.GetComponent<TMP_Text>().text = $"{score.date} - Score: {score.score} ({score.status})";
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
