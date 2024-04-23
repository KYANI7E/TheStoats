using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
using UnityEngine.EventSystems;
using TMPro;

public class Spawning : NetworkBehaviour
{

    [SerializeField]
    private GameObject spawnMenu;

    public static Spawning instance;

    [SerializeField]
    private GameObject[] units;

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

    [SerializeField]
    private GameObject startWaveButton;

    [SerializeField]
    private Tilemap fog;

    private void Awake()
    {
        if(instance == null ) {
            instance = this;
        } else if(instance != this){
            Destroy(this);
        }
    }

    public void Start()
    {
        curMaxSouls = startSouls;
        curSouls = curMaxSouls;
        UpdateSoulsText();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectUnitServerRpc(int i)
    {
        curUnit = units[i];
    }

    public void Update()
    {
        if (!IsOwner)
            return;

        SpawnUnit();
        if(unitsAlive > 0) {
            startWaveButton.SetActive(true);
        } else {
            startWaveButton.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerServerRpc(ulong clientId) {
        GetComponent<NetworkObject>().ChangeOwnership(clientId);
        UpdatePlayerClientRpc();
        BaseBuilding.instance.GiveUpOwnerShipClientRpc();


    }

    [ClientRpc]
    public void GiveUpOwnerShipClientRpc()
    {
        spawnMenu.SetActive(false);
        //Color c = Color.white;
        //c.a = 1;
        //fog.color = c;
    }

    [ClientRpc]
    public void UpdatePlayerClientRpc()
    {
        Color c = Color.white;
        if (IsOwner) {
            spawnMenu.SetActive(true);
            c.a = 1;
        } else {
            spawnMenu.SetActive(false);
            c.a = .70f;
        }
        fog.color = c;
    }


    private void UpdateSoulsText()
    {
        soulText.text = $"{curSouls}";
    }

    public void UnitDied()
    {
        unitsAlive--;
        if (unitsAlive == 0 && GameState.instance.state.Value == State.Play) {
            GameState.instance.state.Value = State.Setup;
            curMaxSouls = Mathf.RoundToInt(curMaxSouls * soulMultiplier);
            curSouls = curMaxSouls;
            UpdateSoulsText();
            placed.Clear();
        }
    }

    private void SpawnUnit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (GameState.instance.state.Value != State.Setup)
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


        SpawnUnitServerRpc((int)pos.x, (int)pos.y);
        UpdateSoulsText();
    }

    [ServerRpc]
    private void SpawnUnitServerRpc(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        GameObject g = Instantiate(curUnit, pos, Quaternion.identity);
        g.GetComponent<NetworkObject>().Spawn(true);
        placed.Add(pos, g);
        unitsAlive++;
        curSouls -= g.GetComponent<Unit>().soulCost;
    }
}
