using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerMoveToInteractingPositionState : PlayerInteractionState {
        public Vector3 targetPosition;
        public IState nextState;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.cc.enabled = false;
            player.playerAnimatorManager.disableOnAnimatorMove = true;
            curInteractable = playerInteraction.currentInteractable;
            player.isMoving = true;
            player.isPerformingAction = true;
            player.playerInteractionManager.isInteracting = true;
            targetPosition = curInteractable.GetComponent<Ladder>().GetInteractionStartingPosition();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.01f, Time.deltaTime);
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 2 * Time.deltaTime);
            
            Vector3 lookDir = -curInteractable.transform.right;
            lookDir.y = 0;
            if (lookDir != Vector3.zero) {
                Quaternion tr = Quaternion.LookRotation(lookDir);
                Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, 5 * Time.deltaTime);
                player.transform.rotation = targetRotation;
            }
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0f, Time.deltaTime);
            player.cc.enabled = true;
            player.playerAnimatorManager.disableOnAnimatorMove = false;
            player.isMoving = false;
            //player.isPerformingAction = false;
            player.playerInteractionManager.isInteracting = false;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (player.transform.position == targetPosition) {
                switch (nextState) {
                    case PlayerLadderBottomStartInteractionState:
                        playerInteraction.pism.ChangeState(playerInteraction.pism.ladderBottomStartInteractionState);
                        break;
                    case PlayerLadderTopStartInteractionState:
                        playerInteraction.pism.ChangeState(playerInteraction.pism.ladderTopStartInteractionState);
                        break;
                }
                player.pmsm.ChangeState(player.pmsm.rightFootUpIdlingState);
            }
        }
    }
}
