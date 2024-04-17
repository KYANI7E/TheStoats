using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    [SerializeField]
    private float musicVolume;
    Slider musicSlider;
    TMP_Text musicNum;
    [SerializeField]
    private float soundVolume;
    Slider soundSlider;
    TMP_Text soundNum;
    [SerializeField]
    private float brightness;
    Slider brightnessSlider;
    TMP_Text brightnessNum;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        soundSlider = GameObject.Find("Sound Slider").GetComponent<Slider>();
        brightnessSlider = GameObject.Find("Brightness Slider").GetComponent<Slider>();

        musicNum = GameObject.Find("Music Volume Number").GetComponent<TMP_Text>();
        soundNum = GameObject.Find("Sound Volume Number").GetComponent<TMP_Text>();
        brightnessNum = GameObject.Find("Brightness Number").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        musicVolume = musicSlider.value;
        musicNum.text = musicSlider.value.ToString();
        soundVolume = soundSlider.value;
        soundNum.text = soundSlider.value.ToString();
        brightness = brightnessSlider.value;
        brightnessNum.text = brightnessSlider.value.ToString();
    }
}
