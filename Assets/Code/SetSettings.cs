using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SetSettings : MonoBehaviour
{
    [SerializeField]
    public static float musicVolume = 20;
    Slider musicSlider;
    TMP_Text musicNum;
    [SerializeField]
    public static float soundVolume;
    Slider soundSlider;
    TMP_Text soundNum;
    [SerializeField]
    public static float brightness;
    Slider brightnessSlider;
    TMP_Text brightnessNum;

    [SerializeField]
    public static bool soundOff;
    Toggle soundSwitch;

    [SerializeField]
    private GameObject settingsMenu;
    bool setVarsBool = false;

    private PauseMenu canvas;


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        FindSettings();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FindSettings();
    }

    // Update is called once per frame
    void Update() { 
        if (settingsMenu.activeSelf == true) {
            if (!setVarsBool) {
                SetVars();
                setVarsBool = true;
            }

            musicVolume = musicSlider.value;
            musicNum.text = musicSlider.value.ToString();
            soundVolume = soundSlider.value;
            soundNum.text = soundSlider.value.ToString();
            brightness = brightnessSlider.value;
            brightnessNum.text = brightnessSlider.value.ToString();

            soundOff = soundSwitch.isOn;
        }
    }

    void SetVars() {
        musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        soundSlider = GameObject.Find("Sound Slider").GetComponent<Slider>();
        brightnessSlider = GameObject.Find("Brightness Slider").GetComponent<Slider>();

        musicNum = GameObject.Find("Music Volume Number").GetComponent<TMP_Text>();
        soundNum = GameObject.Find("Sound Volume Number").GetComponent<TMP_Text>();
        brightnessNum = GameObject.Find("Brightness Number").GetComponent<TMP_Text>();

        soundSwitch = GameObject.Find("Sound Toggle").GetComponent<Toggle>();
    }

    private void FindSettings() {
        try {
            canvas = GameObject.Find("MainGameUI").GetComponent<PauseMenu>();
            settingsMenu = canvas.settingsMenu;
        } catch {

        }
    }
}
