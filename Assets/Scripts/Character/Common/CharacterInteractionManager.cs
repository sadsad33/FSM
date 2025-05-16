using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterInteractionManager : MonoBehaviour {
        protected Collider interactionCollider;
        public Interactable currentInteractable;

        // PlayerLootingInteractionState�� ���
        // �÷��̾ �̵��ϴٰ� �������� ������ pmsm �� PlayerLightStoppingState Ȥ�� PlayerMediumStoppingState�� ����
        // �� ���¿����� isPerformingAction �� true ���� Ű���峪 ���콺�� �Է��� �����Ǹ� isPerformingAction �� false�� �Ǹ鼭 �����ϼ� �ְԵǴµ� �̸� ���� ���� ����
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