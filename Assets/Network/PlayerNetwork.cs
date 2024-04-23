using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{



    //private void Start()
    //{
    //    NetworkManagerUI nwmui = GameObject.Find("NetworkManagerUI").GetComponent<NetworkManagerUI>();

    //    nwmui.defenderBtn.onClick.AddListener(() =>
    //    {

    //    });
    //    nwmui.attackerBtn.onClick.AddListener(() =>
    //    {
    //        SetPlayerServerRpc(OwnerClientId);
    //        Debug.Log("Attacker Button clicked");
    //    });
    //}

    //[ServerRpc]
    //public void SetPlayerServerRpc(ulong clientId)
    //{
    //    Debug.Log("Setting Attacker");
    //    Spawning.instance.SetPlayer(clientId);

    //}

}
