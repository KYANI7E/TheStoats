using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField]
    private Button serverBtn;
    [SerializeField]
    private Button hostBtn;
    [SerializeField]
    private Button cleintBtn;
    [SerializeField]
    private Button defenderBtn;
    [SerializeField]
    private Button attackerBtn;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        cleintBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        defenderBtn.onClick.AddListener(() =>
        {
            BaseBuilding.instance.SetPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        });
        attackerBtn.onClick.AddListener(() =>
        {
            Spawning.instance.SetPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        });
    }

}
