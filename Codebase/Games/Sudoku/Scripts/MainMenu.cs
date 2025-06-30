//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-29
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
    public GameObject newGamePanel;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Transform highScoresContainer;
    public GameObject highScoreEntryPrefab;

    void Start()
    {
        // ✰ Let's see if there's a saved game to continue! ✰
        continueButton.interactable = File.Exists(Application.persistentDataPath + "/sudokuSaveData.json");

        // ✰ Time to load our sound preferences from last time. ✰
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

    public void NewGame(int difficulty)
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetInt("load_game", 0); // ✰ 0 means we're not loading a game. ✰
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        PlayerPrefs.SetInt("load_game", 1); // ✰ 1 means we *are* loading a game. ✰
        SceneManager.LoadScene("GameScene");
    }

    public void OpenNewGamePanel()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        newGamePanel.SetActive(true);
    }

    public void CloseNewGamePanel()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        newGamePanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        settingsPanel.SetActive(false);
    }
    public void OpenAbout()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        aboutPanel.SetActive(true);
    }
    public void CloseAbout()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        aboutPanel.SetActive(false);
    }

    public void OpenHighScores()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        highScoresPanel.SetActive(true);
        PopulateHighScores();
    }
    public void CloseHighScores()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClickSound();
        highScoresPanel.SetActive(false);
    }

    public void OnBGMVolumeChanged()
    {
        float volume = bgmSlider.value;
        PlayerPrefs.SetFloat("bgmVolume", volume);
        if (AudioManager.instance != null) AudioManager.instance.UpdateBGMVolume();
    }

    public void OnSFXVolumeChanged()
    {
        float volume = sfxSlider.value;
        PlayerPrefs.SetFloat("sfxVolume", volume);
        if (AudioManager.instance != null) AudioManager.instance.UpdateSFXVolume();
    }

    private void PopulateHighScores()
    {
        // ✰ Let's clear out the old scores before showing the new ones. ✰
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
