using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : SingletonManager<SoundManager> 
{
    [SerializeField] Sound[] sounds; //List of all our custom AudioClips
    [SerializeField] AudioMixer mixer; //Mixer to change the volume of background music
    public const string MUSIC_KEY = "MusicVolume";
    new void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
         }
         LoadVolume();
    }
    //<summary>
    // Plays and AudioClip from our list based on the name it was assigned
    //<summary>
    public void Play (string name) {
        Sound sound = null;
        foreach (Sound s in sounds) {
            if (s.name == name) {
                sound = s;
            }
        }
        if (sound == null) {
            Debug.LogWarning("Sound: "+ name + " not found!");
            return;
        }
        sound.source.Play();
    }

    //<summary>
    // Loads Volume according to the slider for all other scenes
    //<summary>
    public void LoadVolume() {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        mixer.SetFloat(PauseMenu.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
    }

}
