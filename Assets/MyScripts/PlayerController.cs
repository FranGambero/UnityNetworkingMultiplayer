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
            myType = Type.Undefined;
            typeChoosen = false;
            Instantiate(choosePanel, transform.position, transform.rotation);

            Debug.Log("ESTOY VIVX");
        }

        public void SetGameType(Type type) {
            myType = type;
            Debug.Log("Tengo asignado " + myType);
            typeChoosen = true;
        }

        public bool HasAuthority() {
            return hasAuthority;
        }
        
    }
}
