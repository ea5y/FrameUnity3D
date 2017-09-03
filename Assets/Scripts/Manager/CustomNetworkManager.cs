using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public GameObject PlayerSpawnPos;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, PlayerSpawnPos.transform);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        //PlayerController.Inst.GetAnimator();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }
}
