using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterInteractionManager : MonoBehaviour {
        protected Collider interactionCollider;
        public Interactable currentInteractable;

        // PlayerLootingInteractionState의 경우
        // 플레이어가 이동하다가 아이템을 줏으면 pmsm 은 PlayerLightStoppingState 혹은 PlayerMediumStoppingState로 전이
        // 두 상태에서는 isPerformingAction 이 true 여도 키보드나 마우스의 입력이 감지되면 isPerformingAction 이 false가 되면서 움직일수 있게되는데 이를 막기 위한 변수
        public bool isInteracting;
        protected virtual void Awake() {
            interactionCollider = GetComponent<Collider>();
        }

        protected virtual void OnTriggerStay(Collider other) {
        }

        protected virtual void OnTriggerExit(Collider other) {
        }
    }
}