using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenuUI;
    [SerializeField]
    private GameObject hideableUI;
    public GameObject settingsMenu;

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;

            if (paused) {
                PauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
            } else {
                PauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    public void ResumeGame() {
        PauseMenuUI.SetActive(false);
    }

    public void Settings() {
        hideableUI.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Quit() {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Back() {
        hideableUI.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
