using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Soluciones
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void StartGame()
        {
            if (isServer)
            {
                CmdStartGame();
            }
        }

        private List<PlayerConnection> GetCurrentPlayers()
        {
            PlayerConnection[] foundPlayers = FindObjectsOfType<PlayerConnection>();

            return new List<PlayerConnection>(foundPlayers);
        }

        private void SpawnPlayerTanks(List<PlayerConnection> players)
        {
            foreach (PlayerConnection player in players)
            {
                player.SpawnTank();
            }
        }

        private void InitPlay()
        {
            List<PlayerConnection> currentPlayers = GetCurrentPlayers();
            SpawnPlayerTanks(currentPlayers);
            UIManager.Instance.DisableMenu();
        }

        [Command]
        private void CmdStartGame()
        {
            RpcStartGame();
        }

        [ClientRpc]
        private void RpcStartGame()
        {
            InitPlay();
        }

        public void RespawnTank(TankController tank)
        {
            StartCoroutine(RespawnCoroutine(tank));
        }

        private IEnumerator RespawnCoroutine(TankController tank)
        {
            yield return new WaitForSeconds(2f);
            tank.Respawn();
        }
    }
}
