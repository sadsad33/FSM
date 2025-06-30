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
        // PlayerLootingInteractionState의 경우
        // 플레이어가 이동하다가 아이템을 줏으면 pmsm 은 PlayerLightStoppingState 혹은 PlayerMediumStoppingState로 전이
        // 두 상태에서는 isPerformingAction 이 true 여도 키보드나 마우스의 입력이 감지되면 isPerformingAction 이 false가 되면서 움직일수 있게되는데 이를 막기 위한 변수

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