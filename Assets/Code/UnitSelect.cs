using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{

    [SerializeField]
    public int unit;

    
    public void SelectUnitButton()
    {
        Spawning.instance.SelectUnitServerRpc(unit);
    }

    public void SelectTowertButton()
    {
        BaseBuilding.instance.SelectUnitServerRpc(unit);
    }
}
