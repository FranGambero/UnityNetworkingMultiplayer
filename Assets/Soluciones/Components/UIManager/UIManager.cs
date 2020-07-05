using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace Soluciones
{
    public class UIManager : NetworkBehaviour
    {
        public static UIManager Instance;

        public Text playersCountText;
        public Text ButtonPlayText;
        public GameObject MenuCanvas;

        [SyncVar]
        public int AmountOfPlayers = 0;

        private void Awake()
        {
            Instance = this;    
        }

        private void Update()
        {
            if (isServer)
            {
                if (AmountOfPlayers != NetworkManager.singleton.numPlayers)
                {
                    CmdUpdatePlayers(NetworkManager.singleton.numPlayers);
                }
            }
            else
            {
                ButtonPlayText.text = "Waiting for host";
            }
        }

        private void OnEnable()
        {
            StartCoroutine(UpdateCoroutine());
        }

        public void UpdatePlayersCount()
        {
            playersCountText.text = AmountOfPlayers + " / 12  Players joined";
        }

        public IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                UpdatePlayersCount();
            }
        }

        public void EnableMenu()
        {
            MenuCanvas.SetActive(true);
        }

        public void DisableMenu()
        {
            MenuCanvas.SetActive(false);
        }

        [Command]
        private void CmdUpdatePlayers(int numPlayers)
        {
            AmountOfPlayers = numPlayers;
        }
    }
}