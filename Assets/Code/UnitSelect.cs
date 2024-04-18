using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{

    [SerializeField]
    private GameObject unit;

    
    public void SelectUnitButton()
    {
        Spawning.instance.SelectUnit(unit);
        Debug.Log($"Selected: {unit}");
    }
}
