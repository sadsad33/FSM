using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionManager : MonoBehaviour {
        public Collider interactionCollider;
        public Interactable currentInteractable;

        PlayerManager player;
        private void Awake() {
            interactionCollider = GetComponent<Collider>();
        }

        private void Start() {
            player = PlayerUIManager.instance.player;
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Interactable")) {
                currentInteractable = other.GetComponent<Interactable>();
                PlayerUIManager.instance.interactionPopUpMessage.text = currentInteractable.interactionMessage;
                PlayerUIManager.instance.interactionPopUp.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (PlayerUIManager.instance.interactionPopUp.activeSelf) {
                currentInteractable = null;
                PlayerUIManager.instance.interactionPopUp.SetActive(false);
            }
        }

        public void HandleInteraction() {
            if (currentInteractable == null) return;
            Debug.Log("æ∆¿Ã≈€ »πµÊ!");
            currentInteractable.Interact();
        }
    }
}