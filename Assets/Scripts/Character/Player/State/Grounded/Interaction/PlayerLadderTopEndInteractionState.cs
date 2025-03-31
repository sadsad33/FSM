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
            targetPosition = playerInteraction.currentInteractable.GetComponent<Ladder>().GetClimbingStartPosition();
            
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //player.transform.position = Vector3.Slerp(player.transform.position, targetPosition, Time.deltaTime);
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 2 * Time.deltaTime);
            //Debug.Log("플레이어 좌표 : " + player.transform.position);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            player.playerInteractionManager.isInteracting = false;
            player.cc.enabled = true;
            player.isMoving = false;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (!player.isPerformingAction && player.transform.position == targetPosition) {
                player.isClimbing = false;
                playerInteraction.pism.ChangeState(playerInteraction.pism.notInteractingState);
            }
        }
    }
}
