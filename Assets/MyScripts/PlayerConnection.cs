﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkObjects {
    public class PlayerConnection : NetworkBehaviour {
        public TicTacToeController objectPrefab;

        private void Start() {
            Debug.Log("Llego al Start de PlayerConnection");
            if (isLocalPlayer) {
                CmdSpawnTeam();
            }
        }

        [Command]
        private void CmdSpawnTeam() {
            Debug.Log("Voy a spawnear un coso");
            // Instanciar TankController
            TicTacToeController tic = Instantiate(objectPrefab);


            // Hacer llamada al networkserver
            NetworkServer.SpawnWithClientAuthority(tic.gameObject, connectionToClient);
        }
    }
}
