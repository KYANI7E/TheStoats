using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Seed : MonoBehaviour
{
    public static Seed instance;

    [SerializeField]
    private GameObject seedMenu;

    [SerializeField]
    private int seedValue;

    [SerializeField]
    private GameObject inputText;
    
    private TMP_InputField inputField;

    [SerializeField]
    private string seedText = null;

    
    private bool loaded = true;

    public int landSteps;
    public int fogSteps;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }

        inputField = inputText.GetComponent<TMP_InputField>();

        landSteps = MapGenerator.instance.maxSteps;
        fogSteps = MapGenerator.instance.fogSteps;
}

    // Update is called once per frame
    void Update() {
        if (inputField.isFocused) {
            loaded = false;
            int value;
            try {
                seedText = inputField.text;
                value = int.Parse(seedText);
                seedValue = Mathf.Clamp(value, 0, int.MaxValue);

            } catch {

            }

        }
        if (loaded == false && !inputField.isFocused) {
            MapGenerator.instance.GenEverything();
            loaded = true;
        }
    }



    public int GetSeed()
    {
        return seedValue;
    }
}
