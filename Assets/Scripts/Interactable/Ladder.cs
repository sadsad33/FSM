using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class Ladder : Interactable {
        public bool isTop;
        //[SerializeField] Vector3 bottomStartingPosition;
        //[SerializeField] Vector3 topStartingPosition;
        [SerializeField] Vector3 interactionStartingPosition;
        [SerializeField] Vector3 climbingStartPosition;
        private void Awake() {
            //if (!isTop)
            //    bottomStartingPosition = transform.parent.GetChild(1).GetComponent<Transform>().position;
            //else topStartingPosition = transform.parent.GetChild(1).GetComponent<Transform>().position;
            interactionStartingPosition = transform.parent.GetChild(1).GetComponent<Transform>().position;
            climbingStartPosition = transform.parent.GetChild(2).GetComponent<Transform>().position;
        }

        public override void Interact() {
            PlayerManager player = PlayerUIManager.instance.player;
            //player.playerAnimatorManager.disableOnAnimatorMove = true;
            player.cc.enabled = false;
            if (!isTop)
                player.transform.forward = -transform.right;
        }

        public Vector3 GetClimbingStartPosition() {
            return climbingStartPosition;
        }

        public Vector3 GetInteractionStartingPosition() {
            //if (isTop) return topStartingPosition;
            //else return bottomStartingPosition;
            return interactionStartingPosition;
        }
    }
}
