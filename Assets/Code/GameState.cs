using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public enum State { Play, Setup, Pause, Lose, Win}
public class GameState : NetworkBehaviour
{

    public static GameState instance;

    public NetworkVariable<State> state = new NetworkVariable<State>(State.Setup);

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
        state.Value = State.Setup;
    }

    // Update is called once per frame
    void Update()
    {
        if(state.Value == State.Setup) {
            setupUI.SetActive(true);
        }
    }

    public void StartWave()
    {
        if (Spawning.instance.unitsAlive == 0)
            return;

        if (state.Value != State.Setup)
            return;

        setupUI.SetActive(false);

        state.Value = State.Play;
        waveNum++;
    }
}
