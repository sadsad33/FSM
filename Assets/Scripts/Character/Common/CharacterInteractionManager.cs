using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterInteractionManager : MonoBehaviour {
        public CharacterManager character;
        protected Collider interactionCollider;
        public Interactable currentInteractable;
        [SerializeField] protected Transform fatalBlowRaycastStartingPosition;
        [SerializeField] protected Transform enemyRipostePosition;
        [SerializeField] protected Transform enemyBackstabPosition;
        // PlayerLootingInteractionState�� ���
        // �÷��̾ �̵��ϴٰ� �������� ������ pmsm �� PlayerLightStoppingState Ȥ�� PlayerMediumStoppingState�� ����
        // �� ���¿����� isPerformingAction �� true ���� Ű���峪 ���콺�� �Է��� �����Ǹ� isPerformingAction �� false�� �Ǹ鼭 �����ϼ� �ְԵǴµ� �̸� ���� ���� ����

        LayerMask riposteLayer = 1 << 13;
        LayerMask backstabLayer = 1 << 14;
        public bool isInteracting;
        protected virtual void Awake() {
            character = GetComponentInParent<CharacterManager>();
            interactionCollider = GetComponent<Collider>();
            fatalBlowRaycastStartingPosition = transform.GetChild(3).GetComponent<Transform>();
            enemyRipostePosition = transform.GetChild(4).GetComponent<Transform>();
            enemyBackstabPosition = transform.GetChild(5).GetComponent<Transform>();
        }

        protected virtual void OnTriggerStay(Collider other) {
        }

        protected virtual void OnTriggerExit(Collider other) {
        }

        public Vector3 GetFatalBlowRaycastStartingPosition() => fatalBlowRaycastStartingPosition.position;

        public LayerMask GetRiposteLayerMask() => riposteLayer;
        
        public LayerMask GetBackstabLayerMask() => backstabLayer;

        public Transform GetRipostingCharacterStandingPosition() => enemyRipostePosition;

        public Transform GetBackstabbingCharacterStandingPosition() => enemyBackstabPosition;

        //public virtual bool CheckFatalBlowCollider() {
        //    //Debug.DrawRay(riposteRaycastStartingPosition.position, character.transform.forward, Color.red);
        //    Ray ray = new(riposteRaycastStartingPosition.position, character.transform.forward);
        //    if (Physics.Raycast(ray, 0.5f, riposteLayer)) {
        //        character.canRiposteNow = true;
        //        return true;
        //    } else if (Physics.Raycast(ray, 0.5f, backStabLayer)) {
        //        character.canBackstabNow = true;
        //        return true;
        //    }
        //    return false;
        //}
    }
}