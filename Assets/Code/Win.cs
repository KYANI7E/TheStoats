using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private TMP_Text waveNumber;

    public TextMeshProUGUI waveText;

    public static Win instance;
    
    [SerializeField]
    public int lives;


    // Start is called before the first frame update
    void Start() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        waveNumber.text = "Wave: " + GameState.instance.waveNum;
    }

    public void LoseLife(int l)
    {
        lives -= l;
        if (lives < 0)
            WinGame();
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
