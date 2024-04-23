using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
using UnityEngine.EventSystems;


public class BaseBuilding : NetworkBehaviour
{

    public static BaseBuilding instance;

    [SerializeField]
    private GameObject[] towers;

    [SerializeField]
    private GameObject curUnit;

    [SerializeField]
    private GameObject spawnMenu;

    [SerializeField]
    private Tilemap fog;


    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    [ServerRpc(RequireOwnership =false)]
    public void SelectUnitServerRpc(int i)
    {
        curUnit = towers[i];
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;

        SpawnUnit();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerServerRpc(ulong clientId)
    {
        GetComponent<NetworkObject>().ChangeOwnership(clientId);
        UpdatePlayerClientRpc();
        Spawning.instance.GiveUpOwnerShipClientRpc();

    }

    [ClientRpc]
    public void UpdatePlayerClientRpc()
    {
        Color c = Color.white;
        if (IsOwner) {
            spawnMenu.SetActive(true);
            c.a = .70f;
        } else {
            spawnMenu.SetActive(false);
            c.a = 1;
        }
        fog.color = c;
    }

    [ClientRpc]
    public void GiveUpOwnerShipClientRpc()
    {
        spawnMenu.SetActive(false);
        //Color c = Color.white;
        //c.a = 1;
        //fog.color = c;
    }

    private void SpawnUnit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (GameState.instance.state.Value != State.Setup)
            return;


        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));


        if (!PathingMaster.instance.ValidBuildLocation(pos))
            return;


        //if (curUnit.GetComponent<Unit>().soulCost > curSouls)
        //    return;

        SpawnUnitServerRpc((int)pos.x, (int)pos.y);

    }


    [ServerRpc]
    private void SpawnUnitServerRpc(int x, int y)
    {
        if (curUnit == null)
            return;


        Debug.Log("Build Abse");
        Vector2 pos = new Vector2(x, y);
        GameObject g = Instantiate(curUnit, pos, Quaternion.identity);
        g.GetComponent<NetworkObject>().Spawn(true);
        PathingMaster.instance.UpdateNodeWithBuilding(pos, g);
    }

}
