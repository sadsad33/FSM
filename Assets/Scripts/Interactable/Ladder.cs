using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace KBH {
    public class Ladder : Interactable {
        public bool isTop;
        [SerializeField] Vector3 climbingStartPosition;
        [SerializeField] Vector3 interactionStartingPosition;
        private void Awake() {
            climbingStartPosition = transform.parent.GetChild(1).GetComponent<Transform>().position;
            interactionStartingPosition = transform.parent.GetChild(2).GetComponent<Transform>().position;
        }

        public override void Interact(CharacterManager character) {
            //PlayerManager player = PlayerUIManager.instance.player;
            //player.playerAnimatorManager.disableOnAnimatorMove = true;
            //player.cc.enabled = false;
            //if (!isTop)
            //player.transform.forward = -transform.right;
            character.cc.enabled = false;
            if (!isTop)
                character.transform.forward = -transform.right;
        }

        public Vector3 GetInteractionStartingPosition() {
            return interactionStartingPosition;
        }

        public Vector3 GetClimbingStartPosition() {
            return climbingStartPosition;
        }
    }
}
