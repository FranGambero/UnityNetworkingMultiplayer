using NetworkObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkObjects {
    public class TileController : MonoBehaviour {
        public Type tileType;
        public PlayerController playerController;

        public void setTypeFromTile() {
            playerController.SetGameType(tileType);
        }

    }
}
