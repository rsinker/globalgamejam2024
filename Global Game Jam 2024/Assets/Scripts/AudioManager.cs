using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }

    public void PlaySoundOnce(string soundName)
    {
        var s = GetAudioSource(soundName);
        s.loop = false;
        s.Play();
    }

    public void PlaySoundLooped(string soundName)
    {
        var s = GetAudioSource(soundName);
        s.loop = true;
        GetAudioSource(soundName).Play();
    }

    public void StopSound(string soundName)
    {
        GetAudioSource(soundName).Stop();
    }

    private AudioSource GetAudioSource(string soundName)
    {
        foreach (Sound sound in sounds)
        {
            Debug.Log(sound.m_Volume);
            if(sound.name == soundName) return sound.m_Source;
        }
        Debug.LogError("Sound Name Not Found!");
        return null;
    }
}