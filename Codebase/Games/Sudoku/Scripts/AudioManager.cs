//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-07-01
//	Project: 10TinyBuilds-Sudoku
//  Description: AudioManager.cs
//============================================================================

// This script manages the audio for a Sudoku game, including background music and sound effects.

// Ignore Spelling: Sudoku
// Ignore Spelling: bgm, sfx

using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

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

            // ✰ Let's find our AudioSource components by code to make sure we always have them. ✰
            AudioSource[] sources = GetComponents<AudioSource>();
            bgmSource = sources[0];
            sfxSource = sources[1];

            // ✰ Configure the BGM source correctly. ✰
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
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
    }

    public void PlayCorrectSound()
    {
        if (sfxSource.volume > 0) sfxSource.PlayOneShot(correctSound);
    }

    public void PlayWrongSound()
    {
        if (sfxSource.volume > 0) sfxSource.PlayOneShot(wrongSound);
    }

    public void PlayClickSound()
    {
        if (sfxSource.volume > 0) sfxSource.PlayOneShot(clickSound);
    }

    public void UpdateBGMVolume()
    {
        float volume = PlayerPrefs.GetFloat("bgmVolume", 1f);
        bgmSource.volume = volume;

        if (volume > 0 && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
        else if (volume == 0 && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    public void UpdateSFXVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }
}
