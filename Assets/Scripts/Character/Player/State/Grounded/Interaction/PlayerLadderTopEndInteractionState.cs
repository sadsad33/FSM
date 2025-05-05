using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLadderTopEndInteractionState : PlayerInteractionState {
        Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.cc.enabled = false;
            player.isMoving = true;
            player.isPerformingAction = true;
            //player.playerInteractionManager.isInteracting = true;

            if (player.rightFootUp)
                player.playerAnimatorManager.PlayAnimation("Ladder_End_Top_RightFootUp", player.isPerformingAction);
            else
                player.playerAnimatorManager.PlayAnimation("Ladder_End_Top_LeftFootUp", player.isPerformingAction);
            targetPosition = playerInteraction.currentInteractable.GetComponent<Ladder>().GetInteractionStartingPosition();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (Vector3.Distance(player.transform.position, targetPosition) > 0.1f) {
                player.transform.position = Vector3.Slerp(player.transform.position, targetPosition, 2.5f * Time.deltaTime);
            }
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            //player.playerInteractionManager.isInteracting = false;
            player.cc.enabled = true;
            player.isMoving = false;
            player.isClimbing = false;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (!player.isPerformingAction && Vector3.Distance(player.transform.position, targetPosition) <= 0.1f) {
                playerInteraction.pism.ChangeState(playerInteraction.pism.notInteractingState);
            }
        }
    }
}
