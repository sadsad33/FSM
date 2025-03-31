using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerArmorModelsManager : MonoBehaviour {
        PlayerManager player;
        

        private void Awake() {
            player = GetComponent<PlayerManager>();
            
        }

        private void Start() {
            
        }

        

    }
}
