using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State { Play, Setup, Pause, Lose, Win}
public class GameState : MonoBehaviour
{

    public static GameState instance;

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
