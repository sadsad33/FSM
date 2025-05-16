using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLadderBottomEndInteractionState : PlayerInteractionState {
        Vector3 targetPosition;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            //player.pmsm.ChangeState(player.pmsm.idlingState);
            player.cc.enabled = false;
            player.isClimbing = false;
            player.isPerformingAction = true;
            //player.playerInteractionManager.isInteracting = true;
            if (player.rightFootUp)
                player.playerAnimatorManager.PlayAnimation("Ladder_End_Bottom_RightFootUp", player.isPerformingAction);
            else
                player.playerAnimatorManager.PlayAnimation("Ladder_End_Bottom_LeftFootUp", player.isPerformingAction);
            // 애니메이션이 종료된후 플레이어가 서 있을 좌표 대입
            targetPosition = playerInteraction.currentInteractable.GetComponent<Ladder>().GetInteractionStartingPosition();
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 2 * Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
            player.playerInteractionManager.isInteracting = false;
            player.cc.enabled = true;
        }

        public override void HandleInput(CharacterManager character) {
            base.HandleInput(character);
            if (!player.isPerformingAction)
                playerInteraction.pism.ChangeState(playerInteraction.pism.notInteractingState);
        }
    }
}
