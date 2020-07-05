using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Soluciones
{
    public class PlayerConnection : NetworkBehaviour
    {
        public TankController TankPrefab;

        public string PlayerID { get; private set; }

        public int points;

        private void Start()
        {

        }

        public void SpawnTank()
        {
            if (isLocalPlayer)
            {
                CmdSpawnTank();
            }
        }

        public void SetPlayerID(string id)
        {
            PlayerID = id;
        }

        [Command]
        void CmdSpawnTank()
        {
            // This will be executed on the server
            TankController tank = Instantiate(TankPrefab, transform.position, transform.rotation);
            tank.SetOwner(this);

            // Object exist only on server. We create it on clients too
            NetworkServer.SpawnWithClientAuthority(tank.gameObject, connectionToClient);
        }
    }
}