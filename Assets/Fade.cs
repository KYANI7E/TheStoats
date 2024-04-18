using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    [SerializeField]
    private float fadeTime;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        time += Time.deltaTime;
        if (time > fadeTime)
            Destroy(this.gameObject);
    }
}
