using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLightLandingState : PlayerLandingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking && !player.isPerformingAction) {
                player.isMoving = true;
                player.playerAnimatorManager.PlayAnimation("Light Landing", player.isMoving);
            }
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (!player.isAttacking && !player.isPerformingAction) {
                if (moveAmount > 0) player.isMoving = false;
            }
            //if (moveAmount > 0 || player.isMoving) player.isMoving = false;
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isMoving)
                player.psm.ChangeState(player.psm.idlingState);
        }
    }
}