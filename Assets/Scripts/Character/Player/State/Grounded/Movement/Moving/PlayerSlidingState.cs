using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerSlidingState : PlayerMovingState {

        Vector3 direction;
        float speed;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.playerStatsManager.DeductStamina(25f);
            player.consumingStamina = true;
            moveSpeedModifier = 2f;
            player.pasm.ChangeState(player.pasm.slidingActionIdlingState);
            player.isMoving = true;
            player.canSliding = false;
            player.playerAnimatorManager.PlayAnimation("Sliding", player.isMoving);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            direction = player.playerAnimatorManager.animator.deltaPosition / Time.deltaTime;
            speed = direction.magnitude;
            direction.Normalize();
            player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            player.isMoving = false;
            player.consumingStamina = false;
            player.staminaRegenerateTimer = 0f;
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.canAttackDuringAction && player.playerInputManager.LightAttackInput) {
                if (player.playerStatsManager.currentStamina <= 10f) return;
                player.psm.slidingAttackState.SetVelocity(direction, speed);
                player.psm.ChangeState(player.psm.slidingAttackState);
            } else if (!player.isMoving) {
                player.psm.ChangeState(player.psm.idlingState);
            }
        }
    }
}