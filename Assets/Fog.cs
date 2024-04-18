using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fog"))
            Debug.Log("wwdwdw");
        if (collision.gameObject.GetComponentInParent<UnitPathing>() == null)
            return;


        collision.gameObject.GetComponentInParent<UnitPathing>().ClearFog();
    }
}
