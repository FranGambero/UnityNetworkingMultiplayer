using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkObjects {
    public class PlayerController : NetworkBehaviour {
        public Type myType;

        public GameObject choosePanel;
        public bool typeChoosen;

        private void Awake() {                    
            choosePanel.SetActive(true);
            typeChoosen = false;
        }

        public void SetType(Type type) {        // UwU
            myType = type;
            Debug.Log("Tras seleccionarlo: " + myType);
        }
    }
}
