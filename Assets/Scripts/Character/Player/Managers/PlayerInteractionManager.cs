using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerInteractionManager : MonoBehaviour {
        public Collider interactionCollider;
        public Interactable currentInteractable;
        public PlayerInteractionStateMachine pism;
        [SerializeField] PlayerManager player;

        // PlayerLootingInteractionState의 경우
        // 플레이어가 이동하다가 아이템을 줏으면 pmsm 은 PlayerLightStoppingState 혹은 PlayerMediumStoppingState로 전이
        // 두 상태에서는 isPerformingAction 이 true 여도 키보드나 마우스의 입력이 감지되면 isPerformingAction 이 false가 되면서 움직일수 있게되는데 이를 막기 위한 변수
        public bool isInteracting;

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
                if (PlayerUIManager.instance.itemPopUp.activeSelf) return;
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