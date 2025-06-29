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
        // ✰ This is a special 'Singleton' pattern. It makes sure we only ever have one AudioManager, even when we switch scenes. ✰
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
        // ✰ Let's get the music going based on our saved settings! ✰
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
