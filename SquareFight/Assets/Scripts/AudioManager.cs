using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] soundEffects;
    public static AudioManager instance;

    void Awake()
    {
        instance = this;
        // Create audio source components based on audios
        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        s.source.Play();
    }
}
