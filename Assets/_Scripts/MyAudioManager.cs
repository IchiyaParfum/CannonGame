using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyAudioManager
{
    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    //Resources path relative to Resources Folder. !!!DO NOT INCLUDE FILE EXTENSION
    public const string MenuSoundFile = "_Audio/Swords_Collide-Sound_Explorer-2015600826";
    public const string BackgroundSoundFile = "_Audio/MedievalInnMusic";

    // Singleton instance.
    public static MyAudioManager Instance = null;


    // Audio players components.
    private AudioSource EffectsSource;
    private AudioSource BackgroundSource;
    private AudioSource MenuSource;

    private AudioClip MenuSound;
    private AudioClip BackgroundSound;

    // Initialize the singleton instance.
    static MyAudioManager()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = new MyAudioManager();
        }
    }

    private MyAudioManager()
    {
        GameObject g = new GameObject("MyAudioManager");
        MonoBehaviour.DontDestroyOnLoad(g);
        MenuSource = g.AddComponent<AudioSource>();
        EffectsSource = g.AddComponent<AudioSource>();
        BackgroundSource = g.AddComponent<AudioSource>();

        try
        {
            MenuSound = Resources.Load<AudioClip>(MenuSoundFile);
            BackgroundSound = Resources.Load<AudioClip>(BackgroundSoundFile);
            Debug.Log(BackgroundSound);

        }
        catch (UnityException ex)
        {
            Debug.LogError(ex);
        }
    }

    //Sets volume scale of audio sources (0 to 1.0)
    public void SetVolume(float volume)
    {
        BackgroundSource.volume = volume;
        EffectsSource.volume = volume;
        MenuSource.volume = volume;
    }

    // Play a single clip through the sound effects source.
    public void PlayEffect(AudioClip clip)
    {
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clip;
        EffectsSource.PlayOneShot(clip);
    }

    // Play a single clip through the music source.
    public void PlayMusic()
    {
        PlayMusic(BackgroundSound);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (BackgroundSource.clip != clip || !BackgroundSource.isPlaying)
        {
            Debug.Log("Play Music");
            BackgroundSource.Stop();
            BackgroundSource.loop = true;
            BackgroundSource.clip = clip;
            BackgroundSource.PlayOneShot(clip);
        }
            
    }

    public void StopMusic()
    {
        BackgroundSource.Stop();
    }

    public void PlayMenu()
    {
        PlayMenu(MenuSound);
    }

    public void PlayMenu(AudioClip clip)
    {
        MenuSource.clip = clip;
        MenuSource.PlayOneShot(clip);
    }
}
