using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLadderBottomStartInteractionState : PlayerInteractionState {
        Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            curInteractable = playerInteraction.currentInteractable;
            player.isClimbing = true;
            player.isPerformingAction = true;
            player.playerAnimatorManager.PlayAnimation("Ladder_StartBottom", player.isPerformingAction);
            targetPosition = curInteractable.GetComponent<Ladder>().GetInteractionStartingPosition();
            curInteractable.Interact();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.transform.position = Vector3.Slerp(player.transform.position, targetPosition, 12 * Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            //player.playerAnimatorManager.disableOnAnimatorMove = false;
            curInteractable = null;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (!player.isPerformingAction && player.transform.position == targetPosition) {
                player.cc.enabled = true;
                playerInteraction.pism.ChangeState(playerInteraction.pism.notInteractingState);
            }
        }
    }
}
