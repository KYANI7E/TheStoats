using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField]
    private GameObject winScreen;

    public TextMeshProUGUI waveText;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WinGame() {
        winScreen.SetActive(true);
        string text = "in " + GameState.instance.waveNum + " waves";
        waveText.SetText(text);
    }

    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
