using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    private void Start() {
        if (isLocalPlayer) {
            CmdSpawnTeam();
        }
    }

    [Command]
    private void CmdSpawnTeam() {
        // Instanciar TankController

        // Hacer llamada al networkserver
    }
}
