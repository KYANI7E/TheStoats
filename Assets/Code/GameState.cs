using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State { Play, Setup, Pause, Lose, Win}
public class GameState : MonoBehaviour
{

    public static GameState instance;

    public State state;

    public int waveNum = 0;

    public GameObject setupUI;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    void Start()
    {
        state = State.Setup;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Setup) {
            setupUI.SetActive(true);
        }
    }

    public void StartWave()
    {
        if (Spawning.instance.unitsAlive == 0)
            return;

        if (state != State.Setup)
            return;

        setupUI.SetActive(false);

        state = State.Play;
        waveNum++;
    }
}
