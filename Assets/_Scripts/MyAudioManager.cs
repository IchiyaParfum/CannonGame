using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource BackgroundSource;
    public AudioSource MenuSource;

    public AudioClip MenuSound;
    public AudioClip BackgroundSound;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Singleton instance.
    public static MyAudioManager Instance = null;

    private float volume = 0.5f;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
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
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

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
