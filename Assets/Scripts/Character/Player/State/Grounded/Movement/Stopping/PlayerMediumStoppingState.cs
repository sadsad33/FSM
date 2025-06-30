using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerMediumStoppingState : PlayerStoppingState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isAttacking && !player.playerInteractionManager.isInteracting)
                player.playerAnimatorManager.PlayAnimation("Medium Stop", player.isMoving);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //player.playerInputManager.SprintInputTimer = 0f;
            if (!player.isAttacking && !player.playerInteractionManager.isInteracting) {
                if (moveAmount > 0) player.isMoving = false;
                if (player.playerInputManager.LightAttackInput) player.isMoving = false;
                if (player.playerInputManager.JumpInput) player.isMoving = false;
            }
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isMoving || player.playerInteractionManager.isInteracting) {
                player.pmsm.ChangeState(player.pmsm.idlingState);
            }
        }
    }
}