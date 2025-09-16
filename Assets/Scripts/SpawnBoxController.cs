using System;
using System.Linq;
using Mirror;
using UnityEngine;

public class SpawnBoxController : NetworkBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out NetworkingPlayerController playerController))
        {
            var spawn = GameObject.Find("TeleportSpawn");
            playerController.Teleport(spawn.transform.position); 
        } 
    }
     
}
