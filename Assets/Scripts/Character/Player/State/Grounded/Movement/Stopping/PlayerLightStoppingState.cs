using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerLightStoppingState : PlayerStoppingState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking)
                player.playerAnimatorManager.PlayAnimation("Light Stop", player.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (!player.isAttacking && !player.playerInteractionManager.isInteracting) {
                //if (moveAmount > 0 || player.isMoving) player.isPerformingAction = false;
                if (Input.anyKeyDown) player.isPerformingAction = false;
            }
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.pmsm.ChangeState(player.pmsm.idlingState);
            }
        }
    }
}