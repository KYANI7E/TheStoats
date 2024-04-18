using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    [SerializeField]
    private float fadeTime;
    private float time;

    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        float a = Mathf.InverseLerp(0, fadeTime, time) / 3;
        Color c = sp.color;
        c.a = a;
        sp.color = c;

        time += Time.deltaTime;
        if (time > fadeTime)
            Destroy(this.gameObject);


    }
}
