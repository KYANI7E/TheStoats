using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using TMPro.Examples;

public class MainMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject hideableUI;
    [SerializeField]
    private string gameScene;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject creditsMenu;
    [SerializeField]
    private GameObject seedMenu;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Back();
        }
    }

    public void Play() {
        SceneManager.LoadScene(gameScene);
    }

    public void Settings() {
        hideableUI.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Credits() {
        hideableUI.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Quit() {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Back() {
        hideableUI.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        seedMenu.SetActive(false);
    }

    public void Seed() {
        hideableUI.SetActive(false);
        seedMenu.SetActive(true);
    }
}
