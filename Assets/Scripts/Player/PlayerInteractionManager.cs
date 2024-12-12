using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionManager : MonoBehaviour {
        public Collider interactionCollider;
        public Interactable currentInteractable;

        private void Awake() {
            interactionCollider = GetComponent<Collider>();
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

        public void HandleInteraction() {
            if (currentInteractable == null) {
                if (PlayerUIManager.instance.interactionPopUp.activeSelf)
                    PlayerUIManager.instance.interactionPopUp.SetActive(false);
                if (PlayerUIManager.instance.itemPopUp.activeSelf)
                    PlayerUIManager.instance.itemPopUp.SetActive(false);
                return;
            }
            currentInteractable.Interact();
        }
    }
}