using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionManager : CharacterInteractionManager {

        public PlayerInteractionStateMachine pism;
        [SerializeField] PlayerManager player;
        public GameObject riposteCollider;
        public GameObject backStabCollider;

        protected override void Awake() {
            base.Awake();
            player = GetComponentInParent<PlayerManager>();
            pism = new PlayerInteractionStateMachine(player);
            riposteCollider = transform.GetChild(1).gameObject;
            riposteCollider.SetActive(false);
            backStabCollider = transform.GetChild(2).gameObject;
        }

        private void Start() {
            pism.ChangeState(pism.notInteractingState);
        }

        private void Update() {
            pism.GetCurrentState().Stay(player);
        }

        protected override void OnTriggerStay(Collider other) {
            base.OnTriggerStay(other);
            if (other.CompareTag("Interactable")) {
                if (PlayerUIManager.instance.itemPopUp.activeSelf) return;
                currentInteractable = other.GetComponent<Interactable>();
                PlayerUIManager.instance.interactionPopUpMessage.text = currentInteractable.interactionMessage;
                PlayerUIManager.instance.interactionPopUp.SetActive(true);
            }
        }

        protected override void OnTriggerExit(Collider other) {
            base.OnTriggerExit(other);
            if (other.CompareTag("Interactable")) {
                currentInteractable = null;
                if (PlayerUIManager.instance.interactionPopUp.activeSelf) {
                    PlayerUIManager.instance.interactionPopUp.SetActive(false);
                }
            }
        }

        
    }
}