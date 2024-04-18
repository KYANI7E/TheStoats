using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawning : MonoBehaviour
{

    public static Spawning instance;

    [SerializeField]
    private GameObject curUnit;

    public int unitsAlive = 0;

    private Dictionary<Vector2, GameObject> placed = new Dictionary<Vector2, GameObject>();

    [SerializeField]
    private float soulMultiplier;
    [SerializeField]
    private int startSouls;
    private int curMaxSouls;
    private int curSouls;

    [SerializeField]
    private TMP_Text soulText;

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
        curMaxSouls = startSouls;
    }

    public void Update()
    {
        SpawnUnit();
    }

    private void UpdateSoulsText()
    {
        soulText.text = $"Souls: {curSouls}";
    }

    public void UnitDied()
    {
        unitsAlive--;
        if (unitsAlive == 0 && GameState.instance.state == State.Play) {
            GameState.instance.state = State.Setup;
            curMaxSouls = Mathf.RoundToInt(curMaxSouls * soulMultiplier);
            curSouls = curMaxSouls;
            UpdateSoulsText();
            placed.Clear();
        }
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

        if (placed.ContainsKey(pos))
            return;

        if ( ! PathingMaster.instance.ValidSpawnPosition(pos))
            return;

        if (curUnit.GetComponent<Unit>().soulCost > curSouls)
            return;



        GameObject g = Instantiate(curUnit, pos, Quaternion.identity);
        placed.Add(pos, g);
        unitsAlive++;
        curSouls -= g.GetComponent<Unit>().soulCost;
        UpdateSoulsText();
    }
}
