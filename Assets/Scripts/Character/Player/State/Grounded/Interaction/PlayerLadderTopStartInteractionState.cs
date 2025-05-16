using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLadderTopStartInteractionState : PlayerInteractionState {
        Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //player.playerAnimatorManager.disableOnAnimatorMove = true;
            curInteractable = playerInteraction.currentInteractable;
            player.isClimbing = true;
            player.isPerformingAction = true;
            player.playerInteractionManager.isInteracting = true;

            player.playerAnimatorManager.PlayAnimation("Ladder_StartTop", player.isPerformingAction);
            targetPosition = curInteractable.GetComponent<Ladder>().GetClimbingStartPosition();
            curInteractable.Interact(player);
            //Debug.Log(Quaternion.Euler(-curInteractable.transform.right));
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            
            //Debug.Log(curInteractable);
            Quaternion tr = Quaternion.LookRotation(-curInteractable.transform.right);
            Quaternion targetRotation = Quaternion.Lerp(player.transform.rotation, tr, 5 * Time.deltaTime);
            player.transform.SetPositionAndRotation(Vector3.Slerp(player.transform.position, targetPosition, 12 * Time.deltaTime), targetRotation);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            player.playerInteractionManager.isInteracting = false;
            //player.playerAnimatorManager.disableOnAnimatorMove = true;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (!player.isPerformingAction && player.transform.position == targetPosition) {
                Debug.Log("How");
                player.cc.enabled = true;
                playerInteraction.pism.ChangeState(playerInteraction.pism.notInteractingState);
            }
        }
    }
}