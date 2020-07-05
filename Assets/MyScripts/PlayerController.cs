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
            Debug.Log("ESTOY VIVX");

            Instantiate(choosePanel, transform.position, transform.rotation);

            typeChoosen = false;
        }

        public void SetGameType(Type type) {
            myType = type;
            Debug.Log("tengo asignado" + myType);
        }

        public bool HasAuthority() {
            return hasAuthority;
        }
        
    }
}
