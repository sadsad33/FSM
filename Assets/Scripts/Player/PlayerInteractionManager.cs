using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionManager : MonoBehaviour {
        public Collider interactionCollider;

        private void Awake() {
            interactionCollider = GetComponent<Collider>();
        }

        private void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Character"))
                Debug.Log(other.gameObject);
        }
    }
}