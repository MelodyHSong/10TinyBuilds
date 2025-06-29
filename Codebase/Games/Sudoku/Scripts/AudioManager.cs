//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-28
//	Project: 10TinyBuilds-Sudoku
//  Description: AudioManager.cs
//============================================================================

// This script manages the audio for a Sudoku game, including background music and sound effects.

// Ignore Spelling: Sudoku
// Ignore Spelling: bgm, sfx

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip clickSound;

    private void Awake()
    {
        // * Singleton pattern to ensure only one AudioManager exists *
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // * Apply settings and start the music *
        UpdateBGMVolume();
        UpdateSFXVolume();
        bgmSource.Play();
    }

    public void PlayCorrectSound()
    {
        sfxSource.PlayOneShot(correctSound);
    }

    public void PlayWrongSound()
    {
        sfxSource.PlayOneShot(wrongSound);
    }

    public void PlayClickSound()
    {
        sfxSource.PlayOneShot(clickSound);
    }

    public void UpdateBGMVolume()
    {
        bgmSource.mute = PlayerPrefs.GetInt("bgmMuted", 0) == 1;
    }

    public void UpdateSFXVolume()
    {
        sfxSource.mute = PlayerPrefs.GetInt("sfxMuted", 0) == 1;
    }
}
