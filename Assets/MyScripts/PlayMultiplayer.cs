using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkObjects {
    public class PlayMultiplayer : NetworkBehaviour {
        public bool playerReady { get; private set; }

        private PlayerController myPlayer;

        private void Awake() {
            myPlayer = GetComponent<PlayerController>();
        }

        public void Action() {
            if (!playerReady) {
                CmdAction();
                playerReady = true;
            }
        }

        [Command]
        private void CmdAction() {
            // I'm on the server
            //NetworkServer.spa
        }
    }
}