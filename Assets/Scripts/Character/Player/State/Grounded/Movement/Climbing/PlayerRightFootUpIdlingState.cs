using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerRightFootUpIdlingState : PlayerClimbingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (verticalInput > 0f) {
                if (player.isOnLadderTopEdge) {
                    player.playerInteractionManager.pism.ChangeState(player.playerInteractionManager.pism.ladderTopEndInteractionState);
                    player.pmsm.ChangeState(player.pmsm.idlingState);
                } else player.pmsm.ChangeState(player.pmsm.climbingUpState);
            } else if (verticalInput < 0f) {
                if (player.isOnLadderBottomEdge) {
                    player.playerInteractionManager.pism.ChangeState(player.playerInteractionManager.pism.ladderBottomEndInteractionState);
                    player.pmsm.ChangeState(player.pmsm.idlingState);
                } else player.pmsm.ChangeState(player.pmsm.climbingDownState);
            } else return;
        }
    }
}