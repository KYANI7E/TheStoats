using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SetBrightness();
        MuteSounds();
        SetMusicVolume();
        SetSoundVolume();
    }

    void SetBrightness() {
        Screen.brightness = (SetSettings.brightness / 100);
    }

    void MuteSounds() {
        bool soundOff = SetSettings.soundOff;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (soundOff) {
            camera.GetComponent<AudioListener>().enabled = false;
        } else {
            camera.GetComponent<AudioListener>().enabled = true;
        }
    }

    void SetMusicVolume() {
        GameObject Music = GameObject.Find("Music");

        if (Music) {
            Music.GetComponent<AudioSource>().volume = SetSettings.musicVolume / 100;
        }
    }

    void SetSoundVolume() {
        AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();

        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name != "Music") {
                sounds[i].volume = SetSettings.soundVolume / 100;
            }
        }
    }
}
