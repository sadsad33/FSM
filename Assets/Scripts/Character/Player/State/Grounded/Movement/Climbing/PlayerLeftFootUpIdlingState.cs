using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLeftFootUpIdlingState : PlayerClimbingState {
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
                    player.psm.ChangeState(player.psm.idlingState);
                } else player.psm.ChangeState(player.psm.climbingUpState);
            } else if (verticalInput < 0f) {
                if (player.isOnLadderBottomEdge) {
                    player.playerInteractionManager.pism.ChangeState(player.playerInteractionManager.pism.ladderBottomEndInteractionState);
                    player.psm.ChangeState(player.psm.idlingState);
                } else player.psm.ChangeState(player.psm.climbingDownState);
            } else return;
        }
    }
}
