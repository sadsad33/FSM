using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionManager : MonoBehaviour {
        public Collider interactionCollider;
        public Interactable currentInteractable;
        public PlayerInteractionStateMachine pism;
        [SerializeField] PlayerManager player;
        
        private void Awake() {
            interactionCollider = GetComponent<Collider>();
            player = GetComponentInParent<PlayerManager>();
            pism = new PlayerInteractionStateMachine(player);
        }

        private void Start() {
            pism.ChangeState(pism.notInteractingState);
        }

        private void Update() {
            pism.GetCurrentState().Stay(player);
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Interactable")) {
                currentInteractable = other.GetComponent<Interactable>();
                PlayerUIManager.instance.interactionPopUpMessage.text = currentInteractable.interactionMessage;
                PlayerUIManager.instance.interactionPopUp.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other) {
            currentInteractable = null;
            if (PlayerUIManager.instance.interactionPopUp.activeSelf) {
                PlayerUIManager.instance.interactionPopUp.SetActive(false); 
            }
        }
    }
}