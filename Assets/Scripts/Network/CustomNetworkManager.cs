using System;
using System.Collections;
using System.Reflection;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        var startPosition = GetStartPosition();
        var player = Instantiate(NetworkManager.singleton.spawnPrefabs[0], startPosition.position, startPosition.rotation);
        player.GetComponent<NetworkingPlayerController>().SetIndex(NetworkServer.connections.Count);

        NetworkServer.AddPlayerForConnection(conn, player);
        if(maxConnections == NetworkServer.connections.Count)
        {
            var bot = Instantiate(spawnPrefabs[3], new Vector3(-0.6f, 1.5f, -1.7f), Quaternion.Euler(0, 0, 0));
            NetworkServer.Spawn(bot);

            var gameManager = Instantiate(spawnPrefabs[2]);
            NetworkServer.Spawn(gameManager);
             

            var enviromentBehaviourController = Instantiate(playerPrefab);
            NetworkServer.Spawn(enviromentBehaviourController);
            var block = Instantiate(NetworkManager.singleton.spawnPrefabs[1]); 
        }
    } 
}
