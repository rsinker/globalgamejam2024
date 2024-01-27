using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

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
        s.Play();
    }

    public void StopSound(string soundName)
    {
        GetAudioSource(soundName).Stop();
    }

    public void PlaySoundOnce(Sound s)
    {
        s.m_Source.loop = false;
        s.m_Source.Play();
    }

    public void PlaySoundLooped(Sound s)
    {
        s.m_Source.loop = true;
        s.m_Source.Play();
    }

    public void StopSound(Sound s)
    {
        s.m_Source.Stop();
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