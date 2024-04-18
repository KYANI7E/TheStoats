using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{

    public static Spawning instance;

    [SerializeField]
    private GameObject curUnit;

    private int unitsAlive = 0;

    private void Awake()
    {
        if(instance == null ) {
            instance = this;
        } else if(instance != this){
            Destroy(this);
        }
    }

    public void SelectUnit(GameObject obj)
    {
        curUnit = obj;
    }

    public void Update()
    {
        SpawnUnit();
    }

    public void UnitDied()
    {
        unitsAlive--;
        if (unitsAlive == 0)
            GameState.instance.state = State.Setup;
    }

    private void SpawnUnit()
    {
        if (GameState.instance.state != State.Setup)
            return;
            
        if (curUnit == null)
            return;

        if ( ! Input.GetKeyDown(KeyCode.Mouse0))
            return;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        if ( ! PathingMaster.instance.ValidSpawnPosition(pos))
            return;

        Instantiate(curUnit, pos, Quaternion.identity);
        unitsAlive++;
    }
}
