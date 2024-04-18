using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCostUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int a = gameObject.GetComponentInParent<UnitSelect>().unit.GetComponent<Unit>().soulCost;
        GetComponent<TMPro.TMP_Text>().text = a.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
