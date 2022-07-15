using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : SingletonManager<SoundManager> 
{
    // Start is called before the first frame update
    [SerializeField] Sound[] sounds;
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
        
    }

    void Start() {
        Play("Background");
    }

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

}
