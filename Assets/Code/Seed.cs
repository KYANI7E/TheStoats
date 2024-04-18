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

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }

        inputField = inputText.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update() {
        int value;
        try {
            seedText = inputField.text;
            value = int.Parse(seedText);
            seedValue = Mathf.Clamp(value, 0, int.MaxValue);
        } catch {

        }

        
    }

    public int GetSeed()
    {
        return seedValue;
    }
}
