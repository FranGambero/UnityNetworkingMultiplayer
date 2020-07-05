using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkObjects {
    public class GameSync : NetworkBehaviour {
        private PlayerController myPlayer;
        private Type myLastType;

        private void Awake() {
            myPlayer = GetComponent<PlayerController>();
        }

        private void Update() {
            SyncGame();
        }

        private void SyncGame() {
            if (myPlayer.HasAuthority()) {
                SendData();
            } else {
                SyncOthersPlayer();
            }
        }
        private void SendData() {
            Debug.Log("SendData fue llamao con tipo: " + myPlayer.myType);
            CmdSendChosenType(myPlayer.myType);
        }

        private void SyncOthersPlayer() {
            Debug.Log("SyncOthersPlayer fue llamao con tipo: " + myPlayer.myType);

            myPlayer.myType = myLastType;
        }

        [Command]
        private void CmdSendChosenType(Type chosenType) {
            // I'm on the server

            myLastType = chosenType;

            RpcReceiveChosenType(myLastType);
        }

        [ClientRpc]
        private void RpcReceiveChosenType(Type type) {
            // I'm on the client

            if (!myPlayer.HasAuthority()) {
                // We don't need to update or predict any local player data
                myLastType = type;
            }
        }

    }
}
