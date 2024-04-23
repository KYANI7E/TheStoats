using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{

    [SerializeField]
    public GameObject unit;

    
    public void SelectUnitButton()
    {
        Spawning.instance.SelectUnit(unit);
        Debug.Log($"Selected: {unit}");
    }

    public void SelectTowertButton()
    {
        BaseBuilding.instance.SelectUnit(unit);
        Debug.Log($"Selected: {unit}");
    }
}
